using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;

public class FolderButton : SidebarButton
{
    bool upInHiearchy;
    const string goUpStr = "Parent Folder";
    public SpriteRenderer highlight;
    public Collider2D textboxCollider;
    public RemoveFolderManager rfm;
    public GameObject visual;
    public RFolder rFolder;
    public Canvas canvas;
    public InputField inputField;
    public CustomReducerList customReducerList;

    void Start()
    {
        upperHalf = false;
        if (!upInHiearchy) inputField.onEndEdit.AddListener((string newName) => {rFolder.folderName = newName;});
    }

    protected override void SetInvis()
    {
        visual.gameObject.SetActive(false);
    }

    protected override void SetVis()
    {
        visual.gameObject.SetActive(true);
        highlight.enabled = inputField.isFocused;
    }

    protected override void TopHalfMouseOverlap()
    {
    }

    protected override void BottomHalfMouseOverlap()
    {
        highlight.enabled = true;
        if (!upInHiearchy && (rfm.AnyHovered() || textboxCollider.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)))) return;

        mouseNode.tooltipText.text = upInHiearchy ? goUpStr : rFolder.folderName;
        if (Input.GetMouseButtonDown(0))
        {
            if (!upInHiearchy && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
            {
                Debug.Log("Drag the folder");
            }
            else
            {
                List<ReducerOrFolder> newFileContents;
                customReducerList.ResetList();
                mouseNode.solution.currentFolder = rFolder;

                if (upInHiearchy && rFolder == null)
                {
                    newFileContents = mouseNode.solution.contents;

                }
                else
                {
                    customReducerList.AddGoBackButton(rFolder.parentFolder);
                    newFileContents = rFolder.contents;
                }

                foreach (var rof in newFileContents)
                {
                    customReducerList.AddReducerOrFolderButton(rof, false);
                }
                customReducerList.ResetPosition(); // make it so that customButtons[0] is at the top edge
            }
        }
    }

    public void RemoveFolder()
    {
        Debug.Log("Remove folder");
    }

    public void ActivateUpInHiearchy()
    {
        inputField.text = goUpStr;
        inputField.DeactivateInputField();
        upInHiearchy = true;
        Destroy(rfm.gameObject);
    }
}
