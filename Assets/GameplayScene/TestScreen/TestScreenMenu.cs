using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestScreenMenu : MonoBehaviour
{
    public ReducerPlacementSlot whiteIn;
    public ReducerPlacementSlot blackIn;
    public ReducerPlacementSlot functionReducer;
    public UINodeButton outputNode;
    public UIReducerConnector whiteInCon;
    public UIReducerConnector blackInCon;
    public UIReducerConnector outCon;
    public TestScreen testScreen;
    public MouseNode mouseNode;
    public UIPointerHoverDetector backgroundHoverDetector;

    // Start is called before the first frame update
    void Start()
    {
        whiteIn.invoker = new ReducerPlacementSlotInvoker{slot = whiteIn};
        blackIn.invoker = new ReducerPlacementSlotInvoker{slot = blackIn};
        functionReducer.invoker = new ReducerPlacementSlotInvoker{slot = functionReducer};

        AlignConnectors();

        outputNode.SetVisual();
        outputNode.invoker = new RunTestInvoker{menu = this};
    }

    class RunTestInvoker : GenericButton.MethodInvoker
    {
        public TestScreenMenu menu;

        public override void InvokeMethod()
        {
            menu.RunTest();
        }
    }

    class ReducerPlacementSlotInvoker : GenericButton.MethodInvoker
    {
        public ReducerPlacementSlot slot;
        public override void InvokeMethod()
        {
            if (pointerEventData.button == PointerEventData.InputButton.Left)
            {
                // open the menu downward.
            }
            else if (pointerEventData.button == PointerEventData.InputButton.Right)
            {
                slot.SetReducer(null);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        AlignConnectors();

        if (testScreen.callStackDisplay.GetIsPointerHovered() || backgroundHoverDetector.GetIsPointerHovered())
        {
            mouseNode.mouseOverUI = true;
        }
    }

    public void RunTest()
    {
        var outputReducer = functionReducer.reducer.Execute(blackIn.reducer, whiteIn.reducer);

        testScreen.ShowOutput(outputReducer);
    }

    void AlignConnectors()
    {
        whiteInCon.Align(whiteIn.transform.position, functionReducer.transform.position, false);
        blackInCon.Align(blackIn.transform.position, functionReducer.transform.position, true);
        outCon.Align(functionReducer.transform.position, outputNode.transform.position, true);
    }
}
