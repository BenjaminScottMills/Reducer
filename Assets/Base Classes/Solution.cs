using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Solution : MonoBehaviour
{
    public string sName;
    public uint idCounter;
    public Reducer currentReducer; // must be present in reducers
    public List<Reducer> reducers;
    public Reducer nullReducer;
    public GameObject reducerPrefab;
    public CustomReducerList customReducerList;
    public bool localReducersUnlocked = true;

    void Start()
    {
        CreateMainReducer();
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
            reducers.Add(newRed);
        }

        for (int i = 0; i < s.reducers.Length; i++)
        {
            reducers[i].LoadFromSerialised(s.reducers[i], reducers);
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
        localReducer.isChild = true;
        localReducer.rName = name + " - child";
        localReducer.description = "";
        localReducer.id = (int)Reducer.SpecialReducers.local;
        localReducer.nullReducer = nullReducer;
        localReducer.solution = this;
        localReducer.foregroundColour = 1;
        localReducer.backgroundColour = 0;
        localReducer.foregroundSprite = 9;

        customReducerList.AddReducerButton(newReducer);
    }
}
