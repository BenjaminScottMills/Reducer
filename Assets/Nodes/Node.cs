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
    public bool breakpointAfter;
    public bool highlighted;
    public MouseNode mouseNode;
    public static float radius = 0.45f;
    public ReducerVisual reducerVisual;
    public SpriteRenderer highlightSpriteRenderer;
    public SortingGroup sortingGroup;
    public Connector nextConnector;

    public void InitialLoadFromSerialised(SolutionSerialise.NodeSerialise ns, List<Reducer> reducers, Reducer local)
    {
        id = ns.id;
        blackLink = ns.blackLink;
        bPrev = null;
        wPrev = null;
        SetHighlighted(false);
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

        reducerVisual.SetVisual(reducer.backgroundColour, reducer.foregroundColour, reducer.foregroundSprite);
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

    public static Node FastExecMakeNode(Node toAdd, Reducer black, Reducer white, Reducer outerBlack, Reducer outerWhite, Reducer local, Reducer currentReducer) // keep in mind args may be null. If they are null that means don't change the reducers.
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
        else if (toAdd.reducer.id == (int)Reducer.SpecialReducers.local)
        {
            if (local == null)
            {
                outNode.reducer = currentReducer;
            }
            else
            {
                outNode.reducer = local;
            }
        }
        else
        {
            outNode.reducer = toAdd.reducer;
        }

        outNode.bPrev = FastExecMakeNode(toAdd.bPrev, black, white, outerBlack, outerWhite, local, currentReducer);
        outNode.wPrev = FastExecMakeNode(toAdd.wPrev, black, white, outerBlack, outerWhite, local, currentReducer);
        return outNode;
    }

    public void RealignLinks()
    {
        if (next != null) nextConnector.Align(transform.position, next.transform.position, blackLink);
        if (bPrev != null) bPrev.nextConnector.Align(bPrev.transform.position, transform.position, true);
        if (wPrev != null) wPrev.nextConnector.Align(wPrev.transform.position, transform.position, false);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (mouseNode.mouseOverUI) return;

        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
        if (Vector3.Distance(mousePos, transform.position) < radius && !mouseNode.mouseOverUI && sortingGroup.sortingOrder >= mouseNode.highestNodeSortingOrderThisFrame)
        {
            mouseNode.highestNodeSortingOrderThisFrame = sortingGroup.sortingOrder;
            mouseNode.hoveredThisFrame = this;
        }
    }

    public void HandleShiftClick()
    {
        mouseNode.currentlyDragging = true;
        sortingGroup.sortingOrder = mouseNode.nodeSortingOrderCount;
        mouseNode.nodeSortingOrderCount++;

        if (highlighted)
        {
            mouseNode.currentlyDragging = false;

            mouseNode.selectedNodes.Remove(this);
            SetHighlighted(false);
        }
        else
        {
            mouseNode.selectedNodes.Add(this);
            SetHighlighted(true);
        }
    }

    public void HandleNoKeyClick()
    {
        mouseNode.currentlyDragging = true;
        sortingGroup.sortingOrder = mouseNode.nodeSortingOrderCount;
        mouseNode.nodeSortingOrderCount++;

        if (!highlighted)
        {
            foreach (var node in mouseNode.selectedNodes)
            {
                node.SetHighlighted(false);
            }

            SetHighlighted(true);
            mouseNode.selectedNodes = new HashSet<Node>() { this };
        }
    }

    public void HandleCtrlClick()
    {
        mouseNode.currentlyDragging = true;

        if (highlighted)
        {
            var selectedList = mouseNode.selectedNodes.ToList();
            var newList = new List<Node>();

            foreach (var node in selectedList)
            {
                var newNode = mouseNode.solution.currentReducer.AddNode(node.reducer, node.transform.position, mouseNode);
                newList.Add(newNode);
                newNode.SetHighlighted(true);
                mouseNode.hoveredThisFrame = newNode;
            }

            for (int i = 0; i < newList.Count(); i++)
            {
                int nextIdx = selectedList.FindIndex((n) => n == selectedList[i].next);

                if (nextIdx != -1)
                {
                    var newConnector = Instantiate(mouseNode.connectorPrefab, Vector3.zero, Quaternion.identity, transform.parent).GetComponent<Connector>();

                    newList[i].next = newList[nextIdx];
                    newList[i].blackLink = selectedList[i].blackLink;

                    if (selectedList[i].blackLink) newList[nextIdx].bPrev = newList[i];
                    else newList[nextIdx].wPrev = newList[i];

                    newList[i].nextConnector = newConnector;
                    newConnector.Align(newList[i].transform.position, newList[nextIdx].transform.position, newList[i].blackLink);
                }
            }

            foreach (var node in mouseNode.selectedNodes)
            {
                node.SetHighlighted(false);
            }

            mouseNode.selectedNodes = newList.ToHashSet();
        }
        else
        {
            foreach (var node in mouseNode.selectedNodes)
            {
                node.SetHighlighted(false);
            }

            var newNode = mouseNode.solution.currentReducer.AddNode(reducer, transform.position, mouseNode);
            mouseNode.selectedNodes = new HashSet<Node>() { newNode };
            newNode.SetHighlighted(true);
            mouseNode.hoveredThisFrame = newNode;
        }
    }

    public bool IsDescendentOf(Node candAncestor)
    {
        while (candAncestor != null)
        {
            if (candAncestor == this) return true;
            candAncestor = candAncestor.next;
        }
        return false;
    }

    public void SetHighlighted(bool setHighlight)
    {
        highlighted = setHighlight;
        highlightSpriteRenderer.enabled = setHighlight;
    }

    public void SetDragLayer(bool setDrag, HashSet<Node> dragNodes)
    {
        sortingGroup.sortingLayerName = setDrag ? "MouseNode" : "Nodes";
        if (next != null && dragNodes.Contains(next)) nextConnector.sortingGroup.sortingLayerName = setDrag ? "DragConnector" : "Connector";
    }
}
