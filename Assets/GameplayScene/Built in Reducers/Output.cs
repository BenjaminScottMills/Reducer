using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Output : Reducer
{
    public override ExecuteReducer Execute(ExecuteReducer black, ExecuteReducer white)
    {
        if (black == null && white == null)
        {
            throw new System.Exception("no output");
        }
        else if (black == null)
        {
            return white;
        }
        else if (white == null)
        {
            return black;
        }
        else
        {
            throw new System.Exception("ambiguous output");
        }
    }
}
