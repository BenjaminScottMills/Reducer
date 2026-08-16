using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImportMenuNodeButton : GenericButton
{
    public Image highlight;
    public Reducer reducer;
    public TooltipText tooltipText;
    public UIReducerVisual reducerVisual;

    void Update()
    {
        if (isPointerHovered)
        {
            highlight.enabled = reducer.Selectable();
            tooltipText.text = reducer.isChild ? "Child" : reducer.rName;
        }
        else
        {
            highlight.enabled = false;
        }
    }
}
