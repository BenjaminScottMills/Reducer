using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunTestButton : MonoBehaviour
{
    public CircleCollider2D circleCollider;
    public TestScreen testScreen;

    // Update is called once per frame
    void Update()
    {
        if (circleCollider.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)) && Input.GetMouseButtonDown(0))
        {
            testScreen.RunTest();
        }
    }
}
