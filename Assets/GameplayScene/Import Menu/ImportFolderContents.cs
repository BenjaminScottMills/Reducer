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
    public ImportMenu importMenu;
    public GameObject dummySolutionPrefab;
    public GameObject solutionContainerPrefab;
    public string currDirectory;
    public List<FavouritedReducer> favouritedReducers;
    List<GameObject> entries;
    float baseScrollViewHeight;

    // Start is called before the first frame update
    void Awake()
    {
        baseScrollViewHeight = scrollViewContent.sizeDelta.y;
        entries = new();
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
    }

    public void LoadFolderContents(List<ReducerOrFolder> contents)
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
    }

    public void LoadFolderContents(string folderPath)
    {
        currDirectory = folderPath;
        string[] contents = null;
        switch (currLevel)
        {
            case ImportMenu.DirectoryLevel.chapters:
                currLevel = ImportMenu.DirectoryLevel.levels;
                contents = Directory.GetDirectories(currDirectory).Where(s => ChapterMenu.LevelCompleted(s)).Select((s) => Path.GetFileName(s)).ToArray();
                break;
            case ImportMenu.DirectoryLevel.levels:
                currDirectory = Path.Combine(currDirectory, "solutions");
                currLevel = ImportMenu.DirectoryLevel.solutions;
                contents = Directory.GetDirectories(currDirectory).Select((s) => Path.GetFileName(s)).ToArray();
                if (Path.GetFullPath(currDirectory) == Path.GetFullPath(Directory.GetParent(importMenu.solution.solutionPath).FullName))
                {
                    contents = contents.Where(s => s != Path.GetFileName(importMenu.solution.solutionPath)).ToArray();
                }

                break;
            case ImportMenu.DirectoryLevel.solutions:
                importMenu.solutionContainer = Instantiate(solutionContainerPrefab, Vector3.zero, Quaternion.identity, importMenu.transform);
                importMenu.loadedSolution = Instantiate(dummySolutionPrefab, Vector3.zero, Quaternion.identity, importMenu.solutionContainer.transform).GetComponent<Solution>();
                importMenu.loadedSolution.CopyFixedReducers(importMenu.solution);
                importMenu.loadedSolution.CopySettings(importMenu.solution);
                importMenu.loadedSolution.mouseNode = importMenu.solution.mouseNode;
                importMenu.loadedSolution.LoadFromSerialisedForImporting(JsonUtility.FromJson<SolutionSerialise>(File.ReadAllText(Path.Combine(currDirectory, "solution.json"))));
                LoadFolderContents(importMenu.loadedSolution.contents);
                PositionButtons();
                return;
        }

        CreateFolderButtons(contents);
        PositionButtons();
    }

    void CreateFolderButtons(string[] contents)
    {
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
        currDirectory = Path.Combine(Application.persistentDataPath, "chapters");
        currLevel = ImportMenu.DirectoryLevel.chapters;
        string[] contents = Directory.GetDirectories(currDirectory).Select((s) => Path.GetFileName(s)).ToArray();
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
        CreateFolderButtons(contents);
        PositionButtons();
    }

    public bool IsFavourited(Reducer r)
    {
        return IsFavourited(r.id);
    }

    public bool IsFavourited(uint id)
    {
        Debug.Log("Potential Error stemming from this: solution nums change if we delete something earlier. Thus, maybe increase it to 5 digits and make it not go down?");
        (string targetLevelPath, int targetSolutionNum) = SolutionPathToFavouritedReducerInfo(currDirectory);

        foreach (var fv in favouritedReducers)
        {
            if (fv.reducerId == id && fv.solutionNum == targetSolutionNum && targetLevelPath == Path.GetFullPath(fv.levelPath))
            {
                return true;
            }
        }

        return false;
    }

    public void AddFavourite(Reducer r)
    {
        favouritedReducers.Add(new FavouritedReducer(r, currDirectory));
        WriteFavourites();
    }

    public void AddFavourite(FavouritedReducer r)
    {
        favouritedReducers.Add(r);
        WriteFavourites();
    }

    public void RemoveFavourite(Reducer r)
    {
        RemoveFavourite(r.id);
    }

    public void RemoveFavourite(uint id)
    {
        (string targetLevelPath, int targetSolutionNum) = SolutionPathToFavouritedReducerInfo(currDirectory);

        for (int i = 0; i < favouritedReducers.Count; ++i)
        {
            if (favouritedReducers[i].reducerId == id && favouritedReducers[i].solutionNum == targetSolutionNum && targetLevelPath == Path.GetFullPath(favouritedReducers[i].levelPath))
            {
                favouritedReducers.RemoveAt(i);
                WriteFavourites();
                return;
            }
        }
    }

    public void RemoveFavourite(FavouritedReducer r)
    {
        for (int i = 0; i < favouritedReducers.Count; ++i)
        {
            if (favouritedReducers[i].reducerId == r.reducerId && favouritedReducers[i].solutionNum == r.solutionNum && Path.GetFullPath(r.levelPath) == Path.GetFullPath(favouritedReducers[i].levelPath))
            {
                favouritedReducers.RemoveAt(i);
                WriteFavourites();
                return;
            }
        }
    }

    void WriteFavourites()
    {
        File.WriteAllTextAsync(
            Path.Combine(Application.persistentDataPath, "chapters", "00favourites", "favourites.json"),
            JsonUtility.ToJson(new FavouritesSerialise(favouritedReducers)));
    }

    void ReadFavourites()
    {
        favouritedReducers = JsonUtility.FromJson<FavouritesSerialise>(Path.Combine(Application.persistentDataPath, "chapters", "00favourites", "favourites.json")).reducers.ToList();
    }

    public static (string levelPath, int solutionNum) SolutionPathToFavouritedReducerInfo(string path)
    {
        string levelPath = Path.GetFullPath(Directory.GetParent(path).FullName);
        int solutionNum = int.Parse(Path.GetDirectoryName(path)[..2]);
        return (levelPath, solutionNum);
    }

    [Serializable]
    public struct FavouritedReducer
    {
        public string levelPath;
        public int solutionNum;
        public uint reducerId;
        public string reducerName;
        public int bgc;
        public int fgc;
        public int fgs;

        public FavouritedReducer(Reducer r, string path)
        {
            (levelPath, solutionNum) = SolutionPathToFavouritedReducerInfo(path);
            reducerId = r.id;
            reducerName = r.rName;
            bgc = r.backgroundColour;
            fgc = r.foregroundColour;
            fgs = r.foregroundSprite;
        }
    }

    [Serializable]
    public struct FavouritesSerialise
    {
        public FavouritedReducer[] reducers;

        public FavouritesSerialise(List<FavouritedReducer> faves)
        {
            reducers = faves.ToArray();
        }
    }
}
