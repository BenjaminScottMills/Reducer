using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class RemoveSolutionManager : MonoBehaviour
{
    public GameObject initialButton;
    public GameObject confirm;
    public GameObject deny;
    public SolutionButton sb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void initialClicked()
    {
        initialButton.SetActive(false);
        confirm.SetActive(true);
        deny.SetActive(true);
    }

    public void denyClicked()
    {
        initialButton.SetActive(true);
        confirm.SetActive(false);
        deny.SetActive(false);
    }
    
    public void confirmClicked()
    {
        Directory.Delete(sb.solutionPath, true);
        sb.levelMenu.RemoveSolution(sb.gameObject);
    }
}
