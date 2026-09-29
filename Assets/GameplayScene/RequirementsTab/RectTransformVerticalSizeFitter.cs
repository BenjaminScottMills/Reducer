using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectTransformVerticalSizeFitter : MonoBehaviour
{
    public RectTransform transformToUpdate;
    public RectTransform transformToMatchSize;
    public RectTransform transformToMatchPosition;
    public float maxSize;
    public float topBottomPadding;

    // Update is called once per frame
    void Update()
    {
        UpdateConstraints();
    }
    public void UpdateConstraints()
    {
        transformToUpdate.position = new Vector3(transformToUpdate.position.x, transformToMatchPosition.position.y);

        // update size
        transformToMatchSize.ForceUpdateRectTransforms();
        transformToUpdate.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Math.Min(transformToMatchSize.sizeDelta.y, maxSize));

        // make sure not above and below top / bottom of screen.
        transformToUpdate.ForceUpdateRectTransforms();
        Vector3[] cornersArray = new Vector3[4];
        transformToUpdate.GetWorldCorners(cornersArray);
        float transformTop = cornersArray[2].y;
        float transformBottom = cornersArray[0].y;

        float screenTop = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height)).y - topBottomPadding;
        float screenBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0)).y + topBottomPadding;

        if (transformTop > screenTop)
        {
            transformToUpdate.position += new Vector3(0, screenTop - transformTop);
        }
        else if (transformBottom < screenBottom)
        {
            transformToUpdate.position += new Vector3(0, screenBottom - transformBottom);
        }
    }
}
