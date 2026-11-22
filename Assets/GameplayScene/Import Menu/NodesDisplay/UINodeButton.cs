using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UINodeButton : CircularGenericButton
{
    public Image highlight;
    public Reducer reducer;
    public UIReducerVisual reducerVisual;
    public bool enableHighlight;
    public bool manuallyControlHighlight;
    public bool useRawName;

    protected override void ChildUpdate()
    {
        textToWrite = (!useRawName && reducer.isChild) ? "Child" : reducer.rName;
        if (!manuallyControlHighlight)
        {
            if (isPointerHovered)
            {
                highlight.enabled = enableHighlight && reducer.Selectable();
            }
            else
            {
                highlight.enabled = false;
            }
        }
        else
        {
            highlight.enabled = enableHighlight;
        }
    }

    public void SetVisual()
    {
        reducerVisual.SetVisual(reducer);
    }
}
