using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GenericButton : UIPointerHoverDetector, IPointerClickHandler
{
    public MethodInvoker invoker;
    public TooltipText tooltipText;
    public string textToWrite;
    public bool allowNonLeftClicks;
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (pointerEventData.button == PointerEventData.InputButton.Left || allowNonLeftClicks)
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
    public abstract class LeftRightClickInvoker
    {
        public PointerEventData pointerEventData;
        public abstract void InvokeMethod();
    }
}
