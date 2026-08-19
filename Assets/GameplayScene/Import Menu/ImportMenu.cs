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
        solutionContainer = null;
        solution.SetInteractable();
    }

    public bool MatchesSelectedReducer(Reducer r)
    {
        return r != null && r == selectedReducer;
    }

    public bool MatchesSelectedReducer(ImportFolderContents.FavouritedReducer r)
    {
        return r != null && r.MatchesReducer(selectedReducer);
    }

    public void LoadSolution(string solutionPath)
    {
        if (solutionContainer != null)
        {
            if (loadedSolution != null && Path.GetFullPath(loadedSolution.solutionPath) == Path.GetFullPath(solutionPath))
            {
                return;
            }
            Destroy(solutionContainer);
        }
        solutionContainer = Instantiate(solutionContainerPrefab, Vector3.zero, Quaternion.identity, transform);
        loadedSolution = Instantiate(dummySolutionPrefab, Vector3.zero, Quaternion.identity, solutionContainer.transform).GetComponent<Solution>();
        loadedSolution.CopyFixedReducers(solution);
        loadedSolution.mouseNode = solution.mouseNode;
        loadedSolution.solutionPath = solutionPath;
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
        List<Reducer> dependencies = new List<Reducer>();
        HashSet<uint> addedIds = new HashSet<uint>{selectedReducer.id};
        Queue<Reducer> reducersToProcess = new Queue<Reducer>();
        reducersToProcess.Enqueue(selectedReducer);

        while (reducersToProcess.Count > 0)
        {
            Reducer currReducer = reducersToProcess.Dequeue();
            currReducer.transform.SetParent(solution.transform.parent, true);
            currReducer.child.transform.SetParent(solution.transform.parent, true);
            if (loadedSolution.ReducerIsFixed(currReducer))
            {
                currReducer.foregroundSprite = 0;
                currReducer.rName = currReducer.rName + " - from " + loadedSolution.sName;
            }

            foreach (Node node in currReducer.nodes)
            {
                node.transform.SetParent(solution.transform.parent, true);
                if (node.nextConnector != null) node.nextConnector.transform.SetParent(solution.transform.parent, true);
                if (node.reducer.id > 30 && !node.reducer.isChild && !addedIds.Contains(node.reducer.id))
                {
                    dependencies.Add(node.reducer);
                    addedIds.Add(node.reducer.id);
                    reducersToProcess.Enqueue(node.reducer);
                }
            }

            foreach (Node node in currReducer.child.nodes)
            {
                node.transform.SetParent(solution.transform.parent, true);
                if (node.nextConnector != null) node.nextConnector.transform.SetParent(solution.transform.parent, true);
                if (node.reducer.id > 30 && !node.reducer.isChild && !addedIds.Contains(node.reducer.id))
                {
                    dependencies.Add(node.reducer);
                    addedIds.Add(node.reducer.id);
                    reducersToProcess.Enqueue(node.reducer);
                }
            }
        }

        bool createFolder = dependencies.Count > 0;
        RFolder newFolder = createFolder ? new RFolder(solution, solution.currentFolder) : null;
        if (createFolder) newFolder.folderName = selectedReducer.rName + " - Dependencies";
        foreach (Reducer r in dependencies)
        {
            r.id = solution.idCounter;
            solution.idCounter++;
            r.solution = solution;
            r.folder = newFolder;
            newFolder.contents.Add(new ReducerOrFolder(r));
            r.child.ChildInit(r);
        }
        selectedReducer.id = solution.idCounter;
        solution.idCounter++;
        selectedReducer.solution = solution;
        selectedReducer.folder = solution.currentFolder;
        selectedReducer.child.ChildInit(selectedReducer);

        if (solution.currentFolder != null)
        {
            solution.currentFolder.contents.Add(new ReducerOrFolder(selectedReducer));
            if (createFolder) solution.currentFolder.contents.Add(new ReducerOrFolder(newFolder));
        }
        else
        {
            solution.contents.Add(new ReducerOrFolder(selectedReducer));
            if (createFolder) solution.contents.Add(new ReducerOrFolder(newFolder));
        }

        ReducerButton newReducerButton = solution.customReducerList.AddReducerButton(selectedReducer);
        if (createFolder) solution.customReducerList.AddFolderButton(newFolder);

        newReducerButton.EnableUpdateMenu();

        CancelImport();
    }
}
