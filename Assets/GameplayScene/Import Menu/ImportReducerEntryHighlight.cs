using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImportReducerEntryHighlight : MonoBehaviour
{
    public ImportMenu importMenu;
    public ImportReducerEntry importReducerEntry;
    public Image visual;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        visual.enabled = importMenu.selectedReducer != null && importMenu.selectedReducer == importReducerEntry.myReducer;
    }
}
