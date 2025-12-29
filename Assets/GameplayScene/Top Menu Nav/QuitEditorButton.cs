using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuitEditorButton : MonoBehaviour, IPointerClickHandler
{
    public Solution solution;
    // Start is called before the first frame update
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        solution.SaveQuit();
    }
}
