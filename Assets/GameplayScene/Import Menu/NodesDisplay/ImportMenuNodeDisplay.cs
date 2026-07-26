using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImportMenuNodeDisplay : MonoBehaviour
{
    public GameObject connectorPrefab; // UIReducerConnector
    public GameObject nodeButtonPrefab; // ImportMenuNodeButton
    public GameObject contentsHolderPrefab;
    public Transform connectorLayerTransform;
    public TooltipText tooltipText;
    GameObject connectorLayerContentsHolder;
    public Transform reducerVisualLayerTransform;
    GameObject reducerVisualLayerContentsHolder;
    public Stack<Reducer> reducerStack;
    Vector3 baseScale;
    Vector3 basePosition;
    // Start is called before the first frame update
    void Start()
    {
        baseScale = transform.localScale;
        basePosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DisplayReducer(Reducer reducer)
    {
        if (connectorLayerContentsHolder != null)
        {
            Destroy(connectorLayerContentsHolder);
        }
        if (reducerVisualLayerContentsHolder != null)
        {
            Destroy(reducerVisualLayerContentsHolder);
        }

        connectorLayerContentsHolder = Instantiate(contentsHolderPrefab, connectorLayerTransform);
        reducerVisualLayerContentsHolder = Instantiate(contentsHolderPrefab, reducerVisualLayerTransform);

        List<Node> sortedNodes = new(reducer.nodes);
        sortedNodes.Sort((a, b) => a.sortingGroup.sortingOrder.CompareTo(b.sortingGroup.sortingOrder));
        foreach (var node in reducer.nodes)
        {
            ImportMenuNodeButton nodeButton = Instantiate(nodeButtonPrefab, Vector3.zero, Quaternion.identity, reducerVisualLayerContentsHolder.transform).GetComponent<ImportMenuNodeButton>();
            nodeButton.transform.localPosition = node.transform.position;
            nodeButton.importMenuNodeDisplay = this;

            if (node.reducer.id == (int)Reducer.SpecialReducers.local)
            {
                if (reducer.isChild)
                    {
                        nodeButton.reducerVisual.SetVisual(reducer);
                        nodeButton.reducer = reducer;
                    }
                    else
                    {
                        nodeButton.reducerVisual.SetVisual(reducer.child);
                        nodeButton.reducer = reducer.child;
                    }
            }
            else
            {
                nodeButton.reducerVisual.SetVisual(node.reducer);
                nodeButton.reducer = node.reducer;
            }
            

            if (node.nextConnector != null)
            {
                var newConnector = Instantiate(connectorPrefab, Vector3.zero, node.nextConnector.transform.rotation);
                newConnector.transform.SetParent(connectorLayerContentsHolder.transform);
                newConnector.transform.localPosition = node.nextConnector.transform.position;
                var connectorScript = newConnector.GetComponent<UIReducerConnector>();
                connectorScript.colourImage.color = node.nextConnector.colourSpriteRenderer.color;
                connectorScript.linkVisuals.localScale = node.nextConnector.linkVisuals.localScale;
            }
        }

        // Handle resetting position and scale here.
        // transform.localScale = baseScale;
        // transform.localPosition = basePosition;
    }

    public void ResetToReducer(Reducer reducer)
    {
        if (reducerStack == null) reducerStack = new();
        reducerStack.Clear();
        PushReducer(reducer);
    }

    public void PushReducer(Reducer reducer)
    {
        if (reducerStack == null) reducerStack = new();
        reducerStack.Push(reducer);
        DisplayReducer(reducer);
    }

    public void TryPopReducer()
    {
        if (reducerStack == null) reducerStack = new();
        if (reducerStack.Count <= 1) return;
        reducerStack.Pop();
        DisplayReducer(reducerStack.Peek());
    }
}
