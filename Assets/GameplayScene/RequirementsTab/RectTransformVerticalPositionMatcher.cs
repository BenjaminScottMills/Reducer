using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RectTransformVerticalPositionMatcher : MonoBehaviour
{
    public RectTransform transformToUpdate;
    public RectTransform transformToMatchPosition;
    public float topBottomPadding;
    public Image selfVisual;

    // Update is called once per frame
    void Update()
    {
        transformToUpdate.position = new Vector3(transformToUpdate.position.x, transformToMatchPosition.position.y);
        transformToUpdate.ForceUpdateRectTransforms();

        // make sure not above and below top / bottom of screen.
        Vector3[] cornersArray = new Vector3[4];
        transformToUpdate.GetWorldCorners(cornersArray);
        float transformTop = cornersArray[2].y;
        float transformBottom = cornersArray[0].y;

        float screenTop = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height)).y - topBottomPadding;
        float screenBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0)).y + topBottomPadding;

        if (transformTop > screenTop || transformBottom < screenBottom)
        {
            selfVisual.enabled = false;
        }
        else
        {
            selfVisual.enabled = true;
        }
    }
}
