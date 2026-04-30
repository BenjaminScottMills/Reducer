using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FolderInitialiseDeleteButton : MonoBehaviour, IPointerClickHandler
{
    public RemoveFolderManager rfm;
    public int type;
    public bool hovered = false;

    public void SetHovered()
    {
        hovered = true;
    }

    public void SetUnhovered()
    {
        hovered = false;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (type == 0)
        {
            rfm.initialClicked();
        }
        else if (type == 1)
        {
            rfm.denyClicked();
        }
        else if (type == 2)
        {
            rfm.confirmClicked();
        }
    }

    public bool Hovered()
    {
        return gameObject.activeInHierarchy && hovered;
    }
}
