using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomReducerList : MonoBehaviour
{
    public GameObject reducerButtonPrefab;
    public GameObject folderButtonPrefab;
    public List<SidebarButton> customButtons;
    public GameObject newButtons;
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

    public void AddReducerButton(Reducer r, bool setActive = true)
    {
        var newButton = Instantiate(reducerButtonPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<ReducerButton>();
        newButton.reducer = r;
        newButton.transform.localPosition = newButtons.transform.localPosition;
        newButton.updateMenu.customReducerList = this;
        newButton.updateMenu.fixedReducerList = fixedReducerList;
        newButton.updateMenu.mouseNode = mouseNode;
        newButton.updateMenu.canvas.worldCamera = Camera.main;
        newButton.updateMenu.reducer = r;
        newButton.mouseNode = mouseNode;
        customButtons.Add(newButton);
        newButtons.transform.localPosition += new Vector3(0, -1);

        AddButtonPositionUpdate();

        if (r.solution.localReducersUnlocked)
        {
            var localButton = Instantiate(reducerButtonPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<ReducerButton>();

            localButton.reducer = r.child;
            localButton.transform.localPosition = newButton.transform.localPosition;
            localButton.mouseNode = mouseNode;

            newButton.childButton = localButton;

            localButton.transform.localPosition += new Vector3(0.55f, 0);
            newButton.transform.localPosition += new Vector3(-0.55f, 0);
        }

        if (setActive) r.SetReducerActive(mouseNode);
    }

    public void AddFolderButton(RFolder f)
    {
        var newButton = Instantiate(folderButtonPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<FolderButton>();
        newButton.inputField.text = f.folderName;
        newButton.transform.localPosition = newButtons.transform.localPosition;
        newButton.canvas.worldCamera = Camera.main;
        newButton.rFolder = f;
        newButton.customReducerList = this;
        newButton.mouseNode = mouseNode;
        customButtons.Add(newButton);
        newButtons.transform.localPosition += new Vector3(0, -1);

        AddButtonPositionUpdate();
    }

    public void AddReducerOrFolderButton(ReducerOrFolder rof)
    {
        if (rof.IsReducer())
        {
            AddReducerButton(rof.r);
        }
        else
        {
            AddFolderButton(rof.f);
        }
    }

    private void AddButtonPositionUpdate()
    {
        float minHeight = Camera.main.ScreenToWorldPoint(new Vector3(0, 0)).y + (Camera.main.orthographicSize / 10);
        float maxHeight = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 2)).y - (Camera.main.orthographicSize / 5);

        if (newButtons.transform.position.y < minHeight && customButtons[0].transform.position.y < maxHeight)
        {
            if (customButtons[0].transform.position.y + (minHeight - newButtons.transform.position.y) < maxHeight)
            {
                transform.position += new Vector3(0, minHeight - newButtons.transform.position.y);
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

        if (newButtons.transform.position.y > minHeight && customButtons[0].transform.position.y > maxHeight)
        {
            if (customButtons[0].transform.position.y + (minHeight - newButtons.transform.position.y) > maxHeight)
            {
                transform.position += new Vector3(0, minHeight - newButtons.transform.position.y);
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
        newButtons.transform.localPosition += new Vector3(0, 1);
        newButtons.gameObject.SetActive(false);

        ButtonRemoveUpdate();

        foreach (var button in customButtons)
        {
            ReducerButton rButton = button as ReducerButton;

            if (rButton?.childButton != null)
            {
                rButton.childButton.gameObject.SetActive(false);
                rButton.transform.localPosition += new Vector3(0.55f, 0);
            }
        }
    }

    public void DeactivateTestMode()
    {
        newButtons.transform.localPosition += new Vector3(0, -1);
        newButtons.SetActive(true);

        AddButtonPositionUpdate();

        foreach (var button in customButtons)
        {
            ReducerButton rButton = button as ReducerButton;

            if (rButton?.childButton != null)
            {
                rButton.childButton.gameObject.SetActive(true);
                rButton.transform.localPosition += new Vector3(-0.55f, 0);
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

                if (newButtons.transform.position.y + (-0.5f * Input.mouseScrollDelta.y) > minHeight && customButtons[0].transform.position.y + (-0.5f * Input.mouseScrollDelta.y) > maxHeight)
                {
                    if (newButtons.transform.position.y > minHeight + 0.01f)
                    {
                        transform.position += new Vector3(0, maxHeight - customButtons[0].transform.position.y);
                    }
                    else
                    {
                        transform.position += new Vector3(0, minHeight - newButtons.transform.position.y);
                    }

                    offset = transform.localPosition - basePosition;
                }
                else if (newButtons.transform.position.y + (-0.5f * Input.mouseScrollDelta.y) < minHeight && customButtons[0].transform.position.y + (-0.5f * Input.mouseScrollDelta.y) < maxHeight)
                {
                    if (customButtons[0].transform.position.y < maxHeight - 0.01f)
                    {
                        transform.position += new Vector3(0, minHeight - newButtons.transform.position.y);
                    }
                    else
                    {
                        transform.position += new Vector3(0, maxHeight - customButtons[0].transform.position.y);
                    }

                    offset = transform.localPosition - basePosition;
                }
                else
                {
                    offset.y += -0.5f * Input.mouseScrollDelta.y;
                }
            }
        }
        boxCollider.offset = baseColliderOffset - offset;
        transform.localPosition = basePosition + (Vector3)offset;
        overReducerMenu = false;
    }
}
