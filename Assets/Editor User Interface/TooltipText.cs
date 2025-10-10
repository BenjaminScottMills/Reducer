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
    Vector3 offset = new Vector3(0, 0, 10);
    Vector3 lastFramePosition;
    public GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        standStillCounter = 0;
        lastFramePosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        uiText.text = " " + text + " ";
        uiTextDuplicate.text = " " + text + " ";

        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        transform.localScale = new Vector3(Camera.main.orthographicSize / 5, Camera.main.orthographicSize / 5, 1);

        if (text != "" && Vector3.Distance(lastFramePosition, transform.position) < 0.01f)
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
        lastFramePosition = transform.position;

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
    }
}
