using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GenericButton : UIPointerHoverDetector, IPointerClickHandler
{
    public MethodInvoker invoker;
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (pointerEventData.button == PointerEventData.InputButton.Left) invoker.InvokeMethod();
    }

    public abstract class MethodInvoker
    {
        public abstract void InvokeMethod();
    }
}
