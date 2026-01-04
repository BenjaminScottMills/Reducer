using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ReducerMenu : MonoBehaviour
{
    static Vector3 baseOffset = new Vector3(-4.2f, 0);
    public BoxCollider2D boxCollider;
    public CustomReducerList customReducerList;
    public FixedReducerList fixedReducerList;
    public InputField rName;
    public InputField description;
    public MouseNode mouseNode;
    public ReducerVisual reducerVisual;
    public Image resultBackground;
    public Image resultForeground;
    public Reducer reducer;
    public ReducerButton reducerButton;
    public Canvas canvas;
    void Update()
    {
        if (boxCollider.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
        {
            mouseNode.mouseOverUI = true;
            customReducerList.overReducerMenu = true;
            fixedReducerList.overReducerMenu = true;
        }
        else if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.mouseScrollDelta.y > 0.01f || Input.mouseScrollDelta.y < -0.01f)
        {
            gameObject.SetActive(false);
        }
    }

    public void Setup()
    {
        // Set all fields to default values
        var bottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0)).y;
        transform.localPosition = baseOffset;

        if (reducer != null)
        {
            reducerVisual.SetVisual(reducer);
            description.text = reducer.description;
            rName.text = reducer.rName;
        }
        else
        {
            reducerVisual.SetVisual(0, 1, 0);
            description.text = "";
            rName.text = "";
        }

        resultForeground.sprite = reducerVisual.foreground.sprite;
        resultForeground.color = reducerVisual.foreground.color;
        resultBackground.color = reducerVisual.background.color;

        if (transform.position.y - (Camera.main.orthographicSize / 2) < bottom)
        {
            transform.position = new Vector3(transform.position.x, bottom + (Camera.main.orthographicSize / 2));
        }
    }

    public void UpdateReducer(string rName, string desc)
    {
        reducer.rName = rName;
        reducer.description = desc;
        if (reducer.child != null) reducer.child.rName = rName + " - Child";

        reducer.foregroundColour = reducerVisual.foregroundColour;
        reducer.backgroundColour = reducerVisual.backgroundColour;
        reducer.foregroundSprite = reducerVisual.foregroundSprite;

        foreach (var solReducer in reducer.solution.reducers)
        {
            foreach (var node in solReducer.nodes)
            {
                if (node.reducer == reducer)
                {
                    node.reducerVisual.SetVisual(reducerVisual.backgroundColour, reducerVisual.foregroundColour, reducerVisual.foregroundSprite);
                }
            }

            if (solReducer.child != null)
            {
                foreach (var node in solReducer.child.nodes)
                {
                    if (node.reducer == reducer)
                    {
                        node.reducerVisual.SetVisual(reducerVisual.backgroundColour, reducerVisual.foregroundColour, reducerVisual.foregroundSprite);
                    }
                }
            }
        }

        reducerButton.UpdateVisuals();
    }

    public void DeleteReducer()
    {
        if (reducer.solution.currentReducer == reducer || reducer.solution.currentReducer == reducer.child)
        {
            int reducerIdx = reducer.solution.reducers.FindIndex(r => r == reducer);
            reducer.solution.currentReducer = reducer.solution.reducers[reducerIdx - 1];
            mouseNode.selectedNodes.Clear();
            foreach (var node in reducer.solution.currentReducer.nodes)
            {
                node.gameObject.SetActive(true);
                node.nextConnector?.gameObject.SetActive(true);
            }
        }

        reducer.solution.reducers.Remove(reducer);

        foreach (var node in reducer.nodes)
        {
            if (node?.nextConnector?.gameObject != null) Destroy(node.nextConnector.gameObject);
            Destroy(node.gameObject);
        }

        if (reducer.child != null)
        {
            foreach (var node in reducer.child.nodes)
            {
                if (node?.nextConnector?.gameObject != null) Destroy(node.nextConnector.gameObject);
                Destroy(node.gameObject);
            }
        }

        foreach (var solReducer in reducer.solution.reducers)
        {
            foreach (var node in solReducer.nodes)
            {
                if (node.reducer == reducer)
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

            solReducer.nodes = solReducer.nodes.Where(n => n.reducer != reducer).ToList();

            if (solReducer.child != null)
            {
                foreach (var node in solReducer.child.nodes)
                {
                    if (node.reducer == reducer)
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

                solReducer.child.nodes = solReducer.child.nodes.Where(n => n.reducer != reducer).ToList();
            }
        }

        mouseNode.selectedNodes = mouseNode.selectedNodes.Where(n => n.reducer != reducer).ToHashSet();

        int buttonIdx = customReducerList.customButtons.FindIndex(b => b == reducerButton);
        for (int i = buttonIdx; i < customReducerList.customButtons.Count(); i++)
        {
            customReducerList.customButtons[i].transform.localPosition += new Vector3(0, 1);

            if (mouseNode.solution.localReducersUnlocked)
            {
                customReducerList.customButtons[i].childButton.transform.localPosition += new Vector3(0, 1);
            }
        }
        customReducerList.newReducerButton.transform.localPosition += new Vector3(0, 1);
        customReducerList.customButtons.RemoveAt(buttonIdx);

        customReducerList.ButtonRemoveUpdate();

        Destroy(reducerButton.gameObject);
        Destroy(reducer.gameObject);
        if (reducer.child != null)
        {
            Destroy(reducer.child.gameObject);
        }
        if (reducerButton.childButton != null)
        {
            Destroy(reducerButton.childButton.gameObject);
        }
    }
}
