using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PathFolderButtonClickHandler : MonoBehaviour, IPointerClickHandler
{
    public PathFolderButton pfb;

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        pfb.OnClick();
    }
}
