using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImportMenuNodeButton : UIPointerHoverDetector, IPointerClickHandler
{
    public Image highlight;
    public Reducer reducer;
    public ImportMenuNodeDisplay importMenuNodeDisplay;
    public UIReducerVisual reducerVisual;
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (reducer.Selectable() && pointerEventData.button == PointerEventData.InputButton.Left)
        {
            importMenuNodeDisplay.PushReducer(reducer);
        }
    }

    void Update()
    {
        if (isPointerHovered)
        {
            highlight.enabled = reducer.Selectable();
            importMenuNodeDisplay.tooltipText.text = reducer.isChild ? "Child" : reducer.rName;
        }
        else
        {
            highlight.enabled = false;
        }
    }
}
