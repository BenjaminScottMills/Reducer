using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SolutionButton : MonoBehaviour, IPointerClickHandler
{
    public InputField inputField;
    public string solutionPath;
    public LevelMenu levelMenu;
    public Image completedCheckmark;
    public int levelNumber;
    string rootDirectory;
    // Start is called before the first frame update
    void Start()
    {
        inputField.onEndEdit.AddListener((string newName) => {RenameSolution(newName);});
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetSolution(string path, string name, string number, string rootDir)
    {
        SetSolution(path, name, int.Parse(number), rootDir);
    }

    public void SetSolution(string path, string name, int number, string rootDir)
    {
        solutionPath = path;
        inputField.text = name;
        levelNumber = number;
        rootDirectory = rootDir;

        completedCheckmark.enabled = JsonUtility.FromJson<ChapterMenu.LevelStatus>(File.ReadAllText(Path.Combine(path, "status.json"))).completed;
    }

    public void DecrementName()
    {
        --levelNumber;
        string newPath = Path.Combine(rootDirectory, (levelNumber < 10 ? "0" : "") + levelNumber + inputField.text);
        Directory.Move(solutionPath, newPath);
        solutionPath = newPath;
    }

    void RenameSolution(string newName)
    {
        string newPath = Path.Combine(rootDirectory, (levelNumber < 10 ? "0" : "") + levelNumber + newName);
        
        try
        {
            Directory.Move(solutionPath, newPath);
            solutionPath = newPath;
        }
        catch (Exception) {}
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        
    }
}
