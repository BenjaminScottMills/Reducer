using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImportReducerEntry : MonoBehaviour, IPointerClickHandler
{
    public Reducer myReducer;
    public Text buttonText;
    public ImportMenu importMenu;
    public UIReducerVisual reducerVisual;
    public FavouriteButton favouriteButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        importMenu.SetActiveReducer(myReducer);
    }

    public void Initialise(Reducer r)
    {
        reducerVisual.SetVisual(r);
        buttonText.text = r.rName;
        bool isFavourite = false;
        Debug.Log("Temporarily hardcoded false to isFavourite");
        favouriteButton.Initialise(isFavourite);
    }

    public void SetFavourited()
    {
        
    }

    public void SetUnfavourited()
    {
        
    }
}
