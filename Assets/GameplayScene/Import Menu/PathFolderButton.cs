using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PathFolderButton : MonoBehaviour
{
    public ImportFolderContents importFolderContents;
    public RectTransform rectTransform;
    public Text text;
    public string targetDirectory;
    public RFolder targetFolder;
    public bool isFavourite = false;
    public ImportMenu.DirectoryLevel targetLevel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup(ImportMenu.DirectoryLevel level, RFolder folder, string currPath, ImportFolderContents ifc)
    {
        importFolderContents = ifc;
        targetLevel = level;
        switch (level)
        {
            case ImportMenu.DirectoryLevel.chapters:
                targetDirectory = Path.Combine(Application.persistentDataPath, "chapters");
                text.text = NameToButtonText("Chapters");
                break;
            case ImportMenu.DirectoryLevel.levels:
                targetDirectory = NthParent(currPath, 3);
                text.text = NameToButtonText(Path.GetFileName(targetDirectory).Substring(2));
                break;
            case ImportMenu.DirectoryLevel.solutions:
                targetDirectory = NthParent(currPath, 2);
                text.text = NameToButtonText(Path.GetFileName(targetDirectory).Substring(2));
                break;
            case ImportMenu.DirectoryLevel.specificSolution:
                targetDirectory = currPath;
                if (folder == null)
                {
                    text.text = NameToButtonText(Path.GetFileName(targetDirectory).Substring(2));
                }
                else
                {
                    text.text = NameToButtonText(folder.folderName);
                    targetFolder = folder;
                }
                break;
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }

    public void SetupFavourites()
    {
        isFavourite = true;
        text.text = NameToButtonText("Favourites");
    }

    public void OnClick()
    {
        importFolderContents.ClearContents();
        if (isFavourite)
        {
            importFolderContents.LoadFolderContentsFavourites();
        }
        else if (targetLevel == ImportMenu.DirectoryLevel.specificSolution)
        {
            List<ReducerOrFolder> contents = null;
            if (targetFolder == null)
            {
                contents = importFolderContents.importMenu.loadedSolution.contents;
            }
            else
            {
                contents = targetFolder.contents;
            }
            importFolderContents.LoadFolderContents(contents, targetFolder);
        }
        else
        {
            importFolderContents.LoadFolderContents(targetDirectory, targetLevel);
        }
    }

    static string NameToButtonText(string str)
    {
        return " " + str + "  / ";
    }

    static string NthParent(string path, int n)
    {
        while (n > 0)
        {
            path = Path.GetDirectoryName(path);
            n--;
        }
        return path;
    }
}
