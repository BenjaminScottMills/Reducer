using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ChapterMenu : MonoBehaviour
{
    public string chapterDirectory;
    public Vector3 firstLevelLocation;
    public Vector3 levelListOffset;
    public GameObject levelButtonPrefab;
    public RectTransform scrollViewContent;
    public MainMenuController mainMenuController;
    public bool completed;
    // Start is called before the first frame update
    void Start()
    {
        string location = Path.Combine(Application.persistentDataPath, "chapters", chapterDirectory);
        string[] levels = Directory.GetDirectories(location);
        Array.Sort(levels);

        Vector3 newButtonPos = firstLevelLocation;
        bool levelCompleted = true;

        scrollViewContent.sizeDelta = new Vector2(scrollViewContent.sizeDelta.x, scrollViewContent.sizeDelta.y + Math.Abs(levelListOffset.y * (levels.Length - 1)));

        foreach (string level in levels)
        {
            LevelButton lb = Instantiate(levelButtonPrefab, Vector3.zero, Quaternion.identity, scrollViewContent).GetComponent<LevelButton>();
            lb.transform.localPosition = newButtonPos;
            lb.SetLevel(level, Path.GetFileName(level).Substring(2)); // First 2 characters are for ordering (ie "02")
            lb.chapterMenu = this;
            lb.unlocked = levelCompleted;
            newButtonPos += levelListOffset;

            levelCompleted = JsonUtility.FromJson<LevelStatus>(File.ReadAllText(Path.Combine(level, "status.json"))).completed;

            lb.completed = levelCompleted;
        }

        completed = levelCompleted;

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public struct LevelStatus
    {
        public bool completed;
    }
}
