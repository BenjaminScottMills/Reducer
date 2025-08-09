using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddReducerMenu : MonoBehaviour
{
    static Vector3 baseOffset = new Vector3(-4.2f, 0);
    public BoxCollider2D boxCollider;
    public CustomReducerList customReducerList;
    public FixedReducerList fixedReducerList;
    public InputField rName;
    public InputField description;
    public MouseNode mouseNode;
    void Update()
    {
        if (boxCollider.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
        {
            mouseNode.mouseOverUI = true;
            customReducerList.overReducerMenu = true;
            fixedReducerList.overReducerMenu = true;
        }
        else if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.mouseScrollDelta.y > 0.01f || Input.mouseScrollDelta.y < -0.01f)
        {
            gameObject.SetActive(false);
        }
    }

    public void Setup()
    {
        // Set all fields to default values
        var bottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0)).y;
        transform.localPosition = baseOffset;
        description.text = "";
        rName.text = "";

        if (transform.position.y - (Camera.main.orthographicSize / 2) < bottom)
        {
            transform.position = new Vector3(transform.position.x, bottom + (Camera.main.orthographicSize / 2));
        }
    }
}
