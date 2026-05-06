using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewFolderButton : MonoBehaviour
{
    public Solution solution;
    public Collider2D collider2d;
    public SpriteRenderer spriteRenderer;
    public TooltipText tooltipText;
    // Start is called before the first frame update
    void Start()
    {
        if (!solution.foldersUnlocked) Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 2)).y)
        {
            spriteRenderer.enabled = true;
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (collider2d.OverlapPoint(mousePos) && !solution.mouseNode.onImportScreen)
            {
                tooltipText.text = "Add Folder";
                if (Input.GetMouseButtonDown(0))
                {
                    solution.AddFolder();
                }
            }
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }
}
