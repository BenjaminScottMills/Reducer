using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuitEditorButton : TooltipButton, IPointerClickHandler
{
    public Solution solution;

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        solution.SaveQuit();
    }
}
