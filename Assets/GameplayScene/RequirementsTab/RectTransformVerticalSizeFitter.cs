using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RectTransformVerticalSizeFitter : MonoBehaviour
{
    public RectTransform transformToUpdate;
    public RectTransform transformToMatchSize;
    public RectTransform transformToMatchPosition;
    public float minSize;
    public VerticalLayoutGroup minSizePaddingAdjuster;
    RectOffset basePadding;
    bool basePaddingSet;
    public float maxSize;
    public float topBottomPadding;

    public static void CopyRectOffset(RectOffset baseRect, RectOffset copyRect)
    {
        copyRect.left = baseRect.left;
        copyRect.right = baseRect.right;
        copyRect.top = baseRect.top;
        copyRect.bottom = baseRect.bottom;
    }

    public void UpdateConstraints()
    {
        gameObject.SetActive(true);
        if (!basePaddingSet)
        {
            basePadding = new RectOffset();
            CopyRectOffset(minSizePaddingAdjuster.padding, basePadding);
            LayoutRebuilder.ForceRebuildLayoutImmediate(transformToUpdate);
        }
        else
        {
            CopyRectOffset(basePadding, minSizePaddingAdjuster.padding);
            LayoutRebuilder.ForceRebuildLayoutImmediate(transformToUpdate);
        }

        transformToUpdate.ForceUpdateRectTransforms();
        transformToUpdate.position = new Vector3(transformToUpdate.position.x, transformToMatchPosition.position.y);

        // update size
        transformToMatchSize.ForceUpdateRectTransforms();
        if (transformToMatchSize.sizeDelta.y < minSize)
        {
            int paddingToAdd = (int)((minSize - transformToMatchSize.sizeDelta.y) / 2);
            minSizePaddingAdjuster.padding.top += paddingToAdd;
            minSizePaddingAdjuster.padding.bottom += paddingToAdd;
            LayoutRebuilder.ForceRebuildLayoutImmediate(transformToMatchSize);
            transformToMatchSize.ForceUpdateRectTransforms();
        }
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

        if (!basePaddingSet)
        {
            basePaddingSet = true;
            UpdateConstraints();
        }
    }
}
