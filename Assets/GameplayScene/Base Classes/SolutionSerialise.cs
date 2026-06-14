using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public struct SolutionSerialise
{
    public SolutionSerialise(Solution solution, ImportFolderContents ifc, string solutionPath)
    {
        name = solution.sName;
        idCounter = solution.idCounter;
        Dictionary<uint, FavouriteEntry> favouritesMap = ifc.favouritedReducers
                                            .Where(fr => Path.GetFullPath(fr.solutionPath) == Path.GetFullPath(solutionPath))
                                            .Select(fr => new FavouriteEntry(fr, false))
                                            .ToDictionary(fr => fr.reducerId);
        foreach (Reducer r in solution.reducers)
        {
            if (favouritesMap.ContainsKey(r.id))
            {
                favouritesMap[r.id].UpdateVals(r);
            }
        }
        favourites = favouritesMap.Select(kvp => kvp.Value).Where(fe => fe.validated).ToArray();

        contents = solution.contents.Select(r => new ReducerOrFolderSerialise(r)).ToArray();
    }

    public string name;
    public uint idCounter;
    public ReducerOrFolderSerialise[] contents;
    public FavouriteEntry[] favourites;

    [System.Serializable]
    public struct FavouriteEntry
    {
        public FavouriteEntry(ImportFolderContents.FavouritedReducer fr, bool validatedArg)
        {
            lastAccessed = fr.lastAccessed;
            reducerId = fr.reducerId;
            reducerName = fr.reducerName;
            bgc = fr.bgc;
            fgc = fr.fgc;
            fgs = fr.fgs;
            validated = validatedArg;
        }

        public void UpdateVals(Reducer r)
        {
            reducerName = r.rName;
            bgc = r.backgroundColour;
            fgc = r.foregroundColour;
            fgs = r.foregroundSprite;
            validated = true;
        }

        public long lastAccessed;
        public uint reducerId;
        public string reducerName;
        public int bgc;
        public int fgc;
        public int fgs;
        public bool validated;
    }
    
    [System.Serializable]
    public struct ReducerOrFolderSerialise
    {
        public ReducerOrFolderSerialise(ReducerOrFolder rf)
        {
            isReducer = rf.IsReducer();
            if (isReducer)
            {
                myJson = JsonUtility.ToJson(new ReducerSerialise(rf.r));
            }
            else
            {
                myJson = JsonUtility.ToJson(new FolderSerialise(rf.f));
            }
        }

        public bool isReducer;
        public string myJson;
    }

    [System.Serializable]
    public struct FolderSerialise
    {
        public FolderSerialise(RFolder f)
        {
            folderName = f.folderName;
            contents = f.contents.Select(rf => new ReducerOrFolderSerialise(rf)).ToArray();
        }
        public string folderName;
        public ReducerOrFolderSerialise[] contents;
    }

    [System.Serializable]
    public struct ReducerSerialise
    {
        public ReducerSerialise(Reducer r)
        {
            name = r.rName;
            description = r.description;
            id = r.id;
            nodeIdCounter = r.nodeIdCounter;
            backgroundColour = r.backgroundColour;
            foregroundColour = r.foregroundColour;
            foregroundSprite = r.foregroundSprite;
            nodes = r.nodes.Select(n => new NodeSerialise(n)).ToArray();
            if (r.child == null)
            {
                childNodeIdCounter = 100;
                childNodes = new NodeSerialise[] { };
            }
            else
            {
                childNodeIdCounter = r.child.nodeIdCounter;
                childNodes = r.child.nodes.Select(n => new NodeSerialise(n)).ToArray();
            }
        }

        public string name;
        public string description;
        public uint id;
        public uint nodeIdCounter;
        public int foregroundColour;
        public int backgroundColour;
        public int foregroundSprite;
        public NodeSerialise[] nodes;
        public uint childNodeIdCounter;
        public NodeSerialise[] childNodes;
    }

    [System.Serializable]
    public struct NodeSerialise
    {
        public NodeSerialise(Node n)
        {
            redId = n.reducer.id;
            id = n.id;
            yPos = n.transform.position.y;
            xPos = n.transform.position.x;
            nextId = n.next?.id ?? 0;
            blackLink = n.blackLink;
        }

        public uint redId;
        public uint id;
        public float yPos;
        public float xPos;
        public uint nextId;
        public bool blackLink;
    }
}
