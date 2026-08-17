using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImportMenuNodeDisplay : MonoBehaviour
{
    public GameObject connectorPrefab; // UIReducerConnector
    public GameObject nodeButtonPrefab; // ImportMenuNodeButton
    public GameObject contentsHolderPrefab;
    public GenericButton backgroundButton;
    public CallStackDisplay callStackDisplay;
    public Transform connectorLayerTransform;
    public TooltipText tooltipText;
    public RectTransform backgroundTransform;
    GameObject connectorLayerContentsHolder;
    public Transform reducerVisualLayerTransform;
    GameObject reducerVisualLayerContentsHolder;
    public RectTransform rectTransform;
    public Stack<Reducer> reducerStack;
    Vector3 prevMousePos;
    // Start is called before the first frame update
    void Start()
    {
        callStackDisplay.multiPopHandler = new ImportMenuMultiPopHandler{nodeDisplay = this};
        backgroundButton.invoker = new PopReducerInvoker{nodeDisplay = this};
        backgroundButton.restrictToLeftClicks = true;
    }

    class PopReducerInvoker : GenericButton.MethodInvoker
    {
        public ImportMenuNodeDisplay nodeDisplay;
        public override void InvokeMethod()
        {
            nodeDisplay.TryPopReducer();
        }
    }

    class PushReducerInvoker : GenericButton.MethodInvoker
    {
        public ImportMenuNodeDisplay nodeDisplay;
        public Reducer reducer;
        public override void InvokeMethod()
        {
            if (reducer.Selectable())
            {
                nodeDisplay.PushReducer(reducer);
            }
        }
    }

    class ImportMenuMultiPopHandler : CallStackDisplay.MultiPopHandler
    {
        public ImportMenuNodeDisplay nodeDisplay;
        
        public override void MultiPop(int n)
        {
            for (int i = 0; i < n; i ++)
            {
                nodeDisplay.reducerStack.Pop();
            }
            nodeDisplay.DisplayReducer(nodeDisplay.reducerStack.Peek());
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bool mouseWithinBounds = MouseInsideRectTransform(backgroundTransform);

        if (mouseWithinBounds && Input.GetMouseButton(1))
        {
            Vector3 mouseOffset = prevMousePos == Vector3.zero ? Vector3.zero : mousePos - prevMousePos;
            transform.position += mouseOffset;
        }
        
        prevMousePos = mousePos;
    }

    // assumes that rectTransform is unrotated.
    public static bool MouseInsideRectTransform(RectTransform rt)
    {
        Vector3[] cornersArray = new Vector3[4]; // 0 = bottom left, 2 = top right
        rt.GetWorldCorners(cornersArray);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return  cornersArray[0].x <= mousePos.x && mousePos.x <= cornersArray[2].x &&
                cornersArray[0].y <= mousePos.y && mousePos.y <= cornersArray[2].y;
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
            UINodeButton nodeButton = Instantiate(nodeButtonPrefab, Vector3.zero, Quaternion.identity, reducerVisualLayerContentsHolder.transform).GetComponent<UINodeButton>();
            nodeButton.enableHighlight = true;
            nodeButton.useRawName = false;
            nodeButton.transform.localPosition = node.transform.position;
            nodeButton.restrictToLeftClicks = true;
            nodeButton.tooltipText = tooltipText;

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
            
            nodeButton.invoker = new PushReducerInvoker{nodeDisplay = this, reducer = nodeButton.reducer};

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

        rectTransform.anchoredPosition = Vector2.zero;
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
        callStackDisplay.PushButtonToStack(reducer);
    }

    public void TryPopReducer()
    {
        if (reducerStack == null) reducerStack = new();
        if (reducerStack.Count <= 1) return;
        reducerStack.Pop();
        DisplayReducer(reducerStack.Peek());
        callStackDisplay.PopButton();
    }
}
