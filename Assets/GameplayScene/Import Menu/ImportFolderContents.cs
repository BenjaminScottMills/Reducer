using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
    public List<FavouritedReducer> favouritedReducers;
    Task favouriteLoadingTask;
    List<GameObject> entries;
    float baseScrollViewHeight;

    // Start is called before the first frame update
    void Awake()
    {
        baseScrollViewHeight = scrollViewContent.sizeDelta.y;
        entries = new();
    }

    public void StartReadingFavourites()
    {
        favouriteLoadingTask = ReadFavourites();
    }

    public bool FavouritesLoaded()
    {
        return favouriteLoadingTask?.IsCompleted ?? false;
    }

    public void ClearContents()
    {
        foreach (var entry in entries)
        {
            Destroy(entry);
        }
        entries.Clear();
    }

    public void LoadFolderContentsFavourites()
    {
        for (int i = favouritedReducers.Count - 1; i >= 0; --i)
        {
            var newEntry = Instantiate(reducerEntryPrefab, Vector3.zero, Quaternion.identity, scrollViewContent).GetComponent<ImportReducerEntry>();
            newEntry.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            newEntry.importMenu = importMenu;
            newEntry.Initialise(favouritedReducers[i]);
            entries.Add(newEntry.gameObject);
        }

        PositionButtons();
        importMenu.pathDisplay.CreateFolderButtonsFavourites();
    }

    public void LoadFolderContents(List<ReducerOrFolder> contents, RFolder currentFolder)
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

        PositionButtons();
        importMenu.pathDisplay.CreateFolderButtons(currLevel, currDirectory, currentFolder);
    }

    public void LoadFolderContents(string folderPath)
    {
        ImportMenu.DirectoryLevel newLevel;
        switch(currLevel)
        {
            case ImportMenu.DirectoryLevel.chapters:
                newLevel = ImportMenu.DirectoryLevel.levels;
                break;
            case ImportMenu.DirectoryLevel.levels:
                newLevel = ImportMenu.DirectoryLevel.solutions;
                break;
            case ImportMenu.DirectoryLevel.solutions:
                newLevel = ImportMenu.DirectoryLevel.specificSolution;
                break;
            default:
                throw new Exception("ImportFolderContents case switch messed up");
        }
        LoadFolderContents(folderPath, newLevel);
    }

    public void LoadFolderContents(string folderPath, ImportMenu.DirectoryLevel newLevel)
    {
        currDirectory = folderPath;
        currLevel = newLevel;
        string[] contents = null;
        switch (currLevel)
        {
            case ImportMenu.DirectoryLevel.chapters:
                contents = Directory.GetDirectories(currDirectory).Select(s => Path.GetFileName(s)).ToArray();
                Array.Sort(contents);
                int firstEmptyChapter = 1;
                for (; firstEmptyChapter < contents.Length; ++firstEmptyChapter)
                {
                    string[] chapterContents = Directory.GetDirectories(Path.Combine(currDirectory, contents[firstEmptyChapter]));
                    Array.Sort(chapterContents);
                    if (!ChapterMenu.LevelCompleted(chapterContents[0]))
                    {
                        break;
                    }
                }
                contents = contents[..firstEmptyChapter];
                break;
            case ImportMenu.DirectoryLevel.levels:
                contents = Directory.GetDirectories(currDirectory).Where(s => ChapterMenu.LevelCompleted(s)).Select(s => Path.GetFileName(s)).ToArray();
                break;
            case ImportMenu.DirectoryLevel.solutions:
                currDirectory = Path.Combine(currDirectory, "solutions");
                contents = Directory.GetDirectories(currDirectory).Select(s => Path.GetFileName(s)).ToArray();
                if (Path.GetFullPath(currDirectory) == Path.GetFullPath(Directory.GetParent(importMenu.solution.solutionPath).FullName))
                {
                    contents = contents.Where(s => s != Path.GetFileName(importMenu.solution.solutionPath)).ToArray();
                }

                break;
            case ImportMenu.DirectoryLevel.specificSolution:
                importMenu.solutionContainer = Instantiate(solutionContainerPrefab, Vector3.zero, Quaternion.identity, importMenu.transform);
                importMenu.loadedSolution = Instantiate(dummySolutionPrefab, Vector3.zero, Quaternion.identity, importMenu.solutionContainer.transform).GetComponent<Solution>();
                importMenu.loadedSolution.CopyFixedReducers(importMenu.solution);
                importMenu.loadedSolution.CopySettings(importMenu.solution);
                importMenu.loadedSolution.mouseNode = importMenu.solution.mouseNode;
                importMenu.loadedSolution.LoadFromSerialisedForImporting(JsonUtility.FromJson<SolutionSerialise>(File.ReadAllText(Path.Combine(currDirectory, "solution.json"))));
                LoadFolderContents(importMenu.loadedSolution.contents, null);
                return;
            default:
                throw new Exception("ImportFolderContents case switch messed up");
        }

        CreateFolderButtons(contents);
        PositionButtons();
        importMenu.pathDisplay.CreateFolderButtons(currLevel, currDirectory, null);
    }

    void CreateFolderButtons(string[] contents)
    {
        if (currLevel == ImportMenu.DirectoryLevel.chapters)
        {
            var newEntry = Instantiate(folderEntryPrefab, Vector3.zero, Quaternion.identity, scrollViewContent).GetComponent<ImportFolderEntry>();
            newEntry.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            newEntry.importFolderContents = this;
            newEntry.InitialiseFavourites();
            entries.Add(newEntry.gameObject);
        }

        Array.Sort(contents);

        foreach (var inner in contents)
        {
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
        LoadFolderContents(Path.Combine(Application.persistentDataPath, "chapters"), ImportMenu.DirectoryLevel.chapters);
    }

    public bool IsFavourited(Reducer r)
    {
        return IsFavourited(r.id);
    }

    public bool IsFavourited(uint id)
    {
        string targetSolPath = Path.GetFullPath(currDirectory);

        foreach (var fv in favouritedReducers)
        {
            if (fv.reducerId == id && targetSolPath == Path.GetFullPath(fv.solutionPath))
            {
                return true;
            }
        }

        return false;
    }

    public void AddFavourite(Reducer r)
    {
        FavouritedReducer newFavRed = new FavouritedReducer(r, currDirectory, DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        favouritedReducers.Add(newFavRed);
        newFavRed.AddToSolSerialise();
    }

    public void AddFavourite(FavouritedReducer r)
    {
        FavouritedReducer newFavRed = new FavouritedReducer(r, DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        favouritedReducers.Add(newFavRed);
        newFavRed.AddToSolSerialise();
    }

    public void RemoveFavourite(Reducer r)
    {
        RemoveFavourite(r.id);
    }

    public void RemoveFavourite(uint id)
    {
        RemoveFavourite(id, currDirectory);
    }

    public void RemoveFavourite(FavouritedReducer r)
    {
        RemoveFavourite(r.reducerId, r.solutionPath);
    }

    public void RemoveFavourite(uint id, string targetPath)
    {
        targetPath = Path.GetFullPath(targetPath);
        for (int i = 0; i < favouritedReducers.Count; ++i)
        {
            if (favouritedReducers[i].reducerId == id && targetPath == Path.GetFullPath(favouritedReducers[i].solutionPath))
            {
                favouritedReducers[i].RemoveFromSolSerialise();
                favouritedReducers.RemoveAt(i);
                return;
            }
        }
    }

    public async Task ReadFavourites()
    {
        favouritedReducers = new();
        string[] chapterDirs = Directory.GetDirectories(Path.Combine(Application.persistentDataPath, "chapters"));
        foreach (string chapter in chapterDirs)
        {
            string[] levelDirs = Directory.GetDirectories(chapter);
            foreach (string level in levelDirs)
            {
                string[] solDirs = Directory.GetDirectories(Path.Combine(level, "solutions"));
                foreach (string solutionPath in solDirs)
                {
                    ReadSolutionFavourites
                    (
                        solutionPath,
                        JsonUtility.FromJson<SolutionSerialise>(await File.ReadAllTextAsync(Path.Combine(solutionPath, "solution.json")))
                    );
                }
            }
        }
    }

    void ReadSolutionFavourites(string solutionPath, SolutionSerialise solSerialise)
    {
        foreach (var fe in solSerialise.favourites)
        {
            favouritedReducers.Add(new FavouritedReducer(fe, solutionPath));
        }
    }

    public class FavouritedReducer
    {
        public string solutionPath;
        public uint reducerId;
        public string reducerName;
        public int bgc;
        public int fgc;
        public int fgs;
        public long lastAccessed;

        public FavouritedReducer(Reducer r, string solPathArg, long lastAccessedArg)
        {
            solutionPath = solPathArg;
            reducerId = r.id;
            reducerName = r.rName;
            bgc = r.backgroundColour;
            fgc = r.foregroundColour;
            fgs = r.foregroundSprite;
            lastAccessed = lastAccessedArg;
        }

        public FavouritedReducer(FavouritedReducer fr, long lastAccessedArg)
        {
            solutionPath = fr.solutionPath;
            reducerId = fr.reducerId;
            reducerName = fr.reducerName;
            bgc = fr.bgc;
            fgc = fr.fgc;
            fgs = fr.fgs;
            lastAccessed = lastAccessedArg;
        }

        public FavouritedReducer(SolutionSerialise.FavouriteEntry fe, string solPathArg)
        {
            solutionPath = solPathArg;
            reducerId = fe.reducerId;
            reducerName = fe.reducerName;
            bgc = fe.bgc;
            fgc = fe.fgc;
            fgs = fe.fgs;
            lastAccessed = fe.lastAccessed;
        }

        public void AddToSolSerialise()
        {
            string jsonFile = Path.Combine(solutionPath, "solution.json");
            SolutionSerialise solSerialise = JsonUtility.FromJson<SolutionSerialise>(File.ReadAllText(jsonFile));
            solSerialise.favourites = solSerialise.favourites.Append(new SolutionSerialise.FavouriteEntry(this, true)).ToArray();
            File.WriteAllTextAsync(jsonFile, JsonUtility.ToJson(solSerialise));
        }

        public void RemoveFromSolSerialise()
        {
            string jsonFile = Path.Combine(solutionPath, "solution.json");
            SolutionSerialise solSerialise = JsonUtility.FromJson<SolutionSerialise>(File.ReadAllText(jsonFile));
            solSerialise.favourites = solSerialise.favourites.Where(fe => fe.reducerId != reducerId).ToArray();
            File.WriteAllTextAsync(jsonFile, JsonUtility.ToJson(solSerialise));
        }
    }
}
