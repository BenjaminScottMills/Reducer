using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Primitive : Reducer
{
    public override Reducer ExecuteFast(Reducer black, Reducer white)
    {
        return this;
    }
}
