using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPointerHoverDetector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected bool isPointerHovered;
    
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        isPointerHovered = true;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        isPointerHovered = false;
    }

    public bool GetIsPointerHovered()
    {
        return isPointerHovered;
    }
}
