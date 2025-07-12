using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewReducerButton : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 2)).y)
        {
            spriteRenderer.enabled = true;
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }
}
