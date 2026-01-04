using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TooltipText : MonoBehaviour
{
    public RectTransform txtTransform;
    public RectTransform txtDuplicateTransform;
    public Text uiText;
    public Text uiTextDuplicate;
    public string text;
    float standStillCounter;
    const float standStillCounterMax = 0.75f;
    const int minMoveDistance = 1;
    Vector3 offset = new Vector3(0, 0, 10);
    Vector3 lastFrameMousePosition;
    public GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        standStillCounter = 0;
        lastFrameMousePosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        uiText.text = " " + text + " ";
        uiTextDuplicate.text = " " + text + " ";

        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        transform.localScale = new Vector3(Camera.main.orthographicSize / 5, Camera.main.orthographicSize / 5, 1);

        if (text != "" && !Input.GetMouseButton(0) && !Input.GetMouseButton(1) && Math.Abs(Input.mouseScrollDelta.y) < 0.05f && Vector3.Distance(lastFrameMousePosition, Input.mousePosition) < minMoveDistance)
        {
            standStillCounter += Time.deltaTime;
            if (standStillCounter > standStillCounterMax)
            {
                canvas.SetActive(true);
            }
        }
        else
        {
            standStillCounter = 0;
            canvas.SetActive(false);
        }
        lastFrameMousePosition = Input.mousePosition;

        if (transform.position.y > Camera.main.ScreenToWorldPoint(new Vector2(0, 0.95f * Screen.height)).y)
        {
            txtTransform.pivot = new Vector2(1, 1);
            txtDuplicateTransform.pivot = new Vector2(1, 1);
        }
        else
        {
            if (transform.position.x > Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2, 0)).x)
            {
                txtTransform.pivot = new Vector2(1, 0);
                txtDuplicateTransform.pivot = new Vector2(1, 0);
            }
            else
            {
                txtTransform.pivot = new Vector2(0, 0);
                txtDuplicateTransform.pivot = new Vector2(0, 0);
            }
        }

        text = "";
    }
}
