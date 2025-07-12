using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseNode : MonoBehaviour
{
    public Reducer reducer;
    public CustomReducerList sidebar1;
    public FixedReducerList sidebar2;
    public Solution solution;
    public Vector3 offset = Vector3.zero;
    public SpriteRenderer spriteRenderer;
    public Sprite blank;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;

        if (reducer != null && !Input.GetMouseButton(0))
        {
            if (!sidebar1.boxCollider.OverlapPoint(transform.position) && !sidebar2.boxCollider.OverlapPoint(transform.position))
            {
                solution.reducers[solution.currentReducerIdx].AddNode(reducer, transform.position);
            }

            reducer = null;
        }

        if (reducer == null) spriteRenderer.sprite = blank;
        else spriteRenderer.sprite = reducer.sprite;
    }
}
