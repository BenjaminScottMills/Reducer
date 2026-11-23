using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.UI;

public class ReducerButton : SidebarButton, ICanvasRaycastFilter
{
    public Reducer reducer;
    public UIReducerVisual reducerVisual;
    public ReducerMenu updateMenu;
    public Image highlight;
    public ReducerButton childButton;
    public ReducerButton parentButton;
    public bool fixedReducer;

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        Vector3[] cornersArray = new Vector3[4];
        (transform as RectTransform).GetWorldCorners(cornersArray);

        return (cornersArray[2].x - cornersArray[1].x) / 2 >= Vector2.Distance(eventCamera.ScreenToWorldPoint(sp), transform.position);
    }

    public void UpdateVisuals()
    {
        reducerVisual.SetVisual(reducer);
    }

    protected override void ChildUpdate()
    {
        if (highlight != null) highlight.enabled = reducer.solution.currentReducer == reducer && mouseNode.topMenu.selectedScreen != 'T';
    }

    protected override void MouseOverlapUpdate()
    {
        mouseNode.tooltipText.text = reducer.rName;
        if (fixedReducer)
        {
            if (Input.GetMouseButtonDown(0))
            {
                mouseNode.reducer = reducer;
                mouseNode.offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    Debug.Log("To Implement for UI update"); // probably do this by passing in the "row element" thing
                    mouseNode.StartDraggingSidebarButton(reducer.isChild ? parentButton : this);
                }
                else
                {
                    if (reducer.isChild)
                    {
                        if (mouseNode.solution.currentReducer == reducer || mouseNode.solution.currentReducer.child == reducer)
                            mouseNode.reducer = mouseNode.solution.fixedLocalReducer; // Hell yeah
                    }
                    else
                    {
                        mouseNode.reducer = reducer;
                    }
                    mouseNode.offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
            }
            else if (Input.GetMouseButtonDown(1) && mouseNode.topMenu.selectedScreen != 'T')
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    if (!reducer.isChild && updateMenu != null)
                    {
                        EnableUpdateMenu();
                    }
                }
                else
                {
                    reducer.SetReducerActive(mouseNode);
                }
            }
        }
    }

    public void DisableMenu()
    {
        Destroy(updateMenu.gameObject);
        updateMenu = null;
    }

    public void EnableUpdateMenu()
    {
        updateMenu.gameObject.SetActive(true);
        updateMenu.Setup();
    }
}
