using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestScreen : MonoBehaviour
{
    [HideInInspector]
    public static Vector3 cameraDefaultPos = new Vector3(0, 0, -10);
    public GameObject reducerNodeHolder;
    public GameObject displayNodePrefab;
    public GameObject reducerNodeHolderPrefab;
    public MouseNode mouseNode;
    public Stack<Reducer.ExecuteReducer> eReducerStack = new();

    void OnEnable()
    {
        ClearOutput();
    }

    public void ShowOutput(Reducer.ExecuteReducer outputReducer)
    {
        Debug.Log(outputReducer.selfRed.rName);
        eReducerStack.Clear();
        eReducerStack.Push(outputReducer);

        DisplayReducer(outputReducer);
    }

    public void ClearOutput()
    {
        eReducerStack.Clear();
        if (reducerNodeHolder != null) Destroy(reducerNodeHolder);
    }

    public void ExitReducer()
    {
        if (eReducerStack.ToArray().Length > 1)
        {
            eReducerStack.Pop();
            DisplayReducer(eReducerStack.Peek());
        }
    }

    public void DisplayReducer(Reducer.ExecuteReducer outputReducer)
    {
        if (reducerNodeHolder != null)
        {
            Destroy(reducerNodeHolder);
        }

        reducerNodeHolder = Instantiate(reducerNodeHolderPrefab, transform);

        if (!outputReducer.Selectable())
        {
            var dispNode = Instantiate(displayNodePrefab, Vector3.zero, Quaternion.identity, reducerNodeHolder.transform).GetComponent<TestScreenDisplayNode>();
            dispNode.rVisual.SetVisual(outputReducer.selfRed);
            dispNode.eReducer = outputReducer;
            dispNode.testScreen = this;
            dispNode.transform.localScale *= 5;
        }

        foreach (var node in outputReducer.selfRed.nodes)
        {
            var dispNode = Instantiate(displayNodePrefab, node.transform.position, Quaternion.identity, reducerNodeHolder.transform).GetComponent<TestScreenDisplayNode>();
            dispNode.testScreen = this;
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

        reducerNodeHolder.transform.position = new Vector3(0, -1.5f);
        Camera.main.transform.position = cameraDefaultPos;
    }
}
