using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainTitle : MonoBehaviour
{
    Vector3 basePosition;
    bool show;
    public Vector3 hiddenOffset;
    public Image image;
    const int speedMultiple = 1500;
    // Start is called before the first frame update
    void Start()
    {
        basePosition = transform.position;
        show = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (show)
        {
            float speed = Time.deltaTime * speedMultiple;

            if (basePosition.x > transform.position.x + speed)
            {
                transform.position += new Vector3(speed, 0);
            }
            else if (basePosition.x < transform.position.x - speed)
            {
                transform.position += new Vector3(-speed, 0);
            }
            else
            {
                transform.position = new Vector3(basePosition.x, transform.position.y);
            }

            if (basePosition.y > transform.position.y + speed)
            {
                transform.position += new Vector3(0, speed);
            }
            else if (basePosition.y < transform.position.y - speed)
            {
                transform.position += new Vector3(0, -speed);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, basePosition.y);
            }   
        }
    }
    
    public void SetShow(bool newShow)
    {
        if (newShow == show) return;
        show = newShow;

        if (show)
        {
            image.enabled = true;
        }
        else
        {
            image.enabled = false;
            transform.position = basePosition + hiddenOffset;
        }
    }
}
