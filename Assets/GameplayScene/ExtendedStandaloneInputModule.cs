using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExtendedStandaloneInputModule : StandaloneInputModule
{
    public void ReTriggerTouchPress(PointerEventData pointerEventData)
    {
        ProcessTouchPress(GetTouchPointerEventData(new Touch(){position = pointerEventData.position}, out _, out _), true, true);
    }
}
