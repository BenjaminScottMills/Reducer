using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CustomReducerList : MonoBehaviour
{
    public GameObject reducerButtonPrefab;
    public GameObject folderButtonPrefab;
    public List<SidebarButton> customButtons;
    public GameObject newReducerAndFolderButtons;
    public FixedReducerList fixedReducerList;
    public BoxCollider2D boxCollider;
    public Vector2 offset;
    private Vector2 baseColliderOffset;
    public Vector3 basePosition;
    public MouseNode mouseNode;
    public bool overReducerMenu;
    public bool mouseOverForDragDropLocation;

    // Start is called before the first frame update
    void Start()
    {
        baseColliderOffset = boxCollider.offset;
        basePosition = transform.localPosition;
    }

    public void AddReducerButton(Reducer r, bool setActive = true, bool updatePosition = true)
    {
        var newButton = Instantiate(reducerButtonPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<ReducerButton>();
        newButton.reducer = r;
        newButton.transform.localPosition = newReducerAndFolderButtons.transform.localPosition;
        newButton.updateMenu.customReducerList = this;
        newButton.updateMenu.fixedReducerList = fixedReducerList;
        newButton.updateMenu.mouseNode = mouseNode;
        newButton.updateMenu.canvas.worldCamera = Camera.main;
        newButton.updateMenu.reducer = r;
        newButton.mouseNode = mouseNode;
        customButtons.Add(newButton);
        newReducerAndFolderButtons.transform.localPosition += new Vector3(0, -1);

        if (updatePosition) AddButtonPositionUpdate();

        if (r.solution.localReducersUnlocked)
        {
            var localButton = Instantiate(reducerButtonPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<ReducerButton>();

            localButton.reducer = r.child;
            localButton.transform.localPosition = newButton.transform.localPosition;
            localButton.mouseNode = mouseNode;

            newButton.childButton = localButton;
            localButton.parentButton = newButton;

            localButton.transform.localPosition += new Vector3(0.55f, 0);
            newButton.transform.localPosition += new Vector3(-0.55f, 0);
        }

        if (setActive) r.SetReducerActive(mouseNode);
    }

    public void AddFolderButton(RFolder f, bool updatePosition = true)
    {
        var newButton = Instantiate(folderButtonPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<FolderButton>();
        newButton.inputField.text = f.folderName;
        newButton.transform.localPosition = newReducerAndFolderButtons.transform.localPosition;
        newButton.canvas.worldCamera = Camera.main;
        newButton.rFolder = f;
        newButton.customReducerList = this;
        newButton.mouseNode = mouseNode;
        customButtons.Add(newButton);
        newReducerAndFolderButtons.transform.localPosition += new Vector3(0, -1);

        if (updatePosition) AddButtonPositionUpdate();
    }

    public void AddGoBackButton(RFolder f)
    {
        var newButton = Instantiate(folderButtonPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<FolderButton>();
        newButton.transform.localPosition = newReducerAndFolderButtons.transform.localPosition;
        newButton.canvas.worldCamera = Camera.main;
        newButton.rFolder = f;
        newButton.customReducerList = this;
        newButton.mouseNode = mouseNode;
        newButton.fixedAtTop = true;
        customButtons.Add(newButton);
        newReducerAndFolderButtons.transform.localPosition += new Vector3(0, -1);

        newButton.ActivateUpInHiearchy();
    }

    public void AddReducerOrFolderButton(ReducerOrFolder rof, bool updatePosition = true)
    {
        if (rof.IsReducer())
        {
            AddReducerButton(rof.r, false, updatePosition);
        }
        else
        {
            AddFolderButton(rof.f, updatePosition);
        }
    }

    private void AddButtonPositionUpdate()
    {
        float minHeight = Camera.main.ScreenToWorldPoint(new Vector3(0, 0)).y + (Camera.main.orthographicSize / 10);
        float maxHeight = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 2)).y - (Camera.main.orthographicSize / 5);

        if (newReducerAndFolderButtons.transform.position.y < minHeight && customButtons[0].transform.position.y < maxHeight)
        {
            if (customButtons[0].transform.position.y + (minHeight - newReducerAndFolderButtons.transform.position.y) < maxHeight)
            {
                transform.position += new Vector3(0, minHeight - newReducerAndFolderButtons.transform.position.y);
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

        if (newReducerAndFolderButtons.transform.position.y > minHeight && customButtons[0].transform.position.y > maxHeight)
        {
            if (customButtons[0].transform.position.y + (minHeight - newReducerAndFolderButtons.transform.position.y) > maxHeight)
            {
                transform.position += new Vector3(0, minHeight - newReducerAndFolderButtons.transform.position.y);
            }
            else
            {
                transform.position += new Vector3(0, maxHeight - customButtons[0].transform.position.y);
            }

            offset = transform.localPosition - basePosition;
        }
    }

    public void RemoveButton(SidebarButton sidebarButton)
    {
        int buttonIdx = customButtons.FindIndex(b => b == sidebarButton);
        for (int i = buttonIdx; i < customButtons.Count(); i++)
        {
            customButtons[i].transform.localPosition += new Vector3(0, 1);

            if ((customButtons[i] as ReducerButton)?.childButton != null)
            {
                (customButtons[i] as ReducerButton).childButton.transform.localPosition += new Vector3(0, 1);
            }
        }
        
        newReducerAndFolderButtons.transform.localPosition += new Vector3(0, 1);
        customButtons.RemoveAt(buttonIdx);

        ButtonRemoveUpdate();

        Destroy(sidebarButton.gameObject);
        if ((sidebarButton as ReducerButton)?.childButton != null)
        {
            Destroy((sidebarButton as ReducerButton).childButton.gameObject);
        }
    }

    public void ResetList()
    {
        newReducerAndFolderButtons.transform.localPosition = new Vector3(0, -1f);
        foreach (var button in customButtons)
        {
            Destroy(button.gameObject);
            if ((button as ReducerButton)?.childButton != null)
            {
                Destroy((button as ReducerButton).childButton.gameObject);
            }
        }
        customButtons.Clear();
    }

    public void ActivateTestMode()
    {
        newReducerAndFolderButtons.transform.localPosition += new Vector3(0, 1);
        newReducerAndFolderButtons.gameObject.SetActive(false);

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
        newReducerAndFolderButtons.transform.localPosition += new Vector3(0, -1);
        newReducerAndFolderButtons.SetActive(true);

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

    public void ResetPosition()
    {
        offset = Vector3.zero;
        transform.localPosition = basePosition;
    }

    // Update is called once per frame
    void Update()
    {
        mouseOverForDragDropLocation = false;
        if (boxCollider.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)) && !mouseNode.onImportScreen)
        {
            mouseOverForDragDropLocation = true;
            mouseNode.mouseOverUI = true;

            if (!overReducerMenu)
            {
                float minHeight = Camera.main.ScreenToWorldPoint(new Vector3(0, 0)).y + (Camera.main.orthographicSize / 10);
                float maxHeight = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 2)).y - (Camera.main.orthographicSize / 5);

                if (newReducerAndFolderButtons.transform.position.y + (-0.5f * Input.mouseScrollDelta.y) > minHeight && customButtons[0].transform.position.y + (-0.5f * Input.mouseScrollDelta.y) > maxHeight)
                {
                    if (newReducerAndFolderButtons.transform.position.y > minHeight + 0.01f)
                    {
                        transform.position += new Vector3(0, maxHeight - customButtons[0].transform.position.y);
                    }
                    else
                    {
                        transform.position += new Vector3(0, minHeight - newReducerAndFolderButtons.transform.position.y);
                    }

                    offset = transform.localPosition - basePosition;
                }
                else if (newReducerAndFolderButtons.transform.position.y + (-0.5f * Input.mouseScrollDelta.y) < minHeight && customButtons[0].transform.position.y + (-0.5f * Input.mouseScrollDelta.y) < maxHeight)
                {
                    if (customButtons[0].transform.position.y < maxHeight - 0.01f)
                    {
                        transform.position += new Vector3(0, minHeight - newReducerAndFolderButtons.transform.position.y);
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
