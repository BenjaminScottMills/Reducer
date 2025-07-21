using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Connector : MonoBehaviour
{
    public Color innerWhite;
    public Color innerBlack;
    public Transform linkVisuals;
    public SpriteRenderer colourSpriteRenderer;
    public SortingGroup sortingGroup;
    public GameObject breakpointDisplay;

    public void Align(Vector3 start, Vector3 end, bool black)
    {
        transform.position = new Vector3((start.x + end.x) / 2, (start.y + end.y) / 2);
        float distance = (float)Math.Sqrt(((end.y - start.y) * (end.y - start.y)) + ((end.x - start.x) * (end.x - start.x)));
        if (distance < 0.000001f) distance = 0.000001f;
        Debug.Log(end.x > start.x ? new Vector3(0, 0, (float)(Math.Asin((end.y - start.y) / distance) * 180 / Math.PI)) :
                                    new Vector3(0, 0, 180 - (float)(Math.Asin((end.y - start.y) / distance) * 180 / Math.PI)));
        transform.eulerAngles = end.x > start.x ? new Vector3(0, 0, (float)(Math.Asin((end.y - start.y) / distance) * 180 / Math.PI)) :
                                                  new Vector3(0, 0, 180 - (float)(Math.Asin((end.y - start.y) / distance) * 180 / Math.PI));
        linkVisuals.localScale = new Vector3(distance, 1);
        colourSpriteRenderer.color = black ? innerBlack : innerWhite;
    }
}
