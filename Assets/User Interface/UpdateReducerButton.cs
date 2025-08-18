using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpdateReducerButton : MonoBehaviour, IPointerClickHandler
{
    public InputField rName;
    public InputField description;
    public UpdateReducerMenu parentMenu;

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        parentMenu.UpdateReducer(rName.text, description.text);
        parentMenu.gameObject.SetActive(false);
    }
}
