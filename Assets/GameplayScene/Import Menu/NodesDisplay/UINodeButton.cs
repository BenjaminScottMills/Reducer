using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UINodeButton : CircularGenericButton
{
    public Image highlight;
    public Reducer reducer;
    public TooltipText tooltipText;
    public UIReducerVisual reducerVisual;
    public bool enableHighlight;
    public bool useRawName;

    void Update()
    {
        if (isPointerHovered)
        {
            highlight.enabled = enableHighlight && reducer.Selectable();
            tooltipText.text = (!useRawName && reducer.isChild) ? "Child" : reducer.rName;
        }
        else
        {
            highlight.enabled = false;
        }
    }
}
