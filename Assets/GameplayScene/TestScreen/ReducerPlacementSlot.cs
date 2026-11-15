using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReducerPlacementSlot : CircularGenericButton
{
    public Reducer reducer;
    public GameObject placeholder;
    public UIReducerVisual reducerVisual;
    public string emptyTooltip;

    protected override void ChildUpdate()
    {
        if (reducer == null)
        {
            textToWrite = emptyTooltip;
        }
        else
        {
            textToWrite = reducer.rName;
        }

        if (Input.GetMouseButtonDown(1))
        {
            reducer = null;
            reducerVisual.gameObject.SetActive(false);
            placeholder.SetActive(true);
        }
    }

    public void SetReducer(Reducer r)
    {
        if (r == null)
        {
            reducer = null;
            reducerVisual.gameObject.SetActive(false);
            placeholder.SetActive(true);
        }
        else
        {
            reducer = r;
            reducerVisual.SetVisual(r);
            reducerVisual.gameObject.SetActive(true);
            placeholder.SetActive(false);
        }
    }

    public void CheckPlaced(Reducer r)
    {
        if (isPointerHovered)
        {
            SetReducer(r);
        }
    }
}
