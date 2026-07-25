using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GenericButton : MonoBehaviour, IPointerClickHandler
{
    public MethodInvoker invoker;
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        invoker.InvokeMethod();
    }

    public abstract class MethodInvoker
    {
        public abstract void InvokeMethod();
    }
}
