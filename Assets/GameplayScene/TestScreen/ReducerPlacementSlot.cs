using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReducerPlacementSlot : MonoBehaviour
{
    public Reducer reducer;
    public GameObject placeholder;
    public ReducerVisual reducerVisual;
    public CircleCollider2D boxCollider;

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && boxCollider.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
        {
            reducer = null;
            reducerVisual.gameObject.SetActive(false);
            placeholder.SetActive(true);
        }
    }

    public void CheckPlaced(Reducer r)
    {
        if (boxCollider.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
        {
            reducer = r;
            reducerVisual.SetVisual(r);
            reducerVisual.gameObject.SetActive(true);
            placeholder.SetActive(false);
        }
    }
}
