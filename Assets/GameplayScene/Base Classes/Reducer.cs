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
    public bool isChild;
    public Reducer child = null;
    public Reducer fastExecOuterBlack = null;
    public Reducer fastExecOuterWhite = null;
    public Reducer nullReducer;
    public Solution solution;
    public GameObject nodePrefab;
    public GameObject reducerPrefab;

    public void LoadFromSerialised(SolutionSerialise.ReducerSerialise r, List<Reducer> reducers)
    {
        child = Instantiate(reducerPrefab, Vector3.zero, Quaternion.identity, transform.parent).GetComponent<Reducer>();

        id = r.id;
        rName = r.name;
        description = r.description;
        nodeIdCounter = r.nodeIdCounter;

        backgroundColour = r.backgroundColour;
        foregroundColour = r.foregroundColour;
        foregroundSprite = r.foregroundSprite;

        nodes = new List<Node>();
        for (int i = 0; i < r.nodes.Length; i++)
        {
            var newNode = Instantiate(nodePrefab, new Vector3(r.nodes[i].xPos, r.nodes[i].yPos), Quaternion.identity, transform.parent).GetComponent<Node>();
            newNode.mouseNode = solution.mouseNode;
            newNode.InitialLoadFromSerialised(r.nodes[i], reducers, child, solution);
            nodes.Add(newNode);
        }
        for (int i = 0; i < r.nodes.Length; i++)
        {
            nodes[i].CreateLinksFromSerialised(r.nodes[i], nodes);
        }

        isChild = false;
        if (!solution.localReducersUnlocked) return;

        child.nodeIdCounter = r.childNodeIdCounter;
        child.ChildInit(this);

        child.nodes = new List<Node>();
        for (int i = 0; i < r.nodes.Length; i++)
        {
            var newNode = Instantiate(nodePrefab, new Vector3(r.nodes[i].xPos, r.nodes[i].yPos), Quaternion.identity, transform.parent).GetComponent<Node>();
            newNode.mouseNode = solution.mouseNode;
            newNode.InitialLoadFromSerialised(r.nodes[i], reducers, child, solution);
            child.nodes.Add(newNode);
        }
        for (int i = 0; i < r.nodes.Length; i++)
        {
            child.nodes[i].CreateLinksFromSerialised(r.nodes[i], child.nodes);
        }
    }

    public ExecuteReducer Execute(Reducer black, Reducer white)
    {
        return Execute(black == null ? null : new ExecuteReducer(black), white == null ? null : new ExecuteReducer(white));
    }

    public virtual ExecuteReducer Execute(ExecuteReducer black, ExecuteReducer white)
    {
        var execRed = new ExecuteReducer(this);
        return execRed.Execute(black, white);
    }

    public Node AddNode(Reducer nodeReducer, Vector3 position, MouseNode mouseNode)
    {
        var newNode = Instantiate(nodePrefab, position, Quaternion.identity, transform.parent).GetComponent<Node>();
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

    public bool Selectable()
    {
        return id > 30 || id == (int)SpecialReducers.local;
    }

    public void ChildInit(Reducer parent)
    {
        foregroundColour = 1;
        backgroundColour = 0;
        foregroundSprite = 9;
        nullReducer = parent.nullReducer;
        solution = parent.solution;
        id = (int)SpecialReducers.local;
        isChild = true;
        rName = parent.rName + " - Child";
        description = "";
    }

    public void SetReducerActive(MouseNode mouseNode)
    {
        foreach (var node in mouseNode.selectedNodes)
        {
            node.SetHighlighted(false);
        }
        mouseNode.selectedNodes.Clear();

        if (solution.currentReducer != null)
        {
            foreach (var node in solution.currentReducer.nodes)
            {
                node.nextConnector?.gameObject.SetActive(false);
                node.gameObject.SetActive(false);
            }
        }

        solution.currentReducer = this;
        foreach (var node in nodes)
        {
            node.gameObject.SetActive(true);
            node.nextConnector?.gameObject.SetActive(true);
        }

        Camera.main.transform.position = TestScreen.cameraDefaultPos;
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

            if (blackIn?.selfRed == null && whiteIn?.selfRed == null)
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

            var outNode = selfRed.nodes.FirstOrDefault(n => n.reducer.id == (int)SpecialReducers.outputNode);
            return outNode.Execute(blackIn, whiteIn, selfRed.isChild ? this : new ExecuteReducer(selfRed.child, blackIn, whiteIn), parentBlackIn, parentWhiteIn);
        }

        public bool Selectable()
        {
            return selfRed.Selectable();
        }
    }
}
