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
    bool isFavourites;
    public Text buttonText;
    public ImportFolderContents importFolderContents;

    public void InitialiseDirectory(string directory)
    {
        isDirectory = true;
        isFavourites = false;
        innerDirectory = directory;
        buttonText.text = directory[2..];
    }

    public void InitialiseFavourites()
    {
        isDirectory = false;
        isFavourites = true;
        buttonText.text = "Favourites";
        // favouriteVisual toggled on here
    }

    public void InitialiseRFolder(RFolder rFolder)
    {
        isDirectory = false;
        isFavourites = false;
        innerRFolder = rFolder;
        buttonText.text = rFolder.folderName;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        importFolderContents.ClearContents();
        if (isFavourites)
        {
            importFolderContents.LoadFolderContentsFavourites();
        }
        else if (isDirectory)
        {
            importFolderContents.LoadFolderContents(Path.Combine(importFolderContents.currDirectory, innerDirectory));
        }
        else
        {
            importFolderContents.LoadFolderContents(innerRFolder.contents, innerRFolder);
        }
    }
}
