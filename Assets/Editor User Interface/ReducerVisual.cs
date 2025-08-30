using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReducerVisual : MonoBehaviour
{
    public SpriteRenderer background;
    public SpriteRenderer foreground;
    public int backgroundColour;
    public int foregroundColour;
    public int foregroundSprite;
    public List<Color> colours;
    public List<Sprite> foregroundSprites;

    public void SetVisual(int bgc, int fgc, int fgs)
    {
        backgroundColour = bgc;
        foregroundColour = fgc;
        foregroundSprite = fgs;
        SetVisual();
    }

    public void SetVisual()
    {
        background.color = colours[backgroundColour];
        foreground.color = colours[foregroundColour];
        foreground.sprite = foregroundSprites[foregroundSprite];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
