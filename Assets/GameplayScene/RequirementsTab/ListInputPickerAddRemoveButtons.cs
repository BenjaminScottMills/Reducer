using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ListInputPickerAddRemoveButtons : MonoBehaviour
{
    public GenericButton addButton;
    public GenericButton removeButton;
    public ListTestCaseInput listTestCaseInput;
    public TestCasesList testCasesList;
    public ReducerSchema childSchema;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup(TestCasesList testCasesListArg, ListTestCaseInput listTestCaseInputArg, ReducerSchema childSchemaArg)
    {
        listTestCaseInput = listTestCaseInputArg;
        testCasesList = testCasesListArg;
        childSchema = childSchemaArg;
        addButton.tooltipText = testCasesList.tooltipText;
        addButton.textToWrite = "Add element to end of list";
        addButton.invoker = new AddInvoker{lipard = this};
        removeButton.tooltipText = testCasesList.tooltipText;
        removeButton.textToWrite = "Remove element from end of list";
        removeButton.invoker = new RemoveInvoker{lipard = this};
    }

    class AddInvoker : GenericButton.MethodInvoker
    {
        public ListInputPickerAddRemoveButtons lipard;
        public override void InvokeMethod()
        {
            lipard.AddEntry();
        }
    }

    class RemoveInvoker : GenericButton.MethodInvoker
    {
        public ListInputPickerAddRemoveButtons lipard;
        public override void InvokeMethod()
        {
            lipard.RemoveEntry();
        }
    }

    public void AddEntry()
    {
        testCasesList.requirementsTab.DirtyCustomTestCases();
        TestCaseInput newTci = childSchema.CreateDefaultInput();
        listTestCaseInput.listValue.Add(newTci);
        UINodeButton addedButton = Instantiate(testCasesList.inputMenuReducerButtonPrefab, transform.parent).GetComponent<UINodeButton>();
        TestCaseButton.SetupButton(addedButton, newTci, childSchema, testCasesList);

        transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
    }

    public void RemoveEntry()
    {
        if (listTestCaseInput.listValue.Count == 0) return;
        testCasesList.requirementsTab.DirtyCustomTestCases();
        TestCaseInput tciToRemove = listTestCaseInput.listValue.Last();
        Destroy(tciToRemove.tciButton);
        listTestCaseInput.listValue.RemoveAt(listTestCaseInput.listValue.Count - 1);
    }
}
