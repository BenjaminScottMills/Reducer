using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCasesList : MonoBehaviour
{
    public GameObject testCaseButtonPrefab;
    public TooltipText tooltipText;
    public Solution groundTruthSolution;
    public TestCaseInputPickerPair mostRecentInputPickerPair;
    public RequirementsTab requirementsTab;
    public List<TestCaseButton> customTestCaseButtons;
    public GenericButton addTestCaseButton;
    public Transform addTestCaseButtonContainer;
    public GameObject inputMenuReducerButtonPrefab;
    public GameObject inputMenuListAddRemovePairPrefab;
    public GameObject numberInputFieldPrefab;
    public GameObject testCaseInputPickerMenuPairPrefab;
    public Transform inputMenuContainer;

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
    }

    class AddTestCaseInvoker : GenericButton.MethodInvoker
    {
        public RequirementsTab requirementsTab;
        public override void InvokeMethod()
        {
            requirementsTab.CreateCustomTestCase();
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
        newInputPickerPair.transform.position = (mostRecentInputPickerPair ? mostRecentInputPickerPair.transform : targetTransform).position + new Vector3(-2, 0);
        newInputPickerPair.InitialiseTracking(targetTransform);
        newInputPickerPair.parentPair = mostRecentInputPickerPair;
        mostRecentInputPickerPair = newInputPickerPair;
        return newInputPickerPair.contentsTransform;
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
