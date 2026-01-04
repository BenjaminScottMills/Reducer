using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReducerPlacementSlot : MonoBehaviour
{
    public Reducer reducer;
    public GameObject placeholder;
    public ReducerVisual reducerVisual;
    public CircleCollider2D boxCollider;
    public TooltipText tooltipText;
    public String emptyTooltip;

    void Update()
    {
        if (boxCollider.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
        {
            if (reducer == null)
            {
                tooltipText.text = emptyTooltip;
            }
            else
            {
                tooltipText.text = reducer.rName;
            }

            if (Input.GetMouseButtonDown(1))
            {
                reducer = null;
                reducerVisual.gameObject.SetActive(false);
                placeholder.SetActive(true);
            }
        }
    }

    public void CheckPlaced(Reducer r)
    {
        if (boxCollider.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
        {
            reducer = r;
            reducerVisual.SetVisual(r);
            reducerVisual.gameObject.SetActive(true);
            placeholder.SetActive(false);
        }
    }
}
