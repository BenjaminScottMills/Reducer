using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class ReducerButton : MonoBehaviour
{
    public Reducer reducer;
    public ReducerVisual reducerVisual;
    public Collider2D colliderd2d;
    public bool upperHalf;
    public MouseNode mouseNode;
    public ReducerMenu updateMenu;
    public SpriteRenderer highlight;
    public ReducerButton childButton;

    // Start is called before the first frame update
    void Start()
    {
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        reducerVisual.SetVisual(reducer);
    }

    // Update is called once per frame
    void Update()
    {
        if (upperHalf)
        {
            if (transform.position.y > Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 2)).y)
            {
                reducerVisual.gameObject.SetActive(true);
                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (Input.GetMouseButtonDown(0) && colliderd2d.OverlapPoint(mousePos))
                {
                    mouseNode.reducer = reducer;
                    mouseNode.offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
            }
            else
            {
                reducerVisual.gameObject.SetActive(false);
            }
        }
        else
        {
            if (transform.position.y < Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 2)).y)
            {
                highlight.enabled = reducer.solution.currentReducer == reducer && mouseNode.topMenu.selectedScreen != 'T';
                reducerVisual.gameObject.SetActive(true);
                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (colliderd2d.OverlapPoint(mousePos))
                {
                    if (Input.GetMouseButtonDown(0) && !reducer.isChild)
                    {
                        mouseNode.reducer = reducer;
                        mouseNode.offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    }
                    else if (Input.GetMouseButtonDown(1) && mouseNode.topMenu.selectedScreen != 'T')
                    {
                        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                        {
                            if (!reducer.isChild)
                            {
                                updateMenu.gameObject.SetActive(true);
                                updateMenu.Setup();
                            }
                        }
                        else
                        {
                            SetReducerActive();
                        }
                    }
                }
            }
            else
            {
                highlight.enabled = false;
                reducerVisual.gameObject.SetActive(false);
            }
        }
    }

    public void SetReducerActive()
    {
        reducer.SetReducerActive(mouseNode);
    }
}
