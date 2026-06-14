using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImportReducerEntry : MonoBehaviour, IPointerClickHandler
{
    public Reducer myReducer;
    public ImportFolderContents.FavouritedReducer myFavReducer;
    public bool isFavouriteType;
    public Text buttonText;
    public ImportMenu importMenu;
    public ImportFolderContents importFolderContents;
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
        importFolderContents = importMenu.importFolderContents;
        myReducer = r;
        reducerVisual.SetVisual(r);
        buttonText.text = r.rName;
        bool isFavourite = importFolderContents.IsFavourited(r);
        favouriteButton.Initialise(isFavourite);
        isFavouriteType = false;
    }

    public void Initialise(ImportFolderContents.FavouritedReducer fr)
    {
        importFolderContents = importMenu.importFolderContents;
        myFavReducer = fr;
        reducerVisual.SetVisual(fr);
        buttonText.text = fr.reducerName;
        bool isFavourite = true;
        favouriteButton.Initialise(isFavourite);
        isFavouriteType = true;
    }

    public void SetFavourited()
    {
        if (isFavouriteType)
        {
            importFolderContents.AddFavourite(myFavReducer);
        }
        else
        {
            importFolderContents.AddFavourite(myReducer);
        }
    }

    public void SetUnfavourited()
    {
        if (isFavouriteType)
        {
            importFolderContents.RemoveFavourite(myFavReducer);
        }
        else
        {
            importFolderContents.RemoveFavourite(myReducer);
        }
    }
}
