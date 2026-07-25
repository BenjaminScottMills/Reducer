using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImportMenu : MonoBehaviour
{
    public enum DirectoryLevel {chapters, levels, solutions, specificSolution};
    public Solution solution;
    public Solution loadedSolution;
    public GameObject solutionContainer;
    public ImportFolderContents importFolderContents;
    public PathDisplay pathDisplay;
    public GameObject selectedReducerInfo;
    public Reducer selectedReducer;
    public Text reducerNameText;
    public Text reducerDescriptionText;
    public GenericButton importReducerButton;
    public ImportMenuNodeDisplay nodeDisplay;

    void Start()
    {
        importReducerButton.invoker = new ImportReducerInvoker{importMenu = this};
    }
    class ImportReducerInvoker : GenericButton.MethodInvoker
    {
        public ImportMenu importMenu;
        public override void InvokeMethod()
        {
            importMenu.ImportReducer();
        }
    }

    public bool IsReady()
    {
        return importFolderContents.FavouritesLoaded();
    }

    public void Initialise()
    {
        selectedReducer = null;
        selectedReducerInfo.SetActive(false);

        importFolderContents.Initialise();
    }

    public void CancelImport()
    {
        gameObject.SetActive(false);
        Destroy(solutionContainer);
        solution.SetInteractable();
    }

    public void SetSelectedReducer(Reducer reducer)
    {
        selectedReducer = reducer;
        reducerDescriptionText.text = reducer.description;
        reducerNameText.text = reducer.rName;
        nodeDisplay.ResetToReducer(reducer);
        selectedReducerInfo.SetActive(true);
    }

    public void ImportReducer()
    {
        // do stuff based on selectedReducer.
    }
}
