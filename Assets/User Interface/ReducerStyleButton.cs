using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ReducerStyleButton : MonoBehaviour, IPointerClickHandler
{
    public ReducerMenu menu;
    public Image targetImage;
    public int indexInRow;
    public int row;
    // Start is called before the first frame update
    void Start()
    {
        if (row == 0)
        {
            targetImage.sprite = menu.reducerVisual.foregroundSprites[indexInRow];
        }
        else
        {
            targetImage.color = menu.reducerVisual.colours[indexInRow];
        }
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (row == 0)
        {
            menu.reducerVisual.foregroundSprite = indexInRow;
            menu.reducerVisual.SetVisual();
            menu.resultForeground.sprite = menu.reducerVisual.foreground.sprite;
        }
        else if (row == 1)
        {
            menu.reducerVisual.foregroundColour = indexInRow;
            menu.reducerVisual.SetVisual();
            menu.resultForeground.color = menu.reducerVisual.foreground.color;
        }
        else
        {
            menu.reducerVisual.backgroundColour = indexInRow;
            menu.reducerVisual.SetVisual();
            menu.resultBackground.color = menu.reducerVisual.background.color;
        }
    }
}
