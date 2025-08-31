using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TopMenuButton : MonoBehaviour, IPointerClickHandler
{
    public TopMenu tm;
    public char myScreen;
    // Start is called before the first frame update
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        tm.UpdateSelectedScreen(myScreen);
    }
}
