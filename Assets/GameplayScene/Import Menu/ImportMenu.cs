using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public GameObject dummySolutionPrefab;
    public GameObject solutionContainerPrefab;

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

    public bool MatchesSelectedReducer(Reducer r)
    {
        return r != null && r == selectedReducer;
    }

    public void LoadSolution(string solutionPath)
    {
        if (solutionContainer != null) Destroy(solutionContainer);
        solutionContainer = Instantiate(solutionContainerPrefab, Vector3.zero, Quaternion.identity, transform);
        loadedSolution = Instantiate(dummySolutionPrefab, Vector3.zero, Quaternion.identity, solutionContainer.transform).GetComponent<Solution>();
        loadedSolution.CopyFixedReducers(solution);
        loadedSolution.CopySettings(solution);
        loadedSolution.mouseNode = solution.mouseNode;
        loadedSolution.LoadFromSerialisedForImporting(JsonUtility.FromJson<SolutionSerialise>(File.ReadAllText(Path.Combine(solutionPath, "solution.json")))); // Potentially do async stuff if this ends up being problematic. Could cause more issues though so be careful and test stuff like clicking buttons really really fast.
    }

    public Reducer SetSelectedReducer(ImportFolderContents.FavouritedReducer favouritedReducer)
    {
        LoadSolution(favouritedReducer.solutionPath);
        Reducer foundRed = loadedSolution.reducers.First(r => r.id == favouritedReducer.reducerId);
        if (foundRed == null)
        {
            throw new Exception("The solution referenced by a favourites entry does not have a matching reducer");
        }
        SetSelectedReducer(foundRed);
        return foundRed;
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
