using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InitialiseDeleteButton : MonoBehaviour, IPointerClickHandler
{
    public RemoveSolutionManager rsm;
    public int type;
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (type == 0)
        {
            rsm.initialClicked();
        }
        else if (type == 1)
        {
            rsm.denyClicked();
        }
        else if (type == 2)
        {
            rsm.confirmClicked();
        }
    }
}
