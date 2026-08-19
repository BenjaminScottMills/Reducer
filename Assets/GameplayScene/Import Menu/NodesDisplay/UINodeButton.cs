using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UINodeButton : GenericButton, ICanvasRaycastFilter
{
    public Image highlight;
    public Reducer reducer;
    public TooltipText tooltipText;
    public UIReducerVisual reducerVisual;
    public RectTransform rectTransform;
    public bool enableHighlight;
    public bool useRawName;

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        Vector3[] cornersArray = new Vector3[4];
        rectTransform.GetWorldCorners(cornersArray);

        return (cornersArray[2].x - cornersArray[1].x) / 2 >= Vector2.Distance(eventCamera.ScreenToWorldPoint(sp), transform.position);
    }

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
