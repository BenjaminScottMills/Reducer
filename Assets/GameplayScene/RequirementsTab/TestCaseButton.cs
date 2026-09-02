using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestCaseButton : MonoBehaviour
{
    public Text testCaseNameText;
    public UIReducerVisual blackInputReducerVisual;
    public UINodeButton blackInputButton;
    public UIReducerVisual whiteInputReducerVisual;
    public UINodeButton whiteInputButton;
    public GenericButton runTestButton;
    public GenericButton deleteTestButton;
    public GameObject standardInputContainer;
    public GameObject sequentialInputContainer;
    Solution solution;
    TestCasesList testCasesList;
    StandardTestCase standardTestCase;
    SequentialTestCase sequentialTestCase;
    bool isStandardTestCase;
    bool isCustom;
    int testNumber;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialise(TestCase testCase, TestCasesList testCasesListArg, bool isCustomArg, int testNumberArg)
    {
        testCasesList = testCasesListArg;
        solution = testCasesList.groundTruthSolution;
        isCustom = isCustomArg;
        testNumber = testNumberArg;
        standardTestCase = testCase as StandardTestCase;
        sequentialTestCase = testCase as SequentialTestCase;
        isStandardTestCase = standardTestCase != null;
        if (isStandardTestCase)
        {
            blackInputReducerVisual.SetVisual(solution.blackInputReducer);
            whiteInputReducerVisual.SetVisual(solution.whiteInputReducer);
            SetupButton(blackInputButton, standardTestCase.blackInput);
            SetupButton(whiteInputButton, standardTestCase.whiteInput);
            SetTestText();
        }
        else
        {
            standardInputContainer.SetActive(false);
            sequentialInputContainer.SetActive(true);
            Debug.Log("To Complete");
        }

        runTestButton.invoker = null;
        deleteTestButton.invoker = null;
    }

    void SetTestText()
    {
        testCaseNameText.text = (isCustom ? "Custom" : "Public") + " Test #" + testNumber.ToString();
    }

    void SetupButton(UINodeButton targetButton, TestCaseInput rootTestCaseInput)
    {
        
        targetButton.enableHighlight = false;
        targetButton.useRawName = true;
        targetButton.tooltipText = testCasesList.tooltipText;
        Reducer reducerToSetTargetButtonTo = rootTestCaseInput.GetDisplayReducer(testCasesList);
        targetButton.reducer = reducerToSetTargetButtonTo;
        targetButton.reducerVisual.SetVisual(reducerToSetTargetButtonTo);
        targetButton.invoker = null; // pass rootTestCaseInput and testCasesList into the invoker.
    }
}
