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
            nodes = r.nodes.Select(n => new NodeSerialise(n)).ToArray();
            if (r.child == null)
            {
                childNodeIdCounter = 100;
                childNodes = new NodeSerialise[] { };
            }
            else
            {
                childNodeIdCounter = r.child.nodeIdCounter;
                childNodes = r.child.nodes.Select(n => new NodeSerialise(n)).ToArray();
            }
        }

        public string name;
        public string description;
        public uint id;
        public uint nodeIdCounter;
        public NodeSerialise[] nodes;
        public uint childNodeIdCounter;
        public NodeSerialise[] childNodes;
    }

    public class NodeSerialise
    {
        public NodeSerialise(Node n)
        {
            redId = n.reducer.id;
            id = n.id;
            yPos = n.transform.position.y;
            xPos = n.transform.position.x;
            nextId = n.next.id;
            blackLink = n.blackLink;
        }

        public uint redId;
        public uint id;
        public float yPos;
        public float xPos;
        public uint nextId;
        public bool blackLink;
    }
}
