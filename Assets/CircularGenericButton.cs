using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularGenericButton : GenericButton, ICanvasRaycastFilter
{
    public RectTransform rectTransform;
    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        Vector3[] cornersArray = new Vector3[4];
        rectTransform.GetWorldCorners(cornersArray);

        return (cornersArray[2].x - cornersArray[1].x) / 2 >= Vector2.Distance(eventCamera.ScreenToWorldPoint(sp), transform.position);
    }
}
