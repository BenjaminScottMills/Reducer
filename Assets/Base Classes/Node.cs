using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class Node : MonoBehaviour
{
    public Reducer reducer;
    public uint id;
    public Node next;
    public Node bPrev;
    public Node wPrev;
    public bool blackLink;
    public bool highlighted;
    public MouseNode mouseNode;
    private static float radius = 0.45f;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer highlightSpriteRenderer;
    public SortingGroup sortingGroup;

    public void InitialLoadFromSerialised(SolutionSerialise.NodeSerialise ns, List<Reducer> reducers, Reducer local)
    {
        id = ns.id;
        blackLink = ns.blackLink;
        bPrev = null;
        wPrev = null;
        highlighted = false;
        highlightSpriteRenderer.enabled = false;
        sortingGroup.sortingOrder = mouseNode.nodeSortingOrderCount;
        mouseNode.nodeSortingOrderCount++;

        if (ns.redId < 50)
        {
            switch (ns.redId)
            {
                case (int)Reducer.SpecialReducers.local:
                    reducer = local;
                    break;
                default:
                    reducer = null;
                    break;
            }
        }
        else
        {
            reducer = reducers.First(r => r.id == ns.redId);
        }

        spriteRenderer.sprite = reducer.sprite;
    }

    public void CreateLinksFromSerialised(SolutionSerialise.NodeSerialise ns, List<Node> allNodes)
    {
        next = allNodes?.FirstOrDefault(n => n.id == ns.nextId);
        if (next != null)
        {
            if (blackLink)
            {
                next.bPrev = this;
            }
            else
            {
                next.wPrev = this;
            }
        }
    }

    public Reducer ExecuteFast()
    {
        return reducer.ExecuteFast(bPrev.ExecuteFast(), wPrev.ExecuteFast());
    }

    public static Node FastExecMakeNode(Node toAdd, Reducer black, Reducer white, Reducer outerBlack, Reducer outerWhite, Reducer local) // keep in mind args may be null. If they are null that means don't change the reducers.
    {
        if (toAdd == null) return null;

        var outNode = new Node();
        if (toAdd.reducer.id == (int)Reducer.SpecialReducers.black && black != null)
        {
            outNode.reducer = black;
        }
        else if (toAdd.reducer.id == (int)Reducer.SpecialReducers.white && white != null)
        {
            outNode.reducer = white;
        }
        else if (toAdd.reducer.id == (int)Reducer.SpecialReducers.outerBlack && outerBlack != null)
        {
            outNode.reducer = outerBlack;
        }
        else if (toAdd.reducer.id == (int)Reducer.SpecialReducers.outerWhite && outerWhite != null)
        {
            outNode.reducer = outerWhite;
        }
        else if (toAdd.reducer.id == (int)Reducer.SpecialReducers.local && local != null)
        {
            outNode.reducer = local;
        }
        else
        {
            outNode.reducer = toAdd.reducer;
        }

        outNode.bPrev = FastExecMakeNode(toAdd.bPrev, black, white, outerBlack, outerWhite, local);
        outNode.wPrev = FastExecMakeNode(toAdd.wPrev, black, white, outerBlack, outerWhite, local);
        return outNode;
    }

    public void RealignLinks()
    {
        // make the links correctly positioned.
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetMouseButtonDown(0) || mouseNode.mouseOverUI) return;

        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
        if (Vector3.Distance(mousePos, transform.position) < radius && !mouseNode.mouseOverUI && sortingGroup.sortingOrder >= mouseNode.highestNodeSortingOrderThisFrame)
        {
            mouseNode.highestNodeSortingOrderThisFrame = sortingGroup.sortingOrder;
            mouseNode.clickedThisFrame = this;
        }
    }

    public void HandleClick()
    {
        mouseNode.currentlyDragging = true;
        sortingGroup.sortingOrder = mouseNode.nodeSortingOrderCount;
        mouseNode.nodeSortingOrderCount++;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (highlighted)
            {
                mouseNode.currentlyDragging = false;

                mouseNode.selectedNodes.Remove(this);
                highlighted = false;
                highlightSpriteRenderer.enabled = false;
            }
            else
            {
                mouseNode.selectedNodes.Add(this);
                highlighted = true;
                highlightSpriteRenderer.enabled = true;
            }
        }
        else
        {
            if (!highlighted)
            {
                foreach (var node in mouseNode.selectedNodes)
                {
                    node.highlighted = false;
                    node.highlightSpriteRenderer.enabled = false;
                }

                highlighted = true;
                highlightSpriteRenderer.enabled = true;
                mouseNode.selectedNodes = new HashSet<Node>() { this };
            }
        }
    }
}
