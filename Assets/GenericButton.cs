using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GenericButton : UIPointerHoverDetector, IPointerClickHandler
{
    public MethodInvoker invoker;
    public TooltipText tooltipText;
    public string textToWrite;
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            invoker.pointerEventData = pointerEventData;
            invoker?.InvokeMethod();
        }
    }

    void Update()
    {
        ChildUpdate();

        if (isPointerHovered && tooltipText)
        {
            tooltipText.text = textToWrite;
        }
    }

    protected virtual void ChildUpdate() {}

    public abstract class MethodInvoker
    {
        public PointerEventData pointerEventData;
        public abstract void InvokeMethod();
    }
}
