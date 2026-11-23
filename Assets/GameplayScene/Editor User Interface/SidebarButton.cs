using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SidebarButton : UIPointerHoverDetector
{
    public bool fixedAtTop;
    public MouseNode mouseNode;
    public bool beingDragged;

    void Update()
    {
        ChildUpdate();
        if (isPointerHovered && !mouseNode.onImportScreen)
        {
            MouseOverlapUpdate();
        }
    }
    protected abstract void ChildUpdate();
    protected abstract void MouseOverlapUpdate();
}
