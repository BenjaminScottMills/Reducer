using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour, IPointerClickHandler
{
    public Text text;
    public string levelPath;
    public bool unlocked;
    public bool completed;
    public ChapterMenu chapterMenu;
    public GameObject levelMenuPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetLevel(string path, string levelName)
    {
        text.text = levelName;
        levelPath = path;
    }
    
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (!unlocked) return;

        LevelMenu lm = Instantiate(levelMenuPrefab, Vector3.zero, Quaternion.identity, transform.parent.parent).GetComponent<LevelMenu>();
        chapterMenu.mainMenuController.levelMenu = lm.gameObject;
        lm.chapterMenu = chapterMenu;
        lm.levelPath = levelPath;
        chapterMenu.gameObject.SetActive(false);
    }
}
