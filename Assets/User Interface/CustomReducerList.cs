using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomReducerList : MonoBehaviour
{
    public GameObject buttonPrefab;
    public List<ReducerButton> customButtons;
    public NewReducerButton newReducerButton;
    public FixedReducerList fixedReducerList;
    public BoxCollider2D boxCollider;
    public Vector2 offset;
    private Vector2 baseColliderOffset;
    public Vector3 basePosition;
    public MouseNode mouseNode;
    public bool overReducerMenu;

    // Start is called before the first frame update
    void Start()
    {
        baseColliderOffset = boxCollider.offset;
        basePosition = transform.localPosition;
    }

    public void AddReducerButton(Reducer r)
    {
        var newButton = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<ReducerButton>();
        newButton.reducer = r;
        newButton.transform.localPosition = new Vector3(0, transform.localPosition.y + ((customButtons.Count + 1) * -1));
        newButton.updateMenu.customReducerList = this;
        newButton.updateMenu.fixedReducerList = fixedReducerList;
        newButton.updateMenu.mouseNode = mouseNode;
        newButton.updateMenu.canvas.worldCamera = Camera.main;
        newButton.updateMenu.reducer = r;
        newButton.mouseNode = mouseNode;
        customButtons.Add(newButton);
        newReducerButton.transform.localPosition += new Vector3(0, -1);
    }

    // Update is called once per frame
    void Update()
    {
        if (boxCollider.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
        {
            mouseNode.mouseOverUI = true;

            if (!overReducerMenu) offset += -0.5f * Input.mouseScrollDelta;
        }
        boxCollider.offset = baseColliderOffset - offset;
        transform.localPosition = basePosition + (Vector3)offset;
        overReducerMenu = false;
    }
}
