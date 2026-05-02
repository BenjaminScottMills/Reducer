using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class DragDropLocation : MonoBehaviour
{
    public CustomReducerList customReducerList;
    public int aboveButtonIdx;
    public FolderButton currentlyHoveredFolder;
    public GameObject visual;

    // Update is called once per frame
    void Update()
    {
        aboveButtonIdx = -1; // requires that customReducerList.customButtons[0] is fixedAtTop
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
                    aboveButtonIdx = i - 1;
                    break;
                }
            }

            if (aboveButtonIdx == -1 && currentlyHoveredFolder == null)
            {
                aboveButtonIdx = customReducerList.customButtons.Count - 1;
            }
        }
        
        if (aboveButtonIdx != -1)
        {
            visual.SetActive(true);
            transform.localPosition = new Vector3(0, customReducerList.customButtons[aboveButtonIdx].transform.localPosition.y - 0.5f);
        }
        else
        {
            visual.SetActive(false);
        }
    }

    public void HandleMovement(SidebarButton moveButton)
    {
        Solution solution = customReducerList.mouseNode.solution;
        List<ReducerOrFolder> contents = solution.currentFolder?.contents ?? solution.contents;
        Predicate<ReducerOrFolder> pred = moveButton is ReducerButton ? (ReducerOrFolder rof) => rof.r == (moveButton as ReducerButton).reducer : (ReducerOrFolder rof) => rof.f == (moveButton as FolderButton).rFolder;
        int idxInContents = contents.FindIndex(pred);

        if (currentlyHoveredFolder != null)
        {
            if (currentlyHoveredFolder.rFolder == null) // goes to solution.contents
            {
                Debug.Log("INSERT CODE HERE TO HANDLE CASE WHERE THERE ARE MULTIPLE FIXED AT TOP");
                solution.contents.Insert(1, contents[idxInContents]);
            }
            else
            {
                currentlyHoveredFolder.rFolder.contents.Insert(0, contents[idxInContents]);
            }
            contents.RemoveAt(idxInContents);
            customReducerList.RemoveButton(moveButton);
        }
        else if (aboveButtonIdx != -1)
        {
            int moveButtonIdx = customReducerList.customButtons.FindIndex(but => but == moveButton);
            
            
            if (aboveButtonIdx < moveButtonIdx)
            {
                while (aboveButtonIdx + 1 < moveButtonIdx)
                {
                    SwapButtons(customReducerList.customButtons, moveButtonIdx - 1, moveButtonIdx);
                    SwapRofs(contents, idxInContents, idxInContents - 1);
                    --idxInContents;
                    --moveButtonIdx;
                }
            }
            else
            {
                while (moveButtonIdx < aboveButtonIdx)
                {
                    SwapButtons(customReducerList.customButtons, moveButtonIdx + 1, moveButtonIdx);
                    SwapRofs(contents, idxInContents, idxInContents + 1);
                    ++idxInContents;
                    ++moveButtonIdx;
                }
            }
        }
    }

    void SwapButtons(List<SidebarButton> customButtons, int a, int b)
    {
        float tmpY = customButtons[a].transform.position.y;
        
        SetYPos(customButtons[a].transform, customButtons[b].transform.position.y);
        if ((customButtons[a] as ReducerButton)?.childButton != null)
        {
            SetYPos((customButtons[a] as ReducerButton).childButton.transform, customButtons[b].transform.position.y);
        }

        SetYPos(customButtons[b].transform, tmpY);
        if ((customButtons[b] as ReducerButton)?.childButton != null)
        {
            SetYPos((customButtons[b] as ReducerButton).childButton.transform, tmpY);
        }

        SidebarButton tmpBut = customButtons[a];
        customButtons[a] = customButtons[b];
        customButtons[b] = tmpBut;
    }

    void SwapRofs(List<ReducerOrFolder> contents, int a, int b)
    {
        ReducerOrFolder tmpRof = contents[a];
        contents[a] = contents[b];
        contents[b] = tmpRof;
    }

    static public void SetYPos(Transform trans, float newY)
    {
        trans.position = new Vector3(trans.position.x, newY);
    }
}
