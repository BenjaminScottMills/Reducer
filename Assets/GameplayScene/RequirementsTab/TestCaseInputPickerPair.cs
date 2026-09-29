using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCaseInputPickerPair : MonoBehaviour
{
    public TestCaseInputPickerPair parentPair;
    public Transform contentsTransform;
    public RectTransformVerticalSizeFitter scrollViewComponent;
    public RectTransformVerticalPositionMatcher arrowComponent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitialiseTracking(RectTransform target)
    {
        scrollViewComponent.transformToMatchPosition = target;
        arrowComponent.transformToMatchPosition = target;
    }
}
