using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopMenu : MonoBehaviour
{
    private Vector3 offset = new Vector3(0, 0, 10);

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height)) + offset;
        var scale = Camera.main.orthographicSize / 5;
        transform.localScale = new Vector3(scale, scale, 1);
    }
}
