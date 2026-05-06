using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SidebarButton : MonoBehaviour
{
    public Collider2D colliderd2d;
    public bool upperHalf;
    public bool fixedAtTop;
    public MouseNode mouseNode;
    public bool beingDragged;

    void Update()
    {
        if (upperHalf)
        {
            if (transform.position.y > Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 2)).y)
            {
                SetVis();
                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (colliderd2d.OverlapPoint(mousePos) && !mouseNode.onImportScreen)
                {
                    TopHalfMouseOverlap();
                }
            }
            else
            {
                SetInvis();
            }
        }
        else
        {
            if (transform.position.y < Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 2)).y)
            {
                SetVis();
                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (colliderd2d.OverlapPoint(mousePos) && !mouseNode.onImportScreen)
                {
                    BottomHalfMouseOverlap();
                }
            }
            else
            {
                SetInvis();
            }
        }
    }

    protected abstract void SetInvis();
    protected abstract void SetVis();
    protected abstract void TopHalfMouseOverlap();
    protected abstract void BottomHalfMouseOverlap();
}
