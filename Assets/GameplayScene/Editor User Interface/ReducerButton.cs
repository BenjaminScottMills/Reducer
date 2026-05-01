using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class ReducerButton : SidebarButton
{
    public Reducer reducer;
    public ReducerVisual reducerVisual;
    public ReducerMenu updateMenu;
    public SpriteRenderer highlight;
    public ReducerButton childButton;
    public ReducerButton parentButton;

    // Start is called before the first frame update
    void Start()
    {
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        reducerVisual.SetVisual(reducer);
    }

    protected override void SetInvis()
    {
        if (highlight != null) highlight.enabled = false;
        reducerVisual.gameObject.SetActive(false);
    }

    protected override void SetVis()
    {
        if (highlight != null) highlight.enabled = reducer.solution.currentReducer == reducer && mouseNode.topMenu.selectedScreen != 'T';
        reducerVisual.gameObject.SetActive(true);
    }

    protected override void TopHalfMouseOverlap()
    {
        mouseNode.tooltipText.text = reducer.rName;
        if (Input.GetMouseButtonDown(0))
        {
            mouseNode.reducer = reducer;
            mouseNode.offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    protected override void BottomHalfMouseOverlap()
    {
        mouseNode.tooltipText.text = reducer.rName;
        if (Input.GetMouseButtonDown(0))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                mouseNode.StartDraggingSidebarButton(reducer.isChild ? parentButton : this);
            }
            else
            {
                if (reducer.isChild)
                {
                    if (mouseNode.solution.currentReducer == reducer || mouseNode.solution.currentReducer.child == reducer)
                        mouseNode.reducer = mouseNode.solution.customReducerList.fixedReducerList.localReducer; // Hell yeah
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
                    updateMenu.gameObject.SetActive(true);
                    updateMenu.Setup();
                }
            }
            else
            {
                reducer.SetReducerActive(mouseNode);
            }
        }
    }

    public void DisableMenu()
    {
        Destroy(updateMenu.gameObject);
        updateMenu = null;
    }
}
