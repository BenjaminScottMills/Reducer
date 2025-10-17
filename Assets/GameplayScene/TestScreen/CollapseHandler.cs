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
    Vector3 targetPosition = Vector3.zero;
    const int speedMultiple = 15;

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
            targetPosition = collapseOffset;
        }
        else
        {
            targetPosition = Vector3.zero;
        }

        float speed = Time.deltaTime * speedMultiple;

        if (targetPosition.x > transform.localPosition.x + speed)
        {
            transform.localPosition += new Vector3(speed, 0);
        }
        else if (targetPosition.x < transform.localPosition.x - speed)
        {
            transform.localPosition += new Vector3(-speed, 0);
        }
        else
        {
            transform.localPosition = new Vector3(targetPosition.x, transform.localPosition.y);
        }

        if (targetPosition.y > transform.localPosition.y + speed)
        {
            transform.localPosition += new Vector3(0, speed);
        }
        else if (targetPosition.y < transform.localPosition.y - speed)
        {
            transform.localPosition += new Vector3(0, -speed);
        }
        else
        {
            transform.localPosition = new Vector3(transform.localPosition.x, targetPosition.y);
        }

        mouseInRange = false;
    }
}
