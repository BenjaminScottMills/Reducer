using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImportMenuNodeDisplay : MonoBehaviour
{
    public GameObject connectorPrefab; // UIReducerConnector
    public GameObject nodeButtonPrefab; // GenericButton
    public GameObject reducerVisualPrefab; // UIReducerVisual
    public GameObject contentsHolderPrefab;
    public Transform connectorLayerTransform;
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

    class NodeButtonInvoker : GenericButton.MethodInvoker
    {
        public ImportMenuNodeDisplay importMenuNodeDisplay;
        
        public Reducer reducer;
        public override void InvokeMethod()
        {
            importMenuNodeDisplay.PushReducer(reducer);
        }
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
            NodeButtonInvoker invoker = new NodeButtonInvoker{importMenuNodeDisplay = this};
            GenericButton nodeButton = Instantiate(nodeButtonPrefab, Vector3.zero, Quaternion.identity, connectorLayerContentsHolder.transform).GetComponent<GenericButton>();
            UIReducerVisual reducerVisual = Instantiate(reducerVisualPrefab, Vector3.zero, Quaternion.identity, reducerVisualLayerContentsHolder.transform).GetComponent<UIReducerVisual>();
            nodeButton.transform.localPosition = node.transform.position;
            reducerVisual.transform.localPosition = node.transform.position;
            nodeButton.transform.localScale = new Vector3(1 / 35f, 1 / 35f, 1);
            reducerVisual.transform.localScale = new Vector3(1 / 35f, 1 / 35f, 1);
            nodeButton.invoker = invoker;

            if (node.reducer.id == (int)Reducer.SpecialReducers.local)
            {
                if (reducer.isChild)
                    {
                        reducerVisual.SetVisual(reducer);
                        invoker.reducer = reducer;
                    }
                    else
                    {
                        reducerVisual.SetVisual(reducer.child);
                        invoker.reducer = reducer.child;
                    }
            }
            else
            {
                reducerVisual.SetVisual(node.reducer);
                invoker.reducer = node.reducer;
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
