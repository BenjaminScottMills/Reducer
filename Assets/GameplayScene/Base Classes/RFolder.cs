using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RFolder
{
    public Solution solution;
    public RFolder parentFolder;
    public string folderName;
    public List<ReducerOrFolder> contents;

    public RFolder(SolutionSerialise.FolderSerialise fs, Solution s, RFolder pf)
    {
        solution = s;
        folderName = fs.folderName;
        parentFolder = pf;
        contents = fs.contents.Select(rfs => new ReducerOrFolder(rfs)).ToList();
        contents.ForEach(rof => s.InitialiseSingleReducerOrFolder(rof, this));
    }
}
