using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOverUISetter : UIPointerHoverDetector
{
    public MouseNode mouseNode;
    // Update is called once per frame
    void Update()
    {
        if (isPointerHovered)
        {
            mouseNode.mouseOverUI = true;
        }
    }
}
