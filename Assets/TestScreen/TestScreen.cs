using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScreen : MonoBehaviour
{
    public ReducerPlacementSlot whiteIn;
    public ReducerPlacementSlot blackIn;
    public ReducerPlacementSlot functionReducer;
    public ReducerVisual outputNode;
    public Connector whiteInCon;
    public Connector blackInCon;
    public Connector outCon;
    // Start is called before the first frame update
    void Start()
    {
        whiteInCon.Align(whiteIn.transform.position, functionReducer.transform.position, false);
        blackInCon.Align(blackIn.transform.position, functionReducer.transform.position, true);
        outCon.Align(functionReducer.transform.position, outputNode.transform.position, true);
        outputNode.SetVisual();
    }

    // Update is called once per frame
    void Update()
    {
        var scale = Camera.main.orthographicSize / 5;
        transform.localScale = new Vector3(scale, scale, 1);
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height)) + new Vector3(0, -1.5f * scale, 10);
    }

    public void RunTest()
    {
        var outputReducer = functionReducer.reducer.Execute(blackIn.reducer, whiteIn.reducer);

        Debug.Log(outputReducer.selfRed.rName);
    }
}
