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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetSolution(string path, string name)
    {
        solutionPath = path;
        text.text = name;

        completedCheckmark.enabled = JsonUtility.FromJson<ChapterMenu.LevelStatus>(File.ReadAllText(Path.Combine(path, "status.json"))).completed;
        // THIS CLASS NEEDS THE FOLLOWING: delete button (which spawns confirm / deny button). Probably pencil button that enters into editing the text box.
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        
    }
}
