using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;

public class FolderButton : SidebarButton
{
    bool upInHiearchy;
    bool modifiable;
    const string goUpStr = "Parent Folder";
    public bool hoveredForDragDrop;
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
        hoveredForDragDrop = false;
        visual.gameObject.SetActive(false);
    }

    protected override void SetVis()
    {
        hoveredForDragDrop = false;
        visual.gameObject.SetActive(true);
        highlight.enabled = inputField.isFocused;
        DetermineIfModifiable();
    }

    protected override void TopHalfMouseOverlap()
    {
    }

    protected override void BottomHalfMouseOverlap()
    {
        if (beingDragged) return;
        highlight.enabled = true;
        hoveredForDragDrop = true;
        if (modifiable && (rfm.AnyHovered() || textboxCollider.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)))) return;

        mouseNode.tooltipText.text = upInHiearchy ? goUpStr : rFolder.folderName;
        if (Input.GetMouseButtonDown(0))
        {
            if (!upInHiearchy && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
            {
                mouseNode.StartDraggingSidebarButton(this);
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

    void DetermineIfModifiable()
    {
        if (upInHiearchy) return;

        modifiable = mouseNode.topMenu.selectedScreen != 'T';
        inputField.interactable = modifiable;
        rfm.gameObject.SetActive(modifiable);
    }

    public void RemoveFolder()
    {
        HashSet<Reducer> removedReds = rFolder.GetContainedReducers();
        
        if (removedReds.Contains(mouseNode.solution.currentReducer))
        {
            mouseNode.solution.currentReducer = mouseNode.solution.contents[0].r;
            
            mouseNode.selectedNodes.Clear();
            foreach (var node in mouseNode.solution.currentReducer.nodes)
            {
                node.gameObject.SetActive(true);
                node.nextConnector?.gameObject.SetActive(true);
            }
        }

        // remove the folder
        if (rFolder.parentFolder != null)
        {
            rFolder.parentFolder.contents.RemoveAll(rof => rof.f == rFolder);
        }

        foreach (Reducer solReducer in mouseNode.solution.reducers)
        {
            foreach (var node in solReducer.nodes)
            {
                if (removedReds.Contains(node.reducer))
                {
                    if (node.next != null)
                    {
                        if (node.blackLink) node.next.bPrev = null;
                        else node.next.wPrev = null;
                        Destroy(node.nextConnector.gameObject);
                    }

                    if (node.bPrev != null)
                    {
                        node.bPrev.next = null;
                        Destroy(node.bPrev.nextConnector.gameObject);
                        node.bPrev.nextConnector = null;
                    }

                    if (node.wPrev != null)
                    {
                        node.wPrev.next = null;
                        Destroy(node.wPrev.nextConnector.gameObject);
                        node.wPrev.nextConnector = null;
                    }

                    Destroy(node.gameObject);
                }
            }

            solReducer.nodes = solReducer.nodes.Where(n => !removedReds.Contains(n.reducer)).ToList();

            if (solReducer.child != null)
            {
                foreach (var node in solReducer.child.nodes)
                {
                    if (removedReds.Contains(node.reducer))
                    {
                        if (node.next != null)
                        {
                            if (node.blackLink) node.next.bPrev = null;
                            else node.next.wPrev = null;
                            Destroy(node.nextConnector.gameObject);
                        }

                        if (node.bPrev != null)
                        {
                            node.bPrev.next = null;
                            Destroy(node.bPrev.nextConnector.gameObject);
                            node.bPrev.nextConnector = null;
                        }

                        if (node.wPrev != null)
                        {
                            node.wPrev.next = null;
                            Destroy(node.wPrev.nextConnector.gameObject);
                            node.wPrev.nextConnector = null;
                        }

                        Destroy(node.gameObject);
                    }
                }

                solReducer.child.nodes = solReducer.child.nodes.Where(n => removedReds.Contains(n.reducer)).ToList();
            }
        }

        mouseNode.selectedNodes = mouseNode.selectedNodes.Where(n => !removedReds.Contains(n.reducer)).ToHashSet();

        customReducerList.RemoveButton(this);

        foreach (var reducer in removedReds)
        {
            foreach (var node in reducer.nodes)
            {
                if (node?.nextConnector?.gameObject != null) Destroy(node.nextConnector.gameObject);
                Destroy(node.gameObject);
            }
            Destroy(reducer.gameObject);
        }
    }

    public void ActivateUpInHiearchy()
    {
        inputField.text = goUpStr;
        inputField.DeactivateInputField();
        upInHiearchy = true;
        Destroy(rfm.gameObject);
        rfm = null;
        modifiable = false;
    }
}
