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
    public List<Reducer> reducers;
    public GameObject reducerPrefab;
    public CustomReducerList customReducerList;
    public bool localReducersUnlocked = true;

    // fixed reducers
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
        string solFile = Path.Combine(solutionPath, "solution.json");
        if (File.Exists(solFile))
        {
            LoadFromSerialised(JsonUtility.FromJson<SolutionSerialise>(File.ReadAllText(solFile)));
            customReducerList.customButtons[0].DisableMenu();
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

    public void LoadFromSerialised(SolutionSerialise s)
    {
        sName = s.name;
        idCounter = s.idCounter;
        reducers = new List<Reducer>();
        foreach (var rs in s.reducers)
        {
            var newRed = Instantiate(reducerPrefab, Vector3.zero, Quaternion.identity, transform.parent).GetComponent<Reducer>();
            newRed.id = rs.id;
            newRed.nullReducer = nullReducer;
            newRed.solution = this;
            reducers.Add(newRed);
        }

        for (int i = 0; i < s.reducers.Length; i++)
        {
            reducers[i].LoadFromSerialised(s.reducers[i], reducers);
            customReducerList.AddReducerButton(reducers[i]);
        }
    }

    public Reducer.ExecuteReducer Execute(Reducer black, Reducer white)
    {
        return reducers[0].Execute(black, white);
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
        idCounter++;
        reducers.Add(newReducer);

        var localReducer = Instantiate(reducerPrefab, Vector3.zero, Quaternion.identity, newReducer.transform.parent).GetComponent<Reducer>();
        newReducer.child = localReducer;
        localReducer.ChildInit(newReducer);

        customReducerList.AddReducerButton(newReducer);
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
}
