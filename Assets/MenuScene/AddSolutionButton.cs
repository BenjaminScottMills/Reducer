using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AddSolutionButton : MonoBehaviour, IPointerClickHandler
{
    public LevelMenu lm;
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        lm.AddSolution();
    }
}
