using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FixedReducerList : MonoBehaviour
{
    public List<Reducer> editorScreenFixedReducers;
    public List<Reducer> testScreenFixedReducers;
    public MouseNode mouseNode;
    public RectTransform firstElementLocation;
    public RectTransform secondElementLocation;
    public RectTransform selfTransform;
    public GameObject rowEntryPrefab;
    public GameObject reducerButtonPrefab;
    List<GameObject> gameObjectListEntries;
    void Start()
    {
        gameObjectListEntries = new();
        CreateForReducers(ToTwoCol(editorScreenFixedReducers, false));
    }

    List<List<Reducer>> ToOneCol(List<Reducer> vec)
    {
        List<List<Reducer>> retval = new();
        foreach (var r in vec)
        {
            retval.Add(new List<Reducer>{r});
        }

        return retval;
    }

    List<List<Reducer>> ToTwoCol(List<Reducer> vec, bool rowFirst)
    {
        int numRows = (vec.Count + 1) / 2;
        List<List<Reducer>> retval = new();
        for (int i = 0; i < numRows; i++)
        {
            retval.Add(new List<Reducer>());
        }

        for (int i = 0; i < vec.Count; i++)
        {
            int retvalIdx;
            if (rowFirst)
            {
                retvalIdx = i / 2;
            }
            else
            {
                retvalIdx = i % numRows;
            }
            retval[retvalIdx].Add(vec[i]);
        }

        return retval;
    }

    public void ActivateTestMode()
    {
        CreateForReducers(ToOneCol(testScreenFixedReducers));
    }

    public void DeactivateTestMode()
    {
        CreateForReducers(ToTwoCol(editorScreenFixedReducers, false));
    }

    void CreateForReducers(List<List<Reducer>> reducerRows)
    {
        gameObjectListEntries.ForEach(go => Destroy(go));
        gameObjectListEntries.Clear();

        selfTransform.offsetMin = (secondElementLocation.anchoredPosition - firstElementLocation.anchoredPosition) * reducerRows.Count;
        Vector2 offset = secondElementLocation.position - firstElementLocation.position;
        Vector2 currPos = firstElementLocation.position;

        foreach (List<Reducer> row in reducerRows)
        {
            GameObject newRowEntry = Instantiate(rowEntryPrefab, transform);
            newRowEntry.transform.position = currPos;
            gameObjectListEntries.Add(newRowEntry);
            currPos += offset;
            foreach (Reducer r in row)
            {
                ReducerButton newButton = Instantiate(reducerButtonPrefab, newRowEntry.transform).GetComponent<ReducerButton>();
                newButton.reducer = r;
                newButton.mouseNode = mouseNode;
                newButton.UpdateVisuals();
            }
        }
    }
}
