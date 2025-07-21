using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FixedReducerList : MonoBehaviour
{
    public List<ReducerButton> fixedButtons;
    public BoxCollider2D boxCollider;
    public Vector2 offset;
    private Vector2 baseColliderOffset;
    public Vector3 basePosition;
    public MouseNode mouseNode;
    void Start()
    {
        baseColliderOffset = boxCollider.offset;
        basePosition = transform.localPosition;
        for (int i = 0; i < fixedButtons.Count; i++)
        {
            fixedButtons[i].transform.localPosition = new Vector3(0, i + 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (boxCollider.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
        {
            mouseNode.mouseOverUI = true;
            float maxHeight = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height)).y - (Camera.main.orthographicSize / 10);
            float minHeight = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 2)).y + (Camera.main.orthographicSize / 5);

            if (fixedButtons[0].transform.position.y + (-0.5f * Input.mouseScrollDelta.y) > minHeight && fixedButtons.Last().transform.position.y + (-0.5f * Input.mouseScrollDelta.y) > maxHeight)
            {
                if (fixedButtons[0].transform.position.y > minHeight + 0.01f)
                {
                    offset.y += maxHeight - fixedButtons.Last().transform.position.y;
                }
                else
                {
                    offset.y += minHeight - fixedButtons[0].transform.position.y;
                }
            }
            else if (fixedButtons[0].transform.position.y + (-0.5f * Input.mouseScrollDelta.y) < minHeight && fixedButtons.Last().transform.position.y + (-0.5f * Input.mouseScrollDelta.y) < maxHeight)
            {
                if (fixedButtons.Last().transform.position.y < maxHeight - 0.01f)
                {
                    offset.y += minHeight - fixedButtons[0].transform.position.y;
                }
                else
                {
                    offset.y += maxHeight - fixedButtons.Last().transform.position.y;
                }
            }
            else
            {
                offset += -0.5f * Input.mouseScrollDelta;
            }
        }
        boxCollider.offset = baseColliderOffset - offset;
        transform.localPosition = basePosition + (Vector3)offset;
    }
}
