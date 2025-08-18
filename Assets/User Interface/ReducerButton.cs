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
    public UpdateReducerMenu updateMenu;

    // Start is called before the first frame update
    void Start()
    {
        reducerVisual.SetVisual(reducer.backgroundColour, reducer.foregroundColour, reducer.foregroundSprite);
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
                reducerVisual.gameObject.SetActive(true);
                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (colliderd2d.OverlapPoint(mousePos))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        mouseNode.reducer = reducer;
                        mouseNode.offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    }
                    else if (Input.GetMouseButtonDown(1) && !upperHalf)
                    {
                        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                        {
                            updateMenu.gameObject.SetActive(true);
                            updateMenu.Setup();
                        }
                        else
                        {
                            foreach (var node in mouseNode.selectedNodes)
                            {
                                node.SetHighlighted(false);
                            }
                            mouseNode.selectedNodes.Clear();

                            foreach (var node in reducer.solution.currentReducer.nodes)
                            {
                                node.nextConnector?.gameObject.SetActive(false);
                                node.gameObject.SetActive(false);
                            }

                            reducer.solution.currentReducer = reducer;
                            foreach (var node in reducer.nodes)
                            {
                                node.gameObject.SetActive(true);
                                node.nextConnector?.gameObject.SetActive(true);
                            }
                        }
                    }
                }
            }
            else
            {
                reducerVisual.gameObject.SetActive(false);
            }
        }
    }
}
