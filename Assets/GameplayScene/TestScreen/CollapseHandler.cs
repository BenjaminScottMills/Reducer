using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapseHandler : MonoBehaviour
{
    public Vector3 collapseOffset;
    public BoxCollider2D boxCollider;
    public CollapseHandler otherCH;
    public TopMenu tMenu;
    public bool mouseInRange;
    public TestScreen tScreen;
    // Update is called once per frame
    void Update()
    {
        if (Input.mousePosition.x > Screen.width || Input.mousePosition.y > Screen.height || boxCollider.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
        {
            otherCH.mouseInRange = true;
            mouseInRange = true;
        }

        if (tMenu.selectedScreen == 'T' && tScreen.collapseMenus && !mouseInRange)
        {
            transform.localPosition = collapseOffset;
        }
        else
        {
            transform.localPosition = Vector3.zero;
        }

        mouseInRange = false;
    }
}
