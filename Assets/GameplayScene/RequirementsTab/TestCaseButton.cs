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
    public GameObject blackInputContainer;
    public GameObject whiteInputContainer;
    public GameObject sequentialInputContainer;
    public GameObject greyOverlay;
    public GameObject topBorder;
    public Image testResultDisplay;
    public Sprite checkmarkSprite;
    public Sprite redXSprite;
    Solution solution;
    TestCasesList testCasesList;
    public TestCase testCase;
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

    public void Initialise(TestCase testCaseArg, TestCasesList testCasesListArg, bool isCustomArg, int testNumberArg, bool showTopBorder)
    {
        testResultDisplay.gameObject.SetActive(false);
        topBorder.SetActive(showTopBorder);
        testCasesList = testCasesListArg;
        solution = testCasesList.groundTruthSolution;
        isCustom = isCustomArg;
        testNumber = testNumberArg;
        testCase = testCaseArg;
        standardTestCase = testCase as StandardTestCase;
        sequentialTestCase = testCase as SequentialTestCase;
        isStandardTestCase = standardTestCase != null;
        if (isStandardTestCase)
        {
            blackInputReducerVisual.SetVisual(solution.blackInputReducer);
            whiteInputReducerVisual.SetVisual(solution.whiteInputReducer);
            SetupButton(blackInputButton, standardTestCase.blackInput, testCasesList.GetBlackSchema());
            SetupButton(whiteInputButton, standardTestCase.whiteInput, testCasesList.GetWhiteSchema());
        }
        else
        {
            whiteInputContainer.SetActive(false);
            blackInputContainer.SetActive(false);
            sequentialInputContainer.SetActive(true);
            Debug.Log("To Complete");
        }

        SetTestText();

        runTestButton.invoker = null;
        if (isCustom)
        {
            deleteTestButton.invoker = new DeleteTestInvoker{testCaseButton = this, testCasesList = testCasesList};
        }
        else
        {
            deleteTestButton.gameObject.SetActive(false);
            greyOverlay.SetActive(true);
        }
    }

    class DeleteTestInvoker : GenericButton.MethodInvoker
    {
        public TestCaseButton testCaseButton;
        public TestCasesList testCasesList;
        public override void InvokeMethod()
        {
            testCasesList.RemoveCustomTestCase(testCaseButton);
        }
    }

    public void SetTestNumber(int newTestNumber)
    {
        testNumber = newTestNumber;
        SetTestText();
    }

    void SetTestText()
    {
        testCaseNameText.text = (isCustom ? "Custom" : "Public") + " Test #" + testNumber.ToString();
    }

    void SetupButton(UINodeButton targetButton, TestCaseInput rootTestCaseInput, ReducerSchema inputSchema, bool setInvoker = true)
    {
        SetupButton(targetButton, rootTestCaseInput, inputSchema, testCasesList, setInvoker);
    }

    static void SetupButton(UINodeButton targetButton, TestCaseInput rootTestCaseInput, ReducerSchema inputSchema, TestCasesList testCasesList, bool setInvoker = true)
    {
        targetButton.enableHighlight = false;
        targetButton.useRawName = true;
        targetButton.tooltipText = testCasesList.tooltipText;
        Reducer reducerToSetTargetButtonTo = rootTestCaseInput.GetDisplayReducer(testCasesList);
        targetButton.reducer = reducerToSetTargetButtonTo;
        targetButton.reducerVisual.SetVisual(reducerToSetTargetButtonTo);
        if (setInvoker)
        {
            targetButton.invoker = new TestCaseInputInvoker
            {
                testCaseInput = rootTestCaseInput,
                testCasesList = testCasesList,
                inputSchema = inputSchema,
                targetButton = targetButton
            };
        }
    }

    class TestCaseInputInvoker : GenericButton.MethodInvoker
    {
        public TestCaseInput testCaseInput;
        public ReducerSchema inputSchema;
        public TestCasesList testCasesList;
        public UINodeButton targetButton;

        public override void InvokeMethod()
        {
            // make the window to the left pop out.
            Transform addButtonParent = testCasesList.OpenInputScrollWindow(targetButton.rectTransform);
            switch (inputSchema.type)
            {
                case SchemaType.natNumber:
                    Instantiate(testCasesList.numberInputFieldPrefab, addButtonParent);
                    // TODO: initialise it by passing in testCaseInput or whatever.
                    break;
                case SchemaType.finiteList:
                    var finiteListInputSchema = inputSchema as FiniteListReducerSchema;
                    var finiteListTestCaseInput = testCaseInput as ListTestCaseInput;
                    if (finiteListInputSchema.childSchemas.Count != finiteListTestCaseInput.listValue.Count) throw new System.Exception("finite list schema child length and list test case input contents length mismatch");
                    
                    for (int i  = 0; i < finiteListInputSchema.childSchemas.Count; i++)
                    {
                        UINodeButton addedButton = Instantiate(testCasesList.inputMenuReducerButtonPrefab, addButtonParent).GetComponent<UINodeButton>();
                        SetupButton(addedButton, finiteListTestCaseInput.listValue[i], finiteListInputSchema.childSchemas[i], testCasesList);
                    }

                    break;
                case SchemaType.infiniteList:
                    var infiniteListInputSchema = inputSchema as InfiniteListReducerSchema;
                    var infiniteListTestCaseInput = testCaseInput as ListTestCaseInput;
                    
                    foreach (var tci in infiniteListTestCaseInput.listValue)
                    {
                        UINodeButton addedButton = Instantiate(testCasesList.inputMenuReducerButtonPrefab, addButtonParent).GetComponent<UINodeButton>();
                        SetupButton(addedButton, tci, infiniteListInputSchema.childSchema, testCasesList);
                    }
                    break;
                case SchemaType.boolean:
                    List<bool> boolValueOptions = new List<bool>{false, true};
                    foreach (bool bv in boolValueOptions)
                    {
                        UINodeButton addedButton = Instantiate(testCasesList.inputMenuReducerButtonPrefab, addButtonParent).GetComponent<UINodeButton>();
                        TestCaseInput fixedTestCaseInput = new BooleanTestCaseInput(bv);
                        SetupButton(addedButton, fixedTestCaseInput, null, testCasesList, false);
                        addedButton.invoker = new UpdateInputToFixedTestCaseInput{fixedTestCaseInput = fixedTestCaseInput, parentButtonInvoker = this};
                    }
                    break;
                default:
                    var simpleInputSchema = inputSchema as SimpleReducerSchema;
                    ReducerValue[] reducerValueOptions = simpleInputSchema.ReducerValueOptions();
                    Debug.Log(reducerValueOptions.Length);
                    foreach (ReducerValue rv in reducerValueOptions)
                    {
                        UINodeButton addedButton = Instantiate(testCasesList.inputMenuReducerButtonPrefab, addButtonParent).GetComponent<UINodeButton>();
                        TestCaseInput fixedTestCaseInput = new SimpleReducerTestCaseInput(rv, inputSchema.type);
                        SetupButton(addedButton, fixedTestCaseInput, null, testCasesList, false);
                        addedButton.invoker = new UpdateInputToFixedTestCaseInput{fixedTestCaseInput = fixedTestCaseInput, parentButtonInvoker = this};
                    }
                    break;

            }
        }

        class UpdateInputToFixedTestCaseInput : GenericButton.MethodInvoker
        {
            public TestCaseInput fixedTestCaseInput;
            public TestCaseInputInvoker parentButtonInvoker;
            public override void InvokeMethod()
            {
                parentButtonInvoker.testCaseInput.CopyFrom(fixedTestCaseInput);

                Reducer reducerToSetTargetButtonTo = parentButtonInvoker.testCaseInput.GetDisplayReducer(parentButtonInvoker.testCasesList);
                parentButtonInvoker.targetButton.reducer = reducerToSetTargetButtonTo;
                parentButtonInvoker.targetButton.reducerVisual.SetVisual(reducerToSetTargetButtonTo);
            }
        }
    }
}
