using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightSidebarSeperator : MonoBehaviour
{
    public MouseNode mouseNode;
    public BoxCollider2D boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (boxCollider.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition))) mouseNode.mouseOverUI = true;
    }
}
