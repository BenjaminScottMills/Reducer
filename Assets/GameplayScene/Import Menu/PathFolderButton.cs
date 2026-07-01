using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PathFolderButton : MonoBehaviour
{
    public RectTransform rectTransform;
    public Text text;
    public string targetDirectory;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup(ImportMenu.DirectoryLevel level, RFolder folder, string currPath)
    {
        switch (level)
        {
            case ImportMenu.DirectoryLevel.chapters:
                targetDirectory = Path.Combine(Application.persistentDataPath, "chapters");
                text.text = NameToButtonText("Chapters");
                break;
            case ImportMenu.DirectoryLevel.levels:
                Debug.Log(currPath);
                targetDirectory = NthParent(currPath, 3);
                text.text = NameToButtonText(Path.GetFileName(targetDirectory).Substring(2));
                break;
            case ImportMenu.DirectoryLevel.solutions:
                Debug.Log(currPath);
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
                }
                break;
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }

    public void DisableClickEffect()
    {
        
    }

    public void SetupFavourites()
    {
        
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
