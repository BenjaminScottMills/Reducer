using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FavouriteButton : MonoBehaviour, IPointerClickHandler
{
    public ImportReducerEntry myEntry;
    public Sprite fav;
    public Sprite unfav;
    public Image image;
    public bool favourited;

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        favourited = !favourited;
        if (favourited)
        {
            image.sprite = fav;
            myEntry.SetFavourited();
        }
        else
        {
            image.sprite = unfav;
            myEntry.SetUnfavourited();
        }
    }

    public void Initialise(bool setFavourited)
    {
        favourited = setFavourited;
        image.sprite = favourited ? fav : unfav;
    }
}
