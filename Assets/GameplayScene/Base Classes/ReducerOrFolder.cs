using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReducerOrFolder
{
    public Reducer r;
    public RFolder f;
    public SolutionSerialise.ReducerSerialise? rs;
    public SolutionSerialise.FolderSerialise? fs; 


    public ReducerOrFolder(SolutionSerialise.ReducerOrFolderSerialise rf)
    {
        r = null;
        f = null;
        if (rf.isReducer)
        {
            fs = null;
            rs = JsonUtility.FromJson<SolutionSerialise.ReducerSerialise>(rf.myJson);
        }
        else
        {
            rs = null;
            fs = JsonUtility.FromJson<SolutionSerialise.FolderSerialise>(rf.myJson);
        }
    }

    public ReducerOrFolder(Reducer rIn)
    {
        r = rIn;
        f = null;
        rs = null;
        fs = null;
    }

    public ReducerOrFolder(RFolder fIn)
    {
        r = null;
        f = fIn;
        rs = null;
        fs = null;
    }

    public bool IsReducer()
    {
        if (r == null && f != null)
        {
            return false;
        }
        else if (r != null && f == null)
        {
            return true;
        }

        if (r == null)
        {
            throw new System.Exception("ReducerOrFolder is niether");
        }
        throw new System.Exception("ReducerOrFolder is both");
    }
}
