using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TopMenuButton : TooltipButton, IPointerClickHandler
{
    public TopMenu tm;
    public char myScreen;

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        tm.UpdateSelectedScreen(myScreen);
    }
}
