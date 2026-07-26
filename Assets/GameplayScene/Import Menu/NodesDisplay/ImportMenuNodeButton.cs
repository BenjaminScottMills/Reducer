using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ImportMenuNodeButton : UIPointerHoverDetector, IPointerClickHandler
{
    public Reducer reducer;
    public ImportMenuNodeDisplay importMenuNodeDisplay;
    public UIReducerVisual reducerVisual;
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (reducer.Selectable() && pointerEventData.button != PointerEventData.InputButton.Right)
        {
            importMenuNodeDisplay.PushReducer(reducer);
        }
    }

    void Update()
    {
        if (isPointerHovered)
        {
            importMenuNodeDisplay.tooltipText.text = reducer.isChild ? "Child" : reducer.rName;
        }
    }
}
