using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCaseInputPickerPair : MonoBehaviour
{
    public TestCaseInputPickerPair parentPair;
    public Transform contentsTransform;
    public RectTransformVerticalSizeFitter scrollViewComponent;
    public RectTransformVerticalPositionMatcher arrowComponent;


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
