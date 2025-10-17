using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TestScreenDisplayNode : MonoBehaviour
{
    public ReducerVisual rVisual;
    public Reducer.ExecuteReducer eReducer;
    public SortingGroup sortingGroup;
    public TestScreen testScreen;
    public SpriteRenderer highlight;

    void Update()
    {
        highlight.enabled = false;

        if (testScreen.mouseNode.mouseOverUI) return;

        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
        if (Vector3.Distance(mousePos, transform.position) < (Node.radius * transform.localScale.x) && !testScreen.mouseNode.mouseOverUI && sortingGroup.sortingOrder >= testScreen.mouseNode.highestNodeSortingOrderThisFrame)
        {
            testScreen.mouseNode.highestNodeSortingOrderThisFrame = sortingGroup.sortingOrder;
            testScreen.mouseNode.testHoveredThisFrame = this;
        }
    }

    public void EnterReducer()
    {
        if (eReducer.Selectable())
        {
            testScreen.eReducerStack.Push(eReducer);
            testScreen.DisplayReducer(eReducer);
        }
    }
}
