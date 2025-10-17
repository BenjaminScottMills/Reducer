using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopMenu : MonoBehaviour
{
    private Vector3 offset = new Vector3(0, 0, 10);
    public BoxCollider2D boxCollider;
    public MouseNode mouseNode;
    public char selectedScreen;
    public GameObject editorScreen;
    public GameObject testScreen;
    public GameObject rightSidebar;
    public CustomReducerList customReducerList;
    public FixedReducerList fixedReducerList;

    private Vector3 editorScreenPosition;
    private float editorScreenCameraSize;
    private Vector3 testScreenPosition;
    private float testScreenCameraSize;

    void Start()
    {
        editorScreenPosition = Camera.main.transform.position;
        testScreenPosition = Camera.main.transform.position;
        editorScreenCameraSize = Camera.main.orthographicSize;
        testScreenCameraSize = Camera.main.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height)) + offset;
        var scale = Camera.main.orthographicSize / 5;
        transform.localScale = new Vector3(scale, scale, 1);

        if (boxCollider.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition))) mouseNode.mouseOverUI = true;
    }

    public void UpdateSelectedScreen(char newScreen)
    {
        if (newScreen == selectedScreen) return;

        editorScreen.SetActive(newScreen == 'E');
        testScreen.SetActive(newScreen == 'T');
        rightSidebar.SetActive(newScreen != 'R');

        if (newScreen == 'T')
        {
            editorScreenPosition = Camera.main.transform.position;
            editorScreenCameraSize = Camera.main.orthographicSize;
            Camera.main.transform.position = testScreenPosition;
            Camera.main.orthographicSize = testScreenCameraSize;
            customReducerList.ActivateTestMode();
            fixedReducerList.ActivateTestMode();
        }
        else if (selectedScreen == 'T')
        {
            testScreenPosition = Camera.main.transform.position;
            testScreenCameraSize = Camera.main.orthographicSize;
            Camera.main.transform.position = editorScreenPosition;
            Camera.main.orthographicSize = editorScreenCameraSize;
            customReducerList.DeactivateTestMode();
            fixedReducerList.DeactivateTestMode();
        }

        selectedScreen = newScreen;
    }
}
