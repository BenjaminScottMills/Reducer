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
    }

    public void ActivateTestMode()
    {/*
        fixedButtons.ForEach(b => b.gameObject.SetActive(false));

        var swapper = fixedButtons;
        fixedButtons = altFixedButtons;
        altFixedButtons = swapper;

        fixedButtons.ForEach(b => b.gameObject.SetActive(true));

        float maxHeight = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height)).y - (Camera.main.orthographicSize / 10);
        float minHeight = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 2)).y + (Camera.main.orthographicSize / 5);

        if (fixedButtons[0].transform.position.y > minHeight && fixedButtons.Last().transform.position.y > maxHeight)
        {
            if (fixedButtons.Last().transform.position.y + (minHeight - fixedButtons[0].transform.position.y) > maxHeight)
            {
                transform.position += new Vector3(0, minHeight - fixedButtons[0].transform.position.y);
            }
            else
            {
                transform.position += new Vector3(0, maxHeight - fixedButtons.Last().transform.position.y);
            }

            offset = transform.localPosition - basePosition;
        }*/
    }

    public void DeactivateTestMode()
    {/*
        fixedButtons.ForEach(b => b.gameObject.SetActive(false));

        var swapper = fixedButtons;
        fixedButtons = altFixedButtons;
        altFixedButtons = swapper;

        fixedButtons.ForEach(b => b.gameObject.SetActive(true));

        float maxHeight = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height)).y - (Camera.main.orthographicSize / 10);
        float minHeight = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 2)).y + (Camera.main.orthographicSize / 5);

        if (fixedButtons[0].transform.position.y < minHeight && fixedButtons.Last().transform.position.y < maxHeight)
        {
            if (fixedButtons.Last().transform.position.y + (minHeight - fixedButtons[0].transform.position.y) < maxHeight)
            {
                transform.position += new Vector3(0, minHeight - fixedButtons[0].transform.position.y);
            }
            else
            {
                transform.position += new Vector3(0, maxHeight - fixedButtons.Last().transform.position.y);
            }

            offset = transform.localPosition - basePosition;
        }*/
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
            }
        }
    }
}
