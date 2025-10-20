using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    public ChapterMenu chapterMenu;
    public string levelPath;
    public Text levelNameZone;
    public Vector3 firstSolutionLocation;
    public Vector3 solutionListOffset;
    public GameObject solutionButtonPrefab;
    public RectTransform scrollViewContent;
    public GameObject addSolutionButton;

    // Start is called before the first frame update
    void Start()
    {
        levelNameZone.text = Path.GetFileName(levelPath).Substring(2);
        string[] solutions = Directory.GetDirectories(Path.Combine(levelPath, "solutions"));
        Array.Sort(solutions);

        Vector3 newButtonPos = firstSolutionLocation;

        scrollViewContent.sizeDelta = new Vector2(scrollViewContent.sizeDelta.x, scrollViewContent.sizeDelta.y + Math.Abs(solutionListOffset.y * solutions.Length)); // 1 extra to account for add solution button

        foreach (string solution in solutions)
        {
            SolutionButton sb = Instantiate(solutionButtonPrefab, Vector3.zero, Quaternion.identity, scrollViewContent).GetComponent<SolutionButton>();
            sb.transform.localPosition = newButtonPos;
            sb.SetSolution(solution, Path.GetFileName(solution).Substring(2)); // First 2 characters are for ordering, ie "04".
            sb.levelMenu = this;
            newButtonPos += solutionListOffset;
        }

        addSolutionButton.transform.localPosition = newButtonPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
