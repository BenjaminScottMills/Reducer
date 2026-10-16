using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestCasesList : MonoBehaviour
{
    public GameObject testCaseButtonPrefab;
    public TooltipText tooltipText;
    public Solution groundTruthSolution;
    public TestCaseInputPickerPair mostRecentInputPickerPair;
    public TestCaseInputPickerPair firstInputPickerPair;
    public RequirementsTab requirementsTab;
    public List<TestCaseButton> customTestCaseButtons;
    public GenericButton addTestCaseButton;
    public GenericButton testCaseInputMenuBackground;
    public Transform addTestCaseButtonContainer;
    public GameObject inputMenuReducerButtonPrefab;
    public GameObject inputMenuListAddRemovePairPrefab;
    public GameObject numberInputFieldPrefab;
    public GameObject testCaseInputPickerMenuPairPrefab;
    public Transform inputMenuContainer;
    public ExtendedStandaloneInputModule inputModule;

    // special reduers for visuals
    public Reducer trueVisualReducer;
    public Reducer falseVisualReducer;
    public Reducer numberVisualReducer;
    public Reducer listVisualReducer;
    public Reducer baseTestReducer; // Not just used for visuals, this will be used for executing later on. At the moment, it's empty; implement later.
    bool entryShouldHaveTopBorder = false;

    // Start is called before the first frame update
    void Start()
    {
        addTestCaseButton.invoker = new AddTestCaseInvoker{requirementsTab = requirementsTab};
        testCaseInputMenuBackground.invoker = new ClearTestCaseMenu{tci = this};
    }

    class AddTestCaseInvoker : GenericButton.MethodInvoker
    {
        public RequirementsTab requirementsTab;
        public override void InvokeMethod()
        {
            requirementsTab.CreateCustomTestCase();
        }
    }

    class ClearTestCaseMenu : GenericButton.MethodInvoker
    {
        public TestCasesList tci;
        public override void InvokeMethod()
        {
            tci.SetLeftmostInputPickerPair(null);
            tci.inputModule.ReTriggerTouchPress(pointerEventData);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddTestCase(TestCase newTestCase, bool isCustom, int testNumber)
    {
        if (newTestCase.isPrivate) return;
        TestCaseButton newTestCaseButton = Instantiate(testCaseButtonPrefab, transform).GetComponent<TestCaseButton>();
        addTestCaseButtonContainer.SetSiblingIndex(addTestCaseButtonContainer.GetSiblingIndex() + 1);

        if (customTestCaseButtons == null) customTestCaseButtons = new();
        if (isCustom) customTestCaseButtons.Add(newTestCaseButton);
        newTestCaseButton.Initialise(newTestCase, this, isCustom, testNumber, entryShouldHaveTopBorder);
        entryShouldHaveTopBorder = true;
    }

    public void RemoveCustomTestCase(TestCaseButton buttonToRemove)
    {
        requirementsTab.RemoveTestCase(buttonToRemove.testCase);
        customTestCaseButtons.Remove(buttonToRemove);
        Destroy(buttonToRemove.gameObject);
        for (int i = 0; i < customTestCaseButtons.Count; i++)
        {
            customTestCaseButtons[i].SetTestNumber(i + 1);
        }
    }

    public Transform OpenInputScrollWindow(RectTransform targetTransform)
    {
        TestCaseInputPickerPair newInputPickerPair = Instantiate(testCaseInputPickerMenuPairPrefab, mostRecentInputPickerPair?.transform ?? inputMenuContainer).GetComponent<TestCaseInputPickerPair>();
        newInputPickerPair.tcl = this;
        newInputPickerPair.transform.position = targetTransform.position + new Vector3(-1.7f, 0);
        newInputPickerPair.InitialiseTracking(targetTransform);
        if (firstInputPickerPair == null)
        {
            firstInputPickerPair = newInputPickerPair;
            testCaseInputMenuBackground.gameObject.SetActive(true);
        }
        else
        {
            mostRecentInputPickerPair.childPair = newInputPickerPair;
        }
        mostRecentInputPickerPair = newInputPickerPair;
        return newInputPickerPair.contentsTransform;
    }

    public void SetLeftmostInputPickerPair(TestCaseInputPickerPair newLeftmost)
    {
        mostRecentInputPickerPair = newLeftmost;
        if (newLeftmost == null)
        {
            Destroy(firstInputPickerPair?.gameObject);
            firstInputPickerPair = null;
            testCaseInputMenuBackground.gameObject.SetActive(false);
        }
        else
        {
            Destroy(newLeftmost.childPair?.gameObject);
            newLeftmost.childPair = null;
        }
    }

    public ReducerSchema GetBlackSchema()
    {
        return requirementsTab.GetBlackSchema();
    }

    public ReducerSchema GetWhiteSchema()
    {
        return requirementsTab.GetWhiteSchema();
    }
}
