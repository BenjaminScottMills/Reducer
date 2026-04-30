using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;

public class FolderButton : SidebarButton
{
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
        inputField.onEndEdit.AddListener((string newName) => {rFolder.folderName = newName;});
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
        if (rfm.AnyHovered() || textboxCollider.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition))) return;

        mouseNode.tooltipText.text = rFolder.folderName;
        if (Input.GetMouseButtonDown(0))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                Debug.Log("Drag the folder");
            }
            else
            {
                Debug.Log("Enter the folder");
            }
        }
    }

    public void RemoveFolder()
    {
        Debug.Log("Remove folder");
    }
}
