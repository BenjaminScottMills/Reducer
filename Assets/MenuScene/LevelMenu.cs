using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.PackageManager;
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
    List<SolutionButton> solutionButtons = new List<SolutionButton>();
    float baseScrollViewHeight;

    // Start is called before the first frame update
    void Start()
    {
        baseScrollViewHeight = scrollViewContent.sizeDelta.y;

        levelNameZone.text = Path.GetFileName(levelPath).Substring(2);
        string[] solutions = Directory.GetDirectories(Path.Combine(levelPath, "solutions"));
        Array.Sort(solutions);

        foreach (string solution in solutions)
        {
            SolutionButton sb = Instantiate(solutionButtonPrefab, Vector3.zero, Quaternion.identity, scrollViewContent).GetComponent<SolutionButton>();
            sb.SetSolution(solution, Path.GetFileName(solution).Substring(2), Path.GetFileName(solution).Substring(0, 2)); // First 2 characters are for ordering, ie "04".
            sb.levelMenu = this;
            solutionButtons.Add(sb);
        }

        PositionSolutionButtons(true);
    }

    void PositionSolutionButtons(bool firstGo)
    {
        Vector3 newButtonPos = firstSolutionLocation;

        scrollViewContent.sizeDelta = new Vector2(scrollViewContent.sizeDelta.x, baseScrollViewHeight + Math.Abs(solutionListOffset.y * solutionButtons.Count)); // 1 extra to account for add solution button

        foreach (SolutionButton solutionButton in solutionButtons)
        {
            solutionButton.transform.localPosition = firstGo ? newButtonPos : new Vector3(solutionButton.transform.localPosition.x, newButtonPos.y);
            newButtonPos += solutionListOffset;
        }

        addSolutionButton.transform.localPosition = firstGo ? newButtonPos : new Vector3(addSolutionButton.transform.localPosition.x, newButtonPos.y);
    }

    public void RemoveSolution(SolutionButton sb)
    {
        Directory.Delete(sb.solutionPath, true);
        int sbLocation = solutionButtons.FindIndex((n) => n == sb);
        string solutionsPath = Path.Combine(levelPath, "solutions");

        for (int i = sbLocation + 1; i < solutionButtons.Count; ++i)
        {
            solutionButtons[i].DecrementName(solutionsPath);
        }

        solutionButtons.RemoveAt(sbLocation);
        Destroy(sb.gameObject);
        PositionSolutionButtons(false);
    }

    public void AddSolution()
    {
        int newNumber;
        if (solutionButtons.Count == 0) newNumber = 0;
        else newNumber = solutionButtons.Last().levelNumber + 1;

        string newDirectory = Path.Combine(levelPath, "solutions", (newNumber < 10 ? "0" : "") + newNumber + "New Solution");
        Directory.CreateDirectory(newDirectory);

        File.WriteAllText(Path.Combine(newDirectory, "status.json"), JsonUtility.ToJson(new ChapterMenu.LevelStatus{completed = false}));

        SolutionButton sb = Instantiate(solutionButtonPrefab, addSolutionButton.transform.position, Quaternion.identity, scrollViewContent).GetComponent<SolutionButton>();
        sb.SetSolution(newDirectory, "New Solution", newNumber); // First 2 characters are for ordering, ie "04".
        sb.levelMenu = this;
        solutionButtons.Add(sb);

        PositionSolutionButtons(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
