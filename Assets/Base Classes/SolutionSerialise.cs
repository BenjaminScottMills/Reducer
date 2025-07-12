using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SolutionSerialise
{
    public SolutionSerialise(Solution solution)
    {
        name = solution.sName;
        idCounter = solution.idCounter;
        reducers = solution.reducers.Select(r => new ReducerSerialise(r)).ToArray();
    }

    public string name;
    public uint idCounter;
    public ReducerSerialise[] reducers;

    public class ReducerSerialise
    {
        public ReducerSerialise(Reducer r)
        {
            name = r.rName;
            description = r.description;
            id = r.id;
            nodeIdCounter = r.nodeIdCounter;
            cols = r.cols.Select(c => c.Select(n => new NodeSerialise(n)).ToArray()).ToArray();
            if (r.child == null)
            {
                childNodeIdCounter = 0;
                childCols = new NodeSerialise[][] { };
            }
            else
            {
                childNodeIdCounter = r.child.nodeIdCounter;
                childCols = r.child.cols.Select(c => c.Select(n => new NodeSerialise(n)).ToArray()).ToArray();
            }
        }

        public string name;
        public string description;
        public uint id;
        public uint nodeIdCounter;
        public NodeSerialise[][] cols;
        public uint childNodeIdCounter;
        public NodeSerialise[][] childCols;
    }

    public class NodeSerialise
    {
        public NodeSerialise(Node n)
        {
            redId = n.reducer.id;
            id = n.id;
            yPos = n.yPos;
            nextId = n.next.id;
            blackLink = n.blackLink;
        }

        public uint redId;
        public uint id;
        public float yPos;
        public uint nextId;
        public bool blackLink;
    }
}
