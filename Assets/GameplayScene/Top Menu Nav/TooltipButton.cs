using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    bool hovered;
    public TooltipText tooltipText;
    public String myTooltip;
    // Start is called before the first frame update
    void Start()
    {
        hovered = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hovered)
        {
            tooltipText.text = myTooltip;
            tooltipText.topMenuMode = true;
        }
    }
    
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        hovered = true;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        hovered = false;
    }
}
