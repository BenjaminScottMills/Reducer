using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Solution : MonoBehaviour
{
    public string sName;
    public string solutionPath;
    public uint idCounter;
    public MouseNode mouseNode;
    public Reducer currentReducer; // must be present in reducers
    public RFolder currentFolder;
    public List<ReducerOrFolder> contents;
    public ReducerEnumerable reducers;
    public GameObject reducerPrefab;
    public CustomReducerList customReducerList;
    public bool localReducersUnlocked = true;

    // fixed reducers
    public Reducer fixedLocalReducer;
    public Reducer nullReducer;
    public Reducer fireReducer;
    public Reducer earthReducer;
    public Reducer plantReducer;
    public Reducer waterReducer;
    public Reducer combineReducer;
    public Reducer blackInputReducer;
    public Reducer whiteInputReducer;
    public Reducer localOuterBlackReducer;
    public Reducer localOuterWhiteReducer;
    public Reducer outputNodeReducer;

    void Start()
    {
        solutionPath = PlayerPrefs.GetString("solution path");
        reducers = new ReducerEnumerable(this);
        string solFile = Path.Combine(solutionPath, "solution.json");
        if (File.Exists(solFile))
        {
            LoadFromSerialised(JsonUtility.FromJson<SolutionSerialise>(File.ReadAllText(solFile)));
            customReducerList.customButtons[0].DisableMenu();
            contents[0].r.SetReducerActive(mouseNode);
        }
        else
        {
            CreateMainReducer();
        }
    }

    void CreateMainReducer()
    {
        AddReducer("Main", "", 1, 0, 12);
        customReducerList.customButtons[0].DisableMenu();
    }

    public void InitialiseSingleReducerOrFolder(ReducerOrFolder rof, RFolder parentFolder)
    {
        if (rof.rs != null)
        {
            var newRed = Instantiate(reducerPrefab, Vector3.zero, Quaternion.identity, transform.parent).GetComponent<Reducer>();
            newRed.id = rof.rs.Value.id;
            newRed.nullReducer = nullReducer;
            newRed.solution = this;
            newRed.folder = parentFolder;
            newRed.SetReducerSerialise(rof.rs);
            rof.r = newRed;
            rof.rs = null;
        }
        else
        {
            rof.f = new RFolder(rof.fs.Value, this, parentFolder);
            rof.fs = null;
        }
    }

    public void LoadFromSerialised(SolutionSerialise s)
    {
        sName = s.name;
        idCounter = s.idCounter;
        contents = new List<ReducerOrFolder>();
        foreach (var rofs in s.contents)
        {
            var newRedOrFol = new ReducerOrFolder(rofs);
            InitialiseSingleReducerOrFolder(newRedOrFol, null);
            contents.Add(newRedOrFol);
        }

        foreach (Reducer r in reducers)
        {
            r.LoadFromSerialised(reducers);
        }

        // foreach (var rof in contents)
        // {
        //     customReducerList.AddReducerButton(rof, false);
        // }
    }

    public Reducer.ExecuteReducer Execute(Reducer black, Reducer white)
    {
        return contents[0].r.Execute(black, white);
    }

    public void AddReducer(string name, string desc, ReducerVisual reducerVisual)
    {
        AddReducer(name, desc, reducerVisual.foregroundColour, reducerVisual.backgroundColour, reducerVisual.foregroundSprite);
    }

    public void AddReducer(string name, string desc, int fgc, int bgc, int fgs)
    {
        Reducer newReducer = Instantiate(reducerPrefab, Vector3.zero, Quaternion.identity, transform.parent).GetComponent<Reducer>();

        newReducer.rName = name;
        newReducer.description = desc;
        newReducer.id = idCounter;
        newReducer.nullReducer = nullReducer;
        newReducer.solution = this;
        newReducer.foregroundColour = fgc;
        newReducer.backgroundColour = bgc;
        newReducer.foregroundSprite = fgs;
        newReducer.folder = currentFolder;
        idCounter++;
        
        if (currentFolder != null)
        {
            currentFolder.contents.Add(new ReducerOrFolder(newReducer));
        }
        else
        {
            contents.Add(new ReducerOrFolder(newReducer));
        }

        var localReducer = Instantiate(reducerPrefab, Vector3.zero, Quaternion.identity, newReducer.transform.parent).GetComponent<Reducer>();
        newReducer.child = localReducer;
        localReducer.ChildInit(newReducer);

        // customReducerList.AddReducerButton(newReducer);
    }

    public void SaveQuit()
    {
        Save();
        ReturnToMenus();
    }

    public void Save()
    {
        File.WriteAllTextAsync(
            Path.Combine(solutionPath, "solution.json"),
            JsonUtility.ToJson(new SolutionSerialise(this)));
    }

    public void ReturnToMenus()
    {
        SceneManager.LoadSceneAsync("MenuScene");
    }

    public class ReducerEnumerable : IEnumerable
    {
        Solution s;

        public ReducerEnumerable(Solution sol)
        {
            s = sol;
        }

        public IEnumerator GetEnumerator()
        {
            return new ReducerEnumerator(s);
        }

        public Reducer First(Func<Reducer, bool> predicate)
        {
            foreach (Reducer r in this)
            {
                if (predicate(r))
                {
                    return r;
                }
            }

            return null;
        }

        public void Remove(Reducer r)
        {
            if (r.folder != null)
            {
                r.folder.contents.RemoveAll(rf => rf.r == r);
            }
            else
            {
                s.contents.RemoveAll(rf => rf.r == r);
            }
        }

        public class ReducerEnumerator : IEnumerator
        {
            Solution solution;
            RFolder currFolder;
            Stack<int> indexes;

            public object Current
            {
                get
                {
                    if (currFolder != null)
                    {
                        return currFolder.contents[indexes.Peek()].r;
                    }
                    return solution.contents[indexes.Peek()].r;
                }
            }


            public ReducerEnumerator(Solution s)
            {
                solution = s;
                currFolder = null;
                indexes = new Stack<int>();
                indexes.Push(-1);
            }

            public bool MoveNext() // sexy ass recursion
            {
                indexes.Push(indexes.Pop() + 1);
                if (currFolder == null)
                {
                    if (indexes.Peek() >= solution.contents.Count)
                    {
                        return false;
                    }
                    
                    if (!solution.contents[indexes.Peek()].IsReducer())
                    {
                        currFolder = solution.contents[indexes.Peek()].f;
                        indexes.Push(-1);
                        return MoveNext();
                    }
                }
                else
                {
                    if (indexes.Peek() >= currFolder.contents.Count)
                    {
                        indexes.Pop();
                        currFolder = currFolder.parentFolder;
                        return MoveNext();
                    }
                    
                    if (!currFolder.contents[indexes.Peek()].IsReducer())
                    {
                        currFolder = currFolder.contents[indexes.Peek()].f;
                        indexes.Push(-1);
                        return MoveNext();
                    }
                }

                return true;
            }

            public void Reset()
            {
                currFolder = null;
                indexes = new Stack<int>();
                indexes.Push(-1);
            }
        }
    }
}
