using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImportFolderEntry : MonoBehaviour, IPointerClickHandler
{
    string innerDirectory;
    RFolder innerRFolder;
    bool isDirectory;
    public Text buttonText;
    public ImportFolderContents importFolderContents;

    public void InitialiseDirectory(string directory)
    {
        isDirectory = true;
        innerDirectory = directory;
        buttonText.text = directory[2..];
    }

    public void InitialiseRFolder(RFolder rFolder)
    {
        isDirectory = false;
        innerRFolder = rFolder;
        buttonText.text = rFolder.folderName;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        importFolderContents.ClearContents();
        if (isDirectory)
        {
            importFolderContents.LoadFolderContents(Path.Combine(importFolderContents.currDirectory, innerDirectory));
        }
        else
        {
            importFolderContents.LoadFolderContents(innerRFolder.contents);
        }
    }
}
