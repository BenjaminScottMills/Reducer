using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImportMenu : MonoBehaviour
{
    public enum DirectoryLevel {chapters, levels, solutions};
    public Solution solution;
    public Solution loadedSolution;
    public GameObject solutionContainer;
    public ImportFolderContents importFolderContents;

    public bool IsReady()
    {
        return importFolderContents.FavouritesLoaded();
    }

    public void Initialise()
    {
        importFolderContents.Initialise();
    }

    public void CancelImport()
    {
        gameObject.SetActive(false);
        Destroy(solutionContainer);
        solution.SetInteractable();
    }

    public void SetActiveReducer(Reducer reducer)
    {
        
    }
}
