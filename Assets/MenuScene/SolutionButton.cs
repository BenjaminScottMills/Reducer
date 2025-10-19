using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SolutionButton : MonoBehaviour, IPointerClickHandler
{
    public LevelMenu levelMenu;
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
        // THIS CLASS NEEDS THE FOLLOWING: solution name, delete button (which spawns confirm / deny button), marker showing if solution completed the level or not. Probably pencil button that enters into editing the text box.
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        
    }
}
