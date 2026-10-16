using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestCaseInputPickerPair : MonoBehaviour, IPointerClickHandler
{
    public TestCaseInputPickerPair childPair;
    public Transform contentsTransform;
    public RectTransformVerticalSizeFitter scrollViewComponent;
    public RectTransformVerticalPositionMatcher arrowComponent;
    public TestCasesList tcl;

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        tcl.SetLeftmostInputPickerPair(this);
    }

    // Update is called once per frame
    void Update()
    {
        scrollViewComponent.UpdateConstraints();
        arrowComponent.UpdateConstraints();
    }

    public void InitialiseTracking(RectTransform target)
    {
        scrollViewComponent.transformToMatchPosition = target;
        arrowComponent.transformToMatchPosition = target;
    }
}
