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
    public HashSet<Node> selectedNodes;
    // Start is called before the first frame update
    void Start()
    {
        selectedNodes = new HashSet<Node>();
    }

    // Update is called once per frame
    void Update()
    {
        var newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;

        if (!currentlyDragging && Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift))
        {
            foreach (var node in selectedNodes)
            {
                node.highlighted = false;
                node.highlightSpriteRenderer.enabled = false;
            }

            selectedNodes = new HashSet<Node>();
        }

        if (Input.GetMouseButton(0) && currentlyDragging)
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

        transform.position = newPos;

        if (reducer != null && !Input.GetMouseButton(0))
        {
            if (!mouseOverUI)
            {
                solution.reducers[solution.currentReducerIdx].AddNode(reducer, transform.position, this);
            }

            reducer = null;
        }

        if (reducer == null) spriteRenderer.sprite = blank;
        else spriteRenderer.sprite = reducer.sprite;

        mouseOverUI = false;
    }
}
