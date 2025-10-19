using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject chapterMenu;
    public GameObject levelMenu;
    public MainTitle mainTitle;
    // Start is called before the first frame update
    void Start()
    {
        chapterMenu = null;
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void SetChapter(GameObject newChapterMenu)
    {
        chapterMenu?.SetActive(false);
        if (levelMenu != null) Destroy(levelMenu);
        chapterMenu = newChapterMenu;
        chapterMenu?.SetActive(true);

        mainTitle.SetShow(chapterMenu == null);
    }
}
