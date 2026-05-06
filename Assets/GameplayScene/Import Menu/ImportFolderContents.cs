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
    public string currDirectory;
    List<GameObject> entries;
    float baseScrollViewHeight;

    // Start is called before the first frame update
    void Start()
    {
        baseScrollViewHeight = scrollViewContent.sizeDelta.y;
        entries = new();

        // levelNameZone.text = Path.GetFileName(levelPath).Substring(2);
        // string solutionsPath = Path.Combine(levelPath, "solutions");
        // string[] solutions = Directory.GetDirectories(solutionsPath);
        // Array.Sort(solutions);

        // PositionSolutionButtons(true);
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
                currLevel = ImportMenu.DirectoryLevel.solutions;
                contents = Directory.GetDirectories(Path.Combine(currDirectory, "solutions")).Select((s) => Path.GetFileName(s)).ToArray();
                break;
            case ImportMenu.DirectoryLevel.solutions:
                // load reducer and stuff
                return;
        }

        CreateFolderButtons(contents);
        PositionButtons();
    }

    void CreateFolderButtons(string[] contents)
    {
        Array.Sort(contents);
        // Vector3 middle = 

        foreach (var inner in contents)
        {
            if (false) // if inner has not been completed. Consider currLevel when deciding
            {
                return;
            }
            Instantiate(folderEntryPrefab);
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

    // public void AddSolution()
    // {
    //     int newNumber;
    //     if (solutionButtons.Count == 0) newNumber = 0;
    //     else newNumber = solutionButtons.Last().levelNumber + 1;

    //     string newDirectory = Path.Combine(levelPath, "solutions", (newNumber < 10 ? "0" : "") + newNumber + "New Solution");
    //     Directory.CreateDirectory(newDirectory);

    //     File.WriteAllText(Path.Combine(newDirectory, "status.json"), JsonUtility.ToJson(new ChapterMenu.LevelStatus{completed = false}));

    //     SolutionButton sb = Instantiate(solutionButtonPrefab, addSolutionButton.transform.position, Quaternion.identity, scrollViewContent).GetComponent<SolutionButton>();
    //     sb.SetSolution(newDirectory, "New Solution", newNumber, Path.Combine(levelPath, "solutions")); // First 2 characters are for ordering, ie "04".
    //     sb.levelMenu = this;
    //     solutionButtons.Add(sb);

    //     PositionSolutionButtons(false);
    // }

    // Update is called once per frame
    void Update()
    {
        
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
