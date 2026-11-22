using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FixedReducerList : MonoBehaviour
{
    public List<ReducerButton> fixedButtons;
    public List<ReducerButton> altFixedButtons;
    public MouseNode mouseNode;
    public Reducer localReducer;
    public Transform firstElementLocation;
    public Transform secondElementLocation;
    public RectTransform selfTransform;
    public List<GameObject> testList;
    void Start()
    {
        PositionElements(testList);
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

    void PositionElements(List<GameObject> elements)
    {
        selfTransform.offsetMin = new Vector2(0, -100 * elements.Count);
        Vector2 offset = secondElementLocation.position - firstElementLocation.position;
        Vector2 currPos = firstElementLocation.position;

        foreach (GameObject el in elements)
        {
            el.transform.position = currPos;
            currPos += offset;
        }
    }
}
