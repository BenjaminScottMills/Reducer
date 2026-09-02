using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCasesList : MonoBehaviour
{
    public GameObject testCaseButtonPrefab;
    public TooltipText tooltipText;
    public Solution groundTruthSolution;
    public RequirementsTab requirementsTab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddTestCase(TestCase newTestCase, bool isCustom, int testNumber)
    {
        if (newTestCase.isPrivate) return;
        TestCaseButton newTestCaseButton = Instantiate(testCaseButtonPrefab, transform).GetComponent<TestCaseButton>();

        newTestCaseButton.Initialise(newTestCase, this, isCustom, testNumber);
    }
}
