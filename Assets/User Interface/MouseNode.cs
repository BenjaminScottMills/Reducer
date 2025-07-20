using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseNode : MonoBehaviour
{
    public Reducer reducer;
    public Solution solution;
    public Vector3 offset = new Vector3(0, 0, 10);
    public SpriteRenderer spriteRenderer;
    public Sprite blank;
    public bool currentlyDragging;
    public bool mouseOverUI;
    public int nodeSortingOrderCount = 1;
    public int highestNodeSortingOrderThisFrame;
    public Node hoveredThisFrame = null;
    public HashSet<Node> selectedNodes;
    public GameObject connectorPrefab;
    public Connector newConnector;
    public Node newConnectorStart;
    public HighlightSquare highlightSquare;
    private Vector3 prevMousePos = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        selectedNodes = new HashSet<Node>();
    }

    // Update is called once per frame
    void Update()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var newPos = mousePos + offset;
        bool leftClickPressed = Input.GetMouseButtonDown(0);
        bool leftClickHeld = Input.GetMouseButton(0);
        bool rightClickPressed = Input.GetMouseButtonDown(1) && !leftClickHeld;
        bool rightClickHeld = Input.GetMouseButton(1) && !leftClickHeld;
        bool shiftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        bool ctrlHeld = !shiftHeld && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl));
        bool noKeyHeld = !shiftHeld && !ctrlHeld;

        if (!mouseOverUI)
        {
            Camera.main.orthographicSize = (float)Math.Round(2 * (Camera.main.orthographicSize - (Input.mouseScrollDelta.y / 2))) / 2;

            if (Camera.main.orthographicSize > 8)
            {
                Camera.main.orthographicSize = 8;
            }
            else if (Camera.main.orthographicSize < 3)
            {
                Camera.main.orthographicSize = 3;
            }

            Camera.main.transform.position +=  mousePos - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        mousePos += new Vector3(0, 0, 10);

        if (hoveredThisFrame != null && leftClickPressed) hoveredThisFrame.HandleClick();

        if (!currentlyDragging && leftClickPressed)
        {
            if (!shiftHeld)
            {
                foreach (var node in selectedNodes)
                {
                    node.SetHighlighted(false);
                }

                selectedNodes.Clear();
            }

            if (!mouseOverUI && hoveredThisFrame == null) highlightSquare.Enable(mousePos);
        }

        if (leftClickHeld && currentlyDragging)
        {
            var displacement = newPos - transform.position;
            foreach (var node in selectedNodes)
            {
                node.transform.position += displacement;
                node.SetDragLayer(true, selectedNodes);
            }
            foreach (var node in selectedNodes)
            {
                node.RealignLinks();
            }
        }
        else
        {
            if (currentlyDragging && mouseOverUI)
            {
                foreach (var node in selectedNodes)
                {
                    if (node.next)
                    {
                        Destroy(node.nextConnector.gameObject);
                        if (node.blackLink) node.next.bPrev = null;
                        else node.next.wPrev = null;
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

                    solution.reducers[solution.currentReducerIdx].nodes.Remove(node);
                    Destroy(node.gameObject);
                }

                selectedNodes.Clear();
            }
            else if (currentlyDragging)
            {
                foreach (var node in selectedNodes) node.SetDragLayer(false, selectedNodes);
            }
            currentlyDragging = false;
        }

        if (newConnector != null)
        {
            if (!rightClickHeld)
            {
                if (hoveredThisFrame != null && hoveredThisFrame != newConnectorStart && !newConnectorStart.IsDescendentOf(hoveredThisFrame))
                {
                    if (hoveredThisFrame.bPrev == null)
                    {
                        hoveredThisFrame.bPrev = newConnectorStart;
                        newConnectorStart.next = hoveredThisFrame;
                        newConnectorStart.nextConnector = newConnector;
                        newConnectorStart.blackLink = true;
                        newConnector.Align(newConnectorStart.transform.position, hoveredThisFrame.transform.position, true);
                    }
                    else if (hoveredThisFrame.wPrev == null)
                    {
                        hoveredThisFrame.wPrev = newConnectorStart;
                        newConnectorStart.next = hoveredThisFrame;
                        newConnectorStart.nextConnector = newConnector;
                        newConnectorStart.blackLink = false;
                        newConnector.Align(newConnectorStart.transform.position, hoveredThisFrame.transform.position, false);
                    }
                    else
                    {
                        Destroy(newConnector.gameObject);
                    }
                }
                else
                {
                    Destroy(newConnector.gameObject);
                }

                newConnectorStart = null;
                newConnector = null;
            }
            else
            {
                newConnector.Align(newConnectorStart.transform.position, mousePos, true);
            }
        }
        else if (noKeyHeld && rightClickPressed && hoveredThisFrame != null)
        {
            foreach (var node in selectedNodes)
            {
                node.SetHighlighted(false);
            }
            selectedNodes.Clear();

            newConnectorStart = hoveredThisFrame;
            if (newConnectorStart.next != null)
            {
                if (newConnectorStart.blackLink) newConnectorStart.next.bPrev = null;
                else newConnectorStart.next.wPrev = null;
                newConnectorStart.next = null;
                Destroy(newConnectorStart.nextConnector.gameObject);
                newConnectorStart.nextConnector = null;
            }
            newConnector = Instantiate(connectorPrefab).GetComponent<Connector>();
            newConnector.Align(newConnectorStart.transform.position, mousePos, true);
        }
        else if (shiftHeld && rightClickPressed && hoveredThisFrame != null)
        {
            Node swapper;
            if (selectedNodes.Contains(hoveredThisFrame))
            {
                foreach (var node in selectedNodes)
                {
                    if (node.bPrev != null)
                    {
                        node.bPrev.blackLink = false;
                        node.bPrev.nextConnector.colourSpriteRenderer.color = node.bPrev.nextConnector.innerWhite;
                    }
                    if (node.wPrev != null)
                    {
                        node.wPrev.blackLink = true;
                        node.wPrev.nextConnector.colourSpriteRenderer.color = node.wPrev.nextConnector.innerBlack;
                    }
                    swapper = node.bPrev;
                    node.bPrev = node.wPrev;
                    node.wPrev = swapper;
                }
            }
            else
            {
                if (hoveredThisFrame.bPrev != null)
                {
                    hoveredThisFrame.bPrev.blackLink = false;
                    hoveredThisFrame.bPrev.nextConnector.colourSpriteRenderer.color = hoveredThisFrame.bPrev.nextConnector.innerWhite;
                }
                if (hoveredThisFrame.wPrev != null)
                {
                    hoveredThisFrame.wPrev.blackLink = true;
                    hoveredThisFrame.wPrev.nextConnector.colourSpriteRenderer.color = hoveredThisFrame.wPrev.nextConnector.innerBlack;
                }
                swapper = hoveredThisFrame.bPrev;
                hoveredThisFrame.bPrev = hoveredThisFrame.wPrev;
                hoveredThisFrame.wPrev = swapper;
            }
        }
        else if (rightClickHeld && hoveredThisFrame == null && !mouseOverUI)
        {
            Camera.main.transform.position += Camera.main.ScreenToWorldPoint(prevMousePos) - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPos = transform.position;
        }

        transform.position = newPos;
        var scale = Camera.main.orthographicSize / 5;
        transform.localScale = new Vector3(scale, scale, 1);

        if (reducer != null && !leftClickHeld)
        {
            if (!mouseOverUI)
            {
                solution.reducers[solution.currentReducerIdx].AddNode(reducer, transform.position, this, nodeSortingOrderCount);
                nodeSortingOrderCount++;
            }

            reducer = null;
        }

        if (reducer == null) spriteRenderer.sprite = blank;
        else spriteRenderer.sprite = reducer.sprite;

        mouseOverUI = false;
        highestNodeSortingOrderThisFrame = -1;
        hoveredThisFrame = null;
        prevMousePos = Input.mousePosition;
    }
}
