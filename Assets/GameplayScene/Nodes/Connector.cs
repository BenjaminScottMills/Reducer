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

    public void Align(Vector3 start, Vector3 end, bool black)
    {
        transform.position = new Vector3((start.x + end.x) / 2, (start.y + end.y) / 2);                                                         // Dad, how does this code work?
        float distance = (float)Math.Sqrt(((end.y - start.y) * (end.y - start.y)) + ((end.x - start.x) * (end.x - start.x)));                       // Magic.
        if (distance < 0.000001f) distance = 0.000001f;                                                                                         // Didn't you say that's how Solution.LoadFromSerialised works?
        transform.eulerAngles = end.x > start.x ? new Vector3(0, 0, (float)(Math.Asin((end.y - start.y) / distance) * 180 / Math.PI)) :             // Right. They're both magic.
                                                  new Vector3(0, 0, 180 - (float)(Math.Asin((end.y - start.y) / distance) * 180 / Math.PI));    // You just don't *remember* how they work, I'll bet.
        linkVisuals.localScale = new Vector3(distance, 1);                                                                                          // Fine. Don't believe your own father, who's been around a lot longer than you.

        colourSpriteRenderer.color = black ? innerBlack : innerWhite;                                                                           // Look Mom, magic!
    }                                                                                                                                                   // That's not magic!
}
