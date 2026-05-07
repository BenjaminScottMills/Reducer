using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class ImportFolderContents : MonoBehaviour
{
    public Vector3 firstEntryLocation;
    public Vector3 entryListOffset;
    public GameObject folderEntryPrefab;
    public GameObject reducerEntryPrefab;
    public RectTransform scrollViewContent;
    public ImportMenu.DirectoryLevel currLevel;
    public ImportMenu importMenu;
    public GameObject dummySolutionPrefab;
    public GameObject solutionContainerPrefab;
    public string currDirectory;
    List<GameObject> entries;
    float baseScrollViewHeight;

    // Start is called before the first frame update
    void Awake()
    {
        baseScrollViewHeight = scrollViewContent.sizeDelta.y;
        entries = new();
    }

    public void ClearContents()
    {
        foreach (var entry in entries)
        {
            Destroy(entry);
        }
        entries.Clear();
    }

    public void LoadFolderContents(List<ReducerOrFolder> contents)
    {
        foreach (var rof in contents)
        {
            if (rof.IsReducer())
            {
                var newEntry = Instantiate(reducerEntryPrefab, Vector3.zero, Quaternion.identity, scrollViewContent).GetComponent<ImportReducerEntry>();
                newEntry.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                newEntry.importMenu = importMenu;
                newEntry.Initialise(rof.r);
                entries.Add(newEntry.gameObject);
            }
            else
            {
                var newEntry = Instantiate(folderEntryPrefab, Vector3.zero, Quaternion.identity, scrollViewContent).GetComponent<ImportFolderEntry>();
                newEntry.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                newEntry.importFolderContents = this;
                newEntry.InitialiseRFolder(rof.f);
                entries.Add(newEntry.gameObject);
            }
        }
    }

    public void LoadFolderContents(string folderPath)
    {
        currDirectory = folderPath;
        string[] contents = null;
        switch (currLevel)
        {
            case ImportMenu.DirectoryLevel.chapters:
                currLevel = ImportMenu.DirectoryLevel.levels;
                contents = Directory.GetDirectories(currDirectory).Select((s) => Path.GetFileName(s)).ToArray();
                break;
            case ImportMenu.DirectoryLevel.levels:
                currDirectory = Path.Combine(currDirectory, "solutions");
                currLevel = ImportMenu.DirectoryLevel.solutions;
                contents = Directory.GetDirectories(currDirectory).Select((s) => Path.GetFileName(s)).ToArray();
                break;
            case ImportMenu.DirectoryLevel.solutions:
                importMenu.solutionContainer = Instantiate(solutionContainerPrefab, Vector3.zero, Quaternion.identity, importMenu.transform);
                importMenu.loadedSolution = Instantiate(dummySolutionPrefab, Vector3.zero, Quaternion.identity, importMenu.solutionContainer.transform).GetComponent<Solution>();
                importMenu.loadedSolution.CopyFixedReducers(importMenu.solution);
                importMenu.loadedSolution.CopySettings(importMenu.solution);
                importMenu.loadedSolution.mouseNode = importMenu.solution.mouseNode;
                importMenu.loadedSolution.LoadFromSerialisedForImporting(JsonUtility.FromJson<SolutionSerialise>(File.ReadAllText(Path.Combine(currDirectory, "solution.json"))));
                LoadFolderContents(importMenu.loadedSolution.contents);
                PositionButtons();
                return;
        }

        CreateFolderButtons(contents);
        PositionButtons();
    }

    void CreateFolderButtons(string[] contents)
    {
        Array.Sort(contents);

        foreach (var inner in contents)
        {
            if (false) // if inner has not been completed. Consider currLevel when deciding
            {
                return;
            }
            var newEntry = Instantiate(folderEntryPrefab, Vector3.zero, Quaternion.identity, scrollViewContent).GetComponent<ImportFolderEntry>();
            newEntry.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            newEntry.importFolderContents = this;
            newEntry.InitialiseDirectory(inner);
            entries.Add(newEntry.gameObject);
        }
    }

    void PositionButtons()
    {
        Vector3 newButtonPos = firstEntryLocation;

        scrollViewContent.sizeDelta = new Vector2(scrollViewContent.sizeDelta.x, baseScrollViewHeight + Math.Abs(entryListOffset.y * (entries.Count - 1)));

        foreach (GameObject entry in entries)
        {
            entry.transform.localPosition = new Vector3(entry.transform.localPosition.x, newButtonPos.y);
            newButtonPos += entryListOffset;
        }
    }

    public void Initialise()
    {
        ClearContents();
        currDirectory = Path.Combine(Application.persistentDataPath, "chapters");
        currLevel = ImportMenu.DirectoryLevel.chapters;
        string[] contents = Directory.GetDirectories(currDirectory).Select((s) => Path.GetFileName(s)).ToArray();
        CreateFolderButtons(contents);
        PositionButtons();
    }
}
