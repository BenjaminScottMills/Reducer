using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewReducerButton : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public AddReducerMenu addReducerMenu;
    public Collider2D colliderd2d;

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 2)).y)
        {
            spriteRenderer.enabled = true;
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (colliderd2d.OverlapPoint(mousePos))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    addReducerMenu.gameObject.SetActive(true);
                    addReducerMenu.Setup();
                }
            }
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }
}
