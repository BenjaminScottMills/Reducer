using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIReducerVisual : MonoBehaviour
{
    public ReducerVisual reducerVisual;
    public Image foreground;
    public Image background;

    public void SetVisual(ReducerVisual r)
    {
        reducerVisual.SetVisual(r);
        SetVisual();
    }

    public void SetVisual(Reducer r)
    {
        reducerVisual.SetVisual(r);
        SetVisual();
    }

    public void SetVisual(int bgc, int fgc, int fgs)
    {
        reducerVisual.SetVisual(bgc, fgc, fgs);
        SetVisual();
    }

    public void SetVisual()
    {
        foreground.sprite = reducerVisual.foreground.sprite;
        foreground.color = reducerVisual.foreground.color;
        background.color = reducerVisual.background.color;
    }
}
