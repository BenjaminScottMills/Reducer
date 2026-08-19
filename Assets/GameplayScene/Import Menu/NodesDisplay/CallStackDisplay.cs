using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CallStackDisplay : MonoBehaviour
{
    // set in code
    public List<UINodeButton> buttonStack;
    public MultiPopHandler multiPopHandler;

    // individual config
    public int maxReducers;

    // set in editor
    public Transform nodeButtonsParent;
    public GameObject nodeButtonPrefab;
    public Transform baseButtonTransform;
    public Transform topButtonTransform;
    public TooltipText tooltipText;
    
    Vector3 baseLocalPosition;
    float buttonOffset;
    Vector3 buttonScale;

    public abstract class MultiPopHandler
    {
        public abstract void MultiPop(int n);
    }

    class CallStackDisplayInvoker : GenericButton.MethodInvoker
    {
        public CallStackDisplay callStack;
        public int idx;
        public override void InvokeMethod()
        {
            callStack.ClickButtonAtIndex(idx);
        }
    }

    void Start()
    {
        Reset();
    }

    public void Reset()
    {
        if (buttonStack == null) buttonStack = new();

        foreach (UINodeButton button in buttonStack)
        {
            Destroy(button.gameObject);
        }
        buttonStack.Clear();

        baseLocalPosition = baseButtonTransform.localPosition;
        buttonScale = baseButtonTransform.localScale;
        buttonOffset = (topButtonTransform.localPosition.y - baseButtonTransform.localPosition.y) / (maxReducers - 1);
    }

    public void PushButtonToStack(Reducer r)
    {
        Vector3 instantiateLocalPosition;
        if (buttonStack.Count == 0)
        {
            instantiateLocalPosition = baseLocalPosition;
        }
        else
        {
            instantiateLocalPosition = buttonStack.Last().transform.localPosition;
            instantiateLocalPosition.y += buttonOffset;
        }

        UINodeButton newButton = Instantiate(nodeButtonPrefab, nodeButtonsParent).GetComponent<UINodeButton>();
        newButton.transform.localPosition = instantiateLocalPosition;
        newButton.transform.localScale = buttonScale;
        newButton.enableHighlight = false;
        newButton.restrictToLeftClicks = true;
        newButton.useRawName = true;
        newButton.tooltipText = tooltipText;
        newButton.reducer = r;
        newButton.reducerVisual.SetVisual(r);
        newButton.invoker = new CallStackDisplayInvoker{callStack = this, idx = buttonStack.Count};
        buttonStack.Add(newButton);

        if (buttonStack.Count > maxReducers)
        {
            int n = buttonStack.Count - 1;
            for (int i = 0; i < maxReducers; i++)
            {
                Vector3 newPos = buttonStack[n - i].transform.localPosition;
                newPos.y -= buttonOffset;
                buttonStack[n - i].transform.localPosition = newPos;
            }
        }
    }

    public void PopButton()
    {
        Destroy(buttonStack[buttonStack.Count - 1].gameObject);
        buttonStack.RemoveAt(buttonStack.Count - 1);

        if (buttonStack.Count + 1 > maxReducers)
        {
            int n = buttonStack.Count - 1;
            for (int i = 0; i < maxReducers - 1; i ++)
            {
                Vector3 newPos = buttonStack[n - i].transform.localPosition;
                newPos.y += buttonOffset;
                buttonStack[n - i].transform.localPosition = newPos;
            }
        }
    }

    public void ClickButtonAtIndex(int idx)
    {
        int numPopped = buttonStack.Count - (idx + 1);
        if (numPopped <= 0) return;

        // remove all buttons in range [idx + 1, end]
        for (int i = buttonStack.Count - 1; i > idx; i--)
        {
            Destroy(buttonStack[i].gameObject);
            buttonStack.RemoveAt(i);
        }

        // move buttons appropriately
        if (buttonStack.Count > maxReducers)
        {
            Vector3 targetPos = baseLocalPosition;
            for (int i = maxReducers - 1; i >= 0; i--)
            {
                buttonStack[idx - i].transform.localPosition = targetPos;
                targetPos.y += buttonOffset;
            }
        }
        else
        {
            Vector3 targetPos = baseLocalPosition;
            for (int i = 0; i < buttonStack.Count; i++)
            {
                buttonStack[i].transform.localPosition = targetPos;
                targetPos.y += buttonOffset;
            }
        }

        multiPopHandler.MultiPop(numPopped);
    }
}
