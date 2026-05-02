using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDimmer : MonoBehaviour
{
    public SidebarButton dragButton;
    public GameObject internalDragDimmer;

    // Update is called once per frame
    void Update()
    {
        DragDropLocation.SetYPos(transform, dragButton.transform.position.y);
        internalDragDimmer.transform.position = transform.position;
    }
}
