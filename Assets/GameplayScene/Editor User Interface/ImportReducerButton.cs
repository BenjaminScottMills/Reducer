using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImportReducerButton : MonoBehaviour, IPointerClickHandler
{
    public Solution solution;
    public ReducerMenu parentMenu;
    public ImportMenu importMenu;

    void Update()
    {
        if (!solution.importsUnlocked)
        {
            gameObject.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        solution.SetUninteractable();
        parentMenu.gameObject.SetActive(false);
        importMenu.gameObject.SetActive(true);
        importMenu.Initialise();
    }
}
