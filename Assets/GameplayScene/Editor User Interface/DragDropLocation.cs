using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class DragDropLocation : MonoBehaviour
{
    public CustomReducerList customReducerList;
    public SidebarButton aboveButton;
    public FolderButton currentlyHoveredFolder;
    public GameObject visual;

    // Update is called once per frame
    void Update()
    {
        aboveButton = null; // requires that customReducerList.customButtons[0] is fixedAtTop
        currentlyHoveredFolder = null;
        if (customReducerList.mouseOverForDragDropLocation)
        {
            float yMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
            
            for (int i = 0; i < customReducerList.customButtons.Count; ++i)
            {
                if (customReducerList.customButtons[i] is FolderButton && (customReducerList.customButtons[i] as FolderButton).hoveredForDragDrop)
                {
                    currentlyHoveredFolder = customReducerList.customButtons[i] as FolderButton;
                    break;
                }

                if (customReducerList.customButtons[i].fixedAtTop)
                {
                    continue;
                }

                if (yMousePos >= customReducerList.customButtons[i].transform.position.y)
                {
                    aboveButton = customReducerList.customButtons[i - 1];
                    break;
                }
            }

            if (aboveButton == null && currentlyHoveredFolder == null)
            {
                aboveButton = customReducerList.customButtons.Last();
            }
        }
        
        if (aboveButton != null)
        {
            visual.SetActive(true);
            transform.localPosition = new Vector3(0, aboveButton.transform.localPosition.y - 0.5f);
        }
        else
        {
            visual.SetActive(false);
        }
    }
}
