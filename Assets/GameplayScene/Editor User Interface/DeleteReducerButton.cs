using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeleteReducerButton : MonoBehaviour, IPointerClickHandler
{
    public ReducerMenu parentMenu;

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        parentMenu.DeleteReducer();
    }
}
