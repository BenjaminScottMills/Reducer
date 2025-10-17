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
        newButton.transform.localPosition = newReducerButton.transform.localPosition;
        newButton.updateMenu.customReducerList = this;
        newButton.updateMenu.fixedReducerList = fixedReducerList;
        newButton.updateMenu.mouseNode = mouseNode;
        newButton.updateMenu.canvas.worldCamera = Camera.main;
        newButton.updateMenu.reducer = r;
        newButton.mouseNode = mouseNode;
        customButtons.Add(newButton);
        newReducerButton.transform.localPosition += new Vector3(0, -1);

        AddReducerButtonPositionUpdate();

        if (r.solution.localReducersUnlocked)
        {
            var localButton = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<ReducerButton>();

            localButton.reducer = r.child;
            localButton.transform.localPosition = newButton.transform.localPosition;
            localButton.mouseNode = mouseNode;

            newButton.childButton = localButton;

            localButton.transform.localPosition += new Vector3(0.55f, 0);
            newButton.transform.localPosition += new Vector3(-0.55f, 0);
        }

        r.SetReducerActive(mouseNode);
    }

    private void AddReducerButtonPositionUpdate()
    {
        float minHeight = Camera.main.ScreenToWorldPoint(new Vector3(0, 0)).y + (Camera.main.orthographicSize / 10);
        float maxHeight = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 2)).y - (Camera.main.orthographicSize / 5);

        if (newReducerButton.transform.position.y < minHeight && customButtons[0].transform.position.y < maxHeight)
        {
            if (customButtons[0].transform.position.y + (minHeight - newReducerButton.transform.position.y) < maxHeight)
            {
                transform.position += new Vector3(0, minHeight - newReducerButton.transform.position.y);
            }
            else
            {
                transform.position += new Vector3(0, maxHeight - customButtons[0].transform.position.y);
            }

            offset = transform.localPosition - basePosition;
        }
    }

    public void ButtonRemoveUpdate()
    {
        float minHeight = Camera.main.ScreenToWorldPoint(new Vector3(0, 0)).y + (Camera.main.orthographicSize / 10);
        float maxHeight = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 2)).y - (Camera.main.orthographicSize / 5);

        if (newReducerButton.transform.position.y > minHeight && customButtons[0].transform.position.y > maxHeight)
        {
            if (customButtons[0].transform.position.y + (minHeight - newReducerButton.transform.position.y) > maxHeight)
            {
                transform.position += new Vector3(0, minHeight - newReducerButton.transform.position.y);
            }
            else
            {
                transform.position += new Vector3(0, maxHeight - customButtons[0].transform.position.y);
            }

            offset = transform.localPosition - basePosition;
        }
    }

    public void ActivateTestMode()
    {
        newReducerButton.transform.localPosition += new Vector3(0, 1);
        newReducerButton.gameObject.SetActive(false);

        ButtonRemoveUpdate();

        foreach (var button in customButtons)
        {
            if (button.childButton != null)
            {
                button.childButton.gameObject.SetActive(false);
                button.transform.localPosition += new Vector3(0.55f, 0);
            }
        }
    }

    public void DeactivateTestMode()
    {
        newReducerButton.transform.localPosition += new Vector3(0, -1);
        newReducerButton.gameObject.SetActive(true);

        AddReducerButtonPositionUpdate();

        foreach (var button in customButtons)
        {
            if (button.childButton != null)
            {
                button.childButton.gameObject.SetActive(true);
                button.transform.localPosition += new Vector3(-0.55f, 0);
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
                        transform.position += new Vector3(0, maxHeight - customButtons[0].transform.position.y);
                    }
                    else
                    {
                        transform.position += new Vector3(0, minHeight - newReducerButton.transform.position.y);
                    }

                    offset = transform.localPosition - basePosition;
                }
                else if (newReducerButton.transform.position.y + (-0.5f * Input.mouseScrollDelta.y) < minHeight && customButtons[0].transform.position.y + (-0.5f * Input.mouseScrollDelta.y) < maxHeight)
                {
                    if (customButtons[0].transform.position.y < maxHeight - 0.01f)
                    {
                        transform.position += new Vector3(0, minHeight - newReducerButton.transform.position.y);
                    }
                    else
                    {
                        transform.position += new Vector3(0, maxHeight - customButtons[0].transform.position.y);
                    }

                    offset = transform.localPosition - basePosition;
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
