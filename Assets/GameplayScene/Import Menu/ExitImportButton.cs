using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExitImportButton : MonoBehaviour, IPointerClickHandler
{
    public ImportMenu importMenu;

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        importMenu.CancelImport();
    }
}
