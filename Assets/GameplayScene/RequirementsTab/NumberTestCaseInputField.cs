using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberTestCaseInputField : MonoBehaviour
{
    TestCasesList testCasesList;
    NumberTestCaseInput testCaseInput;
    public InputField numberInputField;
    const int maxNumberValue = 999;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HandleInputComplete()
    {
        testCasesList.requirementsTab.DirtyCustomTestCases();

        int numVal;
        if (int.TryParse(numberInputField.text, out numVal))
        {
            if (numVal < 0)
            {
                numVal *= -1;
            }
            if (numVal > maxNumberValue)
            {
                numVal = maxNumberValue;
            }
        }
        else if (numberInputField.text.Length > 4)
        {
            numVal = maxNumberValue;
        }
        numberInputField.text = numVal.ToString();

        testCaseInput.numberValue = numVal;
    }

    public void Initialise(TestCasesList testCasesListArg, NumberTestCaseInput testCaseInputArg)
    {
        testCasesList = testCasesListArg;
        testCaseInput = testCaseInputArg;
        numberInputField.text = testCaseInput.numberValue.ToString();
        numberInputField.onEndEdit.AddListener((_) => HandleInputComplete());
    }
}
