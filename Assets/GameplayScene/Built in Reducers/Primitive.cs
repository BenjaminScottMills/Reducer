using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Primitive : Reducer
{
    public override ExecuteReducer Execute(ExecuteReducer black, ExecuteReducer white)
    {
        return new ExecuteReducer(this);
    }
}
