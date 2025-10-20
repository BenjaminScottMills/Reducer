using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChapterSelectButton : MonoBehaviour, IPointerClickHandler
{
    public MainMenuController controller;
    public GameObject chapterMenu;
    public bool selected = false;
    public Image unselectedImage;
    public Image selectedImage;

    void Update()
    {
        selectedImage.enabled = selected;
        unselectedImage.enabled = !selected;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (selected)
        {
            selected = false;
            controller.SetChapter(null);
        }
        else
        {
            selected = true;
            controller.SetChapter(chapterMenu);
        }
    }
}
