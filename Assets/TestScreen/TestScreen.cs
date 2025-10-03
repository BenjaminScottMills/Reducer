using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScreen : MonoBehaviour
{
    public GameObject reducerNodeHolder;
    public GameObject displayNodePrefab;
    public MouseNode mouseNode;
    Stack<Reducer.ExecuteReducer> eReducerStack = new();

    public void ShowOutput(Reducer.ExecuteReducer outputReducer)
    {
        Debug.Log(outputReducer.selfRed.rName);
        eReducerStack.Clear();
        eReducerStack.Push(outputReducer);

        DisplayReducer(outputReducer);
    }

    private void DisplayReducer(Reducer.ExecuteReducer outputReducer)
    {
        if (reducerNodeHolder != null)
        {
            Destroy(reducerNodeHolder);
        }

        reducerNodeHolder = Instantiate(new GameObject("OutputReducerNodeHolder"), transform);

        if (outputReducer.selfRed is Primitive || outputReducer.selfRed is Combine)
        {
            var dispNode = Instantiate(displayNodePrefab, Vector3.zero, Quaternion.identity, reducerNodeHolder.transform).GetComponent<TestScreenDisplayNode>();
            dispNode.rVisual.SetVisual(outputReducer.selfRed);
            dispNode.eReducer = outputReducer;
            dispNode.transform.localScale *= 5;
        }

        foreach (var node in outputReducer.selfRed.nodes)
        {
            var dispNode = Instantiate(displayNodePrefab, node.transform.position, Quaternion.identity, reducerNodeHolder.transform).GetComponent<TestScreenDisplayNode>();
            dispNode.sortingGroup.sortingOrder = node.sortingGroup.sortingOrder;

            switch (node.reducer.id)
            {
                case (int)Reducer.SpecialReducers.local:
                    if (outputReducer.selfRed.isChild)
                    {
                        dispNode.rVisual.SetVisual(outputReducer.selfRed);
                        dispNode.eReducer = outputReducer;
                    }
                    else
                    {
                        dispNode.rVisual.SetVisual(outputReducer.selfRed.child);
                        dispNode.eReducer = new Reducer.ExecuteReducer(outputReducer.selfRed.child);
                    }
                    break;
                case (int)Reducer.SpecialReducers.outerBlack:
                    dispNode.rVisual.SetVisual(outputReducer.parentBlackIn.selfRed);
                    dispNode.eReducer = outputReducer.parentBlackIn;
                    break;
                case (int)Reducer.SpecialReducers.outerWhite:
                    dispNode.rVisual.SetVisual(outputReducer.parentWhiteIn.selfRed);
                    dispNode.eReducer = outputReducer.parentWhiteIn;
                    break;
                default:
                    dispNode.rVisual.SetVisual(node.reducer);
                    dispNode.eReducer = new Reducer.ExecuteReducer(node.reducer);
                    break;
            }

            if (node.nextConnector != null)
            {
                var newConnector = Instantiate(mouseNode.connectorPrefab, node.nextConnector.transform.position, node.nextConnector.transform.rotation);
                newConnector.transform.SetParent(reducerNodeHolder.transform, true);
                var connectorScript = newConnector.GetComponent<Connector>();
                connectorScript.colourSpriteRenderer.color = node.nextConnector.colourSpriteRenderer.color;
                connectorScript.linkVisuals.localScale = node.nextConnector.linkVisuals.localScale;
            }
        }
    }
}
