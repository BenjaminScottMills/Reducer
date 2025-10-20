using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChapterSelectButton : MonoBehaviour, IPointerClickHandler
{
    public MainMenuController controller;
    public ChapterMenu chapterMenu;
    public ChapterSelectButton previousChapterButton;
    public bool unlocked;
    public bool selected = false;
    public Image unselectedImage;
    public Image selectedImage;
    public GameObject lockedImage;

    void Start()
    {
        unlocked = previousChapterButton == null || previousChapterButton.chapterMenu.completed;
        lockedImage.SetActive(!unlocked);
    }

    void Update()
    {
        selectedImage.enabled = selected;
        unselectedImage.enabled = !selected;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (!unlocked) return;

        if (selected)
        {
            selected = false;
            controller.SetChapter(null);
        }
        else
        {
            selected = true;
            controller.SetChapter(chapterMenu.gameObject);
        }
    }
}
