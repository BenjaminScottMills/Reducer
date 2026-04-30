using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FolderButton : SidebarButton
{
    public SpriteRenderer highlight;
    public Collider2D textboxCollider;
    public RemoveFolderManager rfm;
    public GameObject visual;
    public Solution solution;
    public RFolder rFolder;
    public Canvas canvas;

    void Start()
    {
        upperHalf = false;
    }

    protected override void SetInvis()
    {
        visual.gameObject.SetActive(false);
    }

    protected override void SetVis()
    {
        visual.gameObject.SetActive(true);
        highlight.enabled = false;
    }

    protected override void TopHalfMouseOverlap()
    {
    }

    protected override void BottomHalfMouseOverlap()
    {
        highlight.enabled = true;
        if (rfm.AnyHovered() || textboxCollider.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition))) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                // DRAG
            }
            else
            {
                Debug.Log("Enter the folder");
            }
        }
    }
}
