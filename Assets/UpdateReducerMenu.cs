using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateReducerMenu : MonoBehaviour
{
    static Vector3 baseOffset = new Vector3(-4.2f, 0);
    public BoxCollider2D boxCollider;
    public CustomReducerList customReducerList;
    public FixedReducerList fixedReducerList;
    public InputField rName;
    public InputField description;
    public MouseNode mouseNode;
    public Reducer reducer;
    public Canvas canvas;

    public void Setup()
    {
        // Set all fields to default values
        var bottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0)).y;
        transform.localPosition = baseOffset;
        description.text = reducer.description;
        rName.text = reducer.rName;

        if (transform.position.y - 2 < bottom)
        {
            transform.position = new Vector3(transform.position.x, bottom + 2);
        }
    }

    // Update is called once per frame
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

    public void UpdateReducer(string rName, string desc)
    {
        reducer.rName = rName;
        reducer.description = desc;
    }
}
