using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SolutionButton : MonoBehaviour, IPointerClickHandler
{
    public Text text;
    public string solutionPath;
    public LevelMenu levelMenu;
    public Image completedCheckmark;
    public int levelNumber;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetSolution(string path, string name, string number)
    {
        SetSolution(path, name, int.Parse(number));
    }

    public void SetSolution(string path, string name, int number)
    {
        solutionPath = path;
        text.text = name;
        levelNumber = number;

        completedCheckmark.enabled = JsonUtility.FromJson<ChapterMenu.LevelStatus>(File.ReadAllText(Path.Combine(path, "status.json"))).completed;
        // THIS CLASS NEEDS THE FOLLOWING: Probably pencil button that enters into editing the text box.
    }

    public void DecrementName(string rootDir)
    {
        --levelNumber;
        string newPath = Path.Combine(rootDir, (levelNumber < 10 ? "0" : "") + levelNumber + text.text);
        Directory.Move(solutionPath, newPath);
        solutionPath = newPath;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        
    }
}
