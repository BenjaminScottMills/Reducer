using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class RemoveFolderManager : MonoBehaviour
{
    public FolderInitialiseDeleteButton initialButton;
    public FolderInitialiseDeleteButton confirm;
    public FolderInitialiseDeleteButton deny;
    public FolderButton fb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool AnyHovered()
    {
        return initialButton.Hovered() || confirm.Hovered() || deny.Hovered();
    }

    public void initialClicked()
    {
        initialButton.gameObject.SetActive(false);
        confirm.gameObject.SetActive(true);
        deny.gameObject.SetActive(true);
    }

    public void denyClicked()
    {
        initialButton.gameObject.SetActive(true);
        confirm.gameObject.SetActive(false);
        deny.gameObject.SetActive(false);
    }
    
    public void confirmClicked()
    {
        Debug.Log("Remove folder");
    }
}
