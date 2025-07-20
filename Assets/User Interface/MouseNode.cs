using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseNode : MonoBehaviour
{
    public Reducer reducer;
    public Solution solution;
    public Vector3 offset = Vector3.zero;
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
    // Start is called before the first frame update
    void Start()
    {
        selectedNodes = new HashSet<Node>();
    }

    // Update is called once per frame
    void Update()
    {
        var newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        bool leftClickPressed = Input.GetMouseButtonDown(0);
        bool leftClickHovered = Input.GetMouseButton(0);
        bool rightClickPressed = Input.GetMouseButtonDown(1) && !leftClickHovered;
        bool rightClickHovered = Input.GetMouseButton(1) && !leftClickHovered;
        bool shiftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        bool ctrlHeld = !shiftHeld && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl));
        bool noKeyHeld = !shiftHeld && !ctrlHeld;

        if (hoveredThisFrame != null && leftClickPressed) hoveredThisFrame.HandleClick();

        if (!currentlyDragging && leftClickPressed && noKeyHeld)
        {
            foreach (var node in selectedNodes)
            {
                node.highlighted = false;
                node.highlightSpriteRenderer.enabled = false;
            }

            selectedNodes = new HashSet<Node>();
        }

        if (leftClickHovered && currentlyDragging)
        {
            var displacement = newPos - transform.position;
            foreach (var node in selectedNodes)
            {
                node.transform.position += displacement;
            }
            foreach (var node in selectedNodes)
            {
                node.RealignLinks();
            }
        }
        else currentlyDragging = false;

        if (newConnector != null)
        {
            if (!rightClickHovered)
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
                newConnector.Align(newConnectorStart.transform.position, newPos - offset, true);
            }
        }
        else if (noKeyHeld && rightClickPressed && hoveredThisFrame != null)
        {
            foreach (var node in selectedNodes)
            {
                node.highlighted = false;
                node.highlightSpriteRenderer.enabled = false;
            }
            selectedNodes = new HashSet<Node>();

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
            newConnector.Align(newConnectorStart.transform.position, newPos - offset, true);
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
                        node.wPrev.nextConnector.colourSpriteRenderer.color = node.bPrev.nextConnector.innerBlack;
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

        transform.position = newPos;

        if (reducer != null && !leftClickHovered)
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
    }
}
