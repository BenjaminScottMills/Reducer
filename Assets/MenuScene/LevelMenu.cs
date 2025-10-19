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
    public GameObject newSolutionButton;

    // Start is called before the first frame update
    void Start()
    {
        levelNameZone.text = Path.GetFileName(levelPath);
        string[] solutions = Directory.GetDirectories(Path.Combine(levelPath, "solutions"));
        Array.Sort(solutions);

        Vector3 newButtonPos = firstSolutionLocation;

        foreach (string solution in solutions)
        {
            SolutionButton sb = Instantiate(solutionButtonPrefab, newButtonPos, Quaternion.identity, transform).GetComponent<SolutionButton>();
            sb.SetSolution(solution, Path.GetFileName(solution).Substring(2)); // First 2 characters are for ordering, ie "04".
            sb.levelMenu = this;
            newButtonPos += solutionListOffset;
        }

        newSolutionButton.transform.position = newButtonPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
