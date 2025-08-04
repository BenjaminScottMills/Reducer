using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreateReducerButton : MonoBehaviour, IPointerClickHandler
{
    public Solution currentSolution;
    public InputField rName;
    public InputField description;
    public AddReducerMenu parentMenu;
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        currentSolution.AddReducer(rName.text, description.text);
        parentMenu.gameObject.SetActive(false);
    }
}
