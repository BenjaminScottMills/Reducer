using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RFolder
{
    public Solution solution;
    public RFolder parentFolder;
    public string folderName;
    public List<ReducerOrFolder> contents;

    public RFolder(Solution s, RFolder pf)
    {
        solution = s;
        folderName = "";
        parentFolder = pf;
        contents = new List<ReducerOrFolder>();
    }
    
    public RFolder(SolutionSerialise.FolderSerialise fs, Solution s, RFolder pf)
    {
        solution = s;
        folderName = fs.folderName;
        parentFolder = pf;
        contents = fs.contents.Select(rfs => new ReducerOrFolder(rfs)).ToList();
        contents.ForEach(rof => s.InitialiseSingleReducerOrFolder(rof, this));
    }

    public HashSet<Reducer> GetContainedReducers()
    {
        HashSet<Reducer> contained = new HashSet<Reducer>();
        Stack<RFolder> unexploredSubfolders = new Stack<RFolder>();
        unexploredSubfolders.Push(this);

        while (unexploredSubfolders.Count > 0)
        {
            RFolder currentFolder = unexploredSubfolders.Pop();
            foreach (var rof in currentFolder.contents)
            {
                if (rof.IsReducer())
                {
                    contained.Add(rof.r);
                    if (rof.r.child != null)
                    {
                        contained.Add(rof.r.child);
                    }
                }
                else
                {
                    unexploredSubfolders.Push(rof.f);
                }
            }
        }

        return contained;
    }
}
