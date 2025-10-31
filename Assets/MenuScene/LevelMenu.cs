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
    List<GameObject> solutionButtonsAndAdd = new List<GameObject>();
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
            sb.SetSolution(solution, Path.GetFileName(solution).Substring(2)); // First 2 characters are for ordering, ie "04".
            sb.levelMenu = this;
            solutionButtonsAndAdd.Add(sb.gameObject);
        }

        PositionSolutionButtons(true);
    }

    void PositionSolutionButtons(bool firstGo)
    {
        Vector3 newButtonPos = firstSolutionLocation;

        scrollViewContent.sizeDelta = new Vector2(scrollViewContent.sizeDelta.x, baseScrollViewHeight + Math.Abs(solutionListOffset.y * solutionButtonsAndAdd.Count)); // 1 extra to account for add solution button

        foreach (GameObject solutionButton in solutionButtonsAndAdd)
        {
            solutionButton.transform.localPosition = firstGo ? newButtonPos : new Vector3(solutionButton.transform.localPosition.x, newButtonPos.y);
            newButtonPos += solutionListOffset;
        }

        addSolutionButton.transform.localPosition = firstGo ? newButtonPos : new Vector3(addSolutionButton.transform.localPosition.x, newButtonPos.y);
    }

    public void RemoveSolution(GameObject go)
    {
        solutionButtonsAndAdd.Remove(go);
        Destroy(go);
        PositionSolutionButtons(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
