using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class ReducerButton : MonoBehaviour
{
    public Reducer reducer;
    public SpriteRenderer spriteRenderer;
    public Collider2D colliderd2d;
    public bool upperHalf;
    public MouseNode mouseNode;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.sprite = reducer.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (upperHalf)
        {
            if (transform.position.y > Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 2)).y)
            {
                spriteRenderer.enabled = true;
                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (Input.GetMouseButtonDown(0) && colliderd2d.OverlapPoint(mousePos))
                {
                    mouseNode.reducer = reducer;
                    mouseNode.offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
            }
            else
            {
                spriteRenderer.enabled = false;
            }
        }
        else
        {
            if (transform.position.y < Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 2)).y)
            {
                spriteRenderer.enabled = true;
                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (Input.GetMouseButtonDown(0) && colliderd2d.OverlapPoint(mousePos))
                {
                    mouseNode.reducer = reducer;
                    mouseNode.offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
            }
            else
            {
                spriteRenderer.enabled = false;
            }
        }
    }
}
