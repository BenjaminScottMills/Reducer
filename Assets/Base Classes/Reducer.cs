using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;

public class Reducer : MonoBehaviour
{
    public enum SpecialReducers { nullRed, fire, earth, plant, water, combine, black, white, local, outerBlack, outerWhite, outputNode };
    public string rName;
    public string description;
    public uint id;
    public uint nodeIdCounter;
    public int foregroundColour;
    public int backgroundColour;
    public int foregroundSprite;
    public List<Node> nodes = new List<Node>();
    public Node outNode;
    public Node fastExecOutNode;
    public bool isChild;
    public Reducer child = null;
    public Reducer fastExecOuterBlack = null;
    public Reducer fastExecOuterWhite = null;
    public Reducer nullReducer;
    public Solution solution;
    public GameObject nodePrefab;
    public GameObject reducerPrefab;
    private static float distanceBetweenNodes = 1.5f;

    public void LoadFromSerialised(SolutionSerialise.ReducerSerialise r, List<Reducer> reducers)
    {
        child = Instantiate(reducerPrefab, Vector3.zero, Quaternion.identity, transform.parent).GetComponent<Reducer>();

        rName = r.name;
        description = r.description;
        nodeIdCounter = r.nodeIdCounter;

        backgroundColour = r.backgroundColour;
        foregroundColour = r.foregroundColour;
        foregroundSprite = r.foregroundSprite;

        nodes = new List<Node>();
        for (int i = 0; i < r.nodes.Length; i--)
        {
            var newNode = Instantiate(nodePrefab, new Vector3(r.nodes[i].xPos, r.nodes[i].yPos), Quaternion.identity, transform.parent).GetComponent<Node>();
            newNode.InitialLoadFromSerialised(r.nodes[i], reducers, child);
            nodes.Add(newNode);
        }
        for (int i = 0; i < r.nodes.Length; i--)
        {
            nodes[i].CreateLinksFromSerialised(r.nodes[i], nodes);
        }

        outNode = nodes.FirstOrDefault(n => n.id == (int)SpecialReducers.outputNode);

        isChild = false;
        child.nodeIdCounter = r.childNodeIdCounter;
        child.isChild = true;

        child.nodes = new List<Node>();
        for (int i = 0; i < r.nodes.Length; i--)
        {
            var newNode = Instantiate(nodePrefab, new Vector3(r.nodes[i].xPos, r.nodes[i].yPos), Quaternion.identity, transform.parent).GetComponent<Node>();
            newNode.InitialLoadFromSerialised(r.nodes[i], reducers, child);
            child.nodes.Add(newNode);
        }
        for (int i = 0; i < r.nodes.Length; i--)
        {
            child.nodes[i].CreateLinksFromSerialised(r.nodes[i], child.nodes);
        }

        child.outNode = child.nodes.FirstOrDefault(n => n.id == (int)SpecialReducers.outputNode);
    }

    public ExecuteReducer Execute(Reducer black, Reducer white)
    {
        return Execute(new ExecuteReducer(black), new ExecuteReducer(white));
    }

    public virtual ExecuteReducer Execute(ExecuteReducer black, ExecuteReducer white)
    {
        var execRed = new ExecuteReducer(this);
        return execRed.Execute(black, white);
    }

    public void PositionNodes()
    {
        foreach (var node in nodes)
        {
            // TODO: connector based on node.transform.position and node.next.transform.position and node.blackLinks
        }
    }

    public Node AddNode(Reducer nodeReducer, Vector3 position, MouseNode mouseNode)
    {
        var newNode = Instantiate(nodePrefab, position, Quaternion.identity, transform.parent).GetComponent<Node>();
        position.x /= distanceBetweenNodes;
        newNode.reducer = nodeReducer;
        newNode.reducerVisual.SetVisual(nodeReducer);
        newNode.id = nodeIdCounter;
        newNode.mouseNode = mouseNode;
        nodeIdCounter++;
        newNode.sortingGroup.sortingOrder = mouseNode.nodeSortingOrderCount;
        mouseNode.nodeSortingOrderCount++;
        nodes.Add(newNode);
        return newNode;
    }

    public class ExecuteReducer
    {
        public Reducer selfRed;
        public ExecuteReducer parentWhiteIn;
        public ExecuteReducer parentBlackIn;

        public ExecuteReducer(Reducer selfRedArg, ExecuteReducer parentBlackInArg = null, ExecuteReducer parentWhiteInArg = null)
        {
            selfRed = selfRedArg;
            parentBlackIn = parentBlackInArg;
            parentWhiteIn = parentWhiteInArg;
        }

        public ExecuteReducer Execute(ExecuteReducer blackIn, ExecuteReducer whiteIn)
        {
            if (selfRed is Primitive || selfRed is Output || selfRed is Combine)
            {
                return selfRed.Execute(blackIn, whiteIn);
            }

            if (blackIn == null && whiteIn == null)
            {
                return this;
            }
            else
            {
                if (blackIn == null)
                {
                    blackIn = new ExecuteReducer(selfRed.nullReducer);
                }
                else if (whiteIn == null)
                {
                    whiteIn = new ExecuteReducer(selfRed.nullReducer);
                }
            }

            var outNode = selfRed.nodes.FirstOrDefault(n => n.id == (int)SpecialReducers.outputNode);
            return outNode.Execute(blackIn, whiteIn, selfRed.isChild ? this : new ExecuteReducer(selfRed.child, blackIn, whiteIn), parentBlackIn, parentWhiteIn);
        }
    }
}
