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

    public void LoadFromSerialised(SolutionSerialise s)
    {
        sName = s.name;
        idCounter = s.idCounter;
        reducers = new List<Reducer>();
        foreach (var rs in s.reducers)
        {
            var newRed = Instantiate(reducerPrefab).GetComponent<Reducer>();
            newRed.id = rs.id;
            reducers.Add(newRed);
        }

        for (int i = 0; i < s.reducers.Length; i++)
        {
            reducers[i].LoadFromSerialised(s.reducers[i], reducers);
        }
    }

    public Reducer ExecuteFast(Reducer black, Reducer white)
    {
        return reducers[0].ExecuteFast(black, white);
    }

    public void AddReducer(string name, string desc)
    {
        Reducer newReducer = Instantiate(reducerPrefab).GetComponent<Reducer>();

        newReducer.rName = name;
        newReducer.description = desc;
        newReducer.id = idCounter;
        newReducer.nullReducer = nullReducer;
        newReducer.solution = this;
        idCounter++;
        reducers.Add(newReducer);
        customReducerList.AddReducerButton(newReducer);

        // add button to custom reducer list.
    }
}
