using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuitEditorButton : TooltipButton, IPointerClickHandler
{
    public Solution solution;
    public RequirementsTab requirementsTab;

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        requirementsTab.WriteCustomTestCases();
        solution.SaveQuit();
    }
}
