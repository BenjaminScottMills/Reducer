using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public bool localReducersUnlocked = true;

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
        newButton.transform.localPosition = new Vector3(0, customButtons.Last().transform.localPosition.y - 1);
        newButton.updateMenu.customReducerList = this;
        newButton.updateMenu.fixedReducerList = fixedReducerList;
        newButton.updateMenu.mouseNode = mouseNode;
        newButton.updateMenu.canvas.worldCamera = Camera.main;
        newButton.updateMenu.reducer = r;
        newButton.mouseNode = mouseNode;
        customButtons.Add(newButton);
        newReducerButton.transform.localPosition += new Vector3(0, -1);

        float minHeight = Camera.main.ScreenToWorldPoint(new Vector3(0, 0)).y + (Camera.main.orthographicSize / 10);
        float maxHeight = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 2)).y - (Camera.main.orthographicSize / 5);

        if (newReducerButton.transform.position.y - 0.5f < minHeight && customButtons[0].transform.position.y - 0.5f < maxHeight)
        {
            if (customButtons[0].transform.position.y < maxHeight - 0.01f)
            {
                offset.y += minHeight - newReducerButton.transform.position.y;
            }
            else
            {
                offset.y += maxHeight - customButtons[0].transform.position.y;
            }
        }

        if (newReducerButton.transform.position.y - 0.5f > minHeight && customButtons[0].transform.position.y - 0.5f > maxHeight)
        {
            if (newReducerButton.transform.position.y > minHeight + 0.01f)
            {
                offset.y += maxHeight - customButtons[0].transform.position.y;
            }
            else
            {
                offset.y += minHeight - newReducerButton.transform.position.y;
            }
        }

        if (localReducersUnlocked)
        {
            var localButton = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<ReducerButton>();
            var localReducer = Instantiate(r.solution.reducerPrefab).GetComponent<Reducer>();
            r.child = localReducer;
            localReducer.isChild = true;
            localReducer.rName = r.rName + " - child";
            localReducer.description = "";
            localReducer.id = (int)Reducer.SpecialReducers.local;
            localReducer.nullReducer = r.nullReducer;
            localReducer.solution = r.solution;
            localReducer.foregroundColour = 1;
            localReducer.backgroundColour = 0;
            localReducer.foregroundSprite = 9;

            localButton.reducer = localReducer;
            localButton.transform.localPosition = newButton.transform.localPosition;
            localButton.mouseNode = mouseNode;

            localButton.transform.localPosition += new Vector3(0.55f, 0);
            newButton.transform.localPosition += new Vector3(-0.55f, 0);
        }
    }

    public void ButtonRemoveUpdate()
    {
        float minHeight = Camera.main.ScreenToWorldPoint(new Vector3(0, 0)).y + (Camera.main.orthographicSize / 10);
        float maxHeight = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 2)).y - (Camera.main.orthographicSize / 5);

        if (newReducerButton.transform.position.y - 0.5f > minHeight && customButtons[0].transform.position.y - 0.5f > maxHeight)
        {
            if (customButtons[0].transform.position.y + (minHeight - newReducerButton.transform.position.y) - 0.5f > maxHeight)
            {
                offset.y += minHeight - newReducerButton.transform.position.y;
            }
            else
            {
                offset.y += maxHeight - customButtons[0].transform.position.y;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (boxCollider.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
        {
            mouseNode.mouseOverUI = true;

            if (!overReducerMenu)
            {
                float minHeight = Camera.main.ScreenToWorldPoint(new Vector3(0, 0)).y + (Camera.main.orthographicSize / 10);
                float maxHeight = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 2)).y - (Camera.main.orthographicSize / 5);

                if (newReducerButton.transform.position.y + (-0.5f * Input.mouseScrollDelta.y) > minHeight && customButtons[0].transform.position.y + (-0.5f * Input.mouseScrollDelta.y) > maxHeight)
                {
                    if (newReducerButton.transform.position.y > minHeight + 0.01f)
                    {
                        offset.y += maxHeight - customButtons[0].transform.position.y;
                    }
                    else
                    {
                        offset.y += minHeight - newReducerButton.transform.position.y;
                    }
                }
                else if (newReducerButton.transform.position.y + (-0.5f * Input.mouseScrollDelta.y) < minHeight && customButtons[0].transform.position.y + (-0.5f * Input.mouseScrollDelta.y) < maxHeight)
                {
                    if (customButtons[0].transform.position.y < maxHeight - 0.01f)
                    {
                        offset.y += minHeight - newReducerButton.transform.position.y;
                    }
                    else
                    {
                        offset.y += maxHeight - customButtons[0].transform.position.y;
                    }
                }
                else
                {
                    offset += -0.5f * Input.mouseScrollDelta;
                }
            }
        }
        boxCollider.offset = baseColliderOffset - offset;
        transform.localPosition = basePosition + (Vector3)offset;
        overReducerMenu = false;
    }
}
