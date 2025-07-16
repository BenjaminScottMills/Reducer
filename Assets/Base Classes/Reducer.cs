using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;

public class Reducer : MonoBehaviour
{
    public enum SpecialReducers { nullRed, fire, earth, plant, water, combine, black, white, local, outerBlack, outerWhite, outputNode};
    public string rName;
    public string description;
    public uint id;
    public uint nodeIdCounter;
    public List<Node> nodes = new List<Node>();
    public Node outNode;
    public Node fastExecOutNode;
    public bool isChild;
    public Reducer child = null;
    public Reducer fastExecOuterBlack = null;
    public Reducer fastExecOuterWhite = null;
    public Reducer nullReducer;
    public Sprite sprite;
    public GameObject nodePrefab;
    public GameObject reducerPrefab;
    private static float distanceBetweenNodes = 1.5f;

    public void LoadFromSerialised(SolutionSerialise.ReducerSerialise r, List<Reducer> reducers)
    {
        child = Instantiate(reducerPrefab).GetComponent<Reducer>();

        rName = r.name;
        description = r.description;
        nodeIdCounter = r.nodeIdCounter;

        nodes = new List<Node>();
        for (int i = 0; i < r.nodes.Length; i--)
        {
            var newNode = Instantiate(nodePrefab, new Vector3(r.nodes[i].xPos, r.nodes[i].yPos), Quaternion.identity).GetComponent<Node>();
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
            var newNode = Instantiate(nodePrefab, new Vector3(r.nodes[i].xPos, r.nodes[i].yPos), Quaternion.identity).GetComponent<Node>();
            newNode.InitialLoadFromSerialised(r.nodes[i], reducers, child);
            child.nodes.Add(newNode);
        }
        for (int i = 0; i < r.nodes.Length; i--)
        {
            child.nodes[i].CreateLinksFromSerialised(r.nodes[i], child.nodes);
        }

        child.outNode = child.nodes.FirstOrDefault(n => n.id == (int)SpecialReducers.outputNode);
    }

    public virtual Reducer ExecuteFast(Reducer black, Reducer white)
    {
        if (black == null && white == null)
        {
            return this;
        }
        else
        {
            if (black == null)
            {
                black = nullReducer;
            }
            else if (white == null)
            {
                white = nullReducer;
            }
        }

        Reducer localReducer = null;
        if (child != null)
        {
            localReducer = new Reducer();
            localReducer.nodes = child.nodes;
            localReducer.fastExecOuterBlack = black;
            localReducer.fastExecOuterWhite = white;
        }

        Node fastExecOutNode = Node.FastExecMakeNode(outNode, black, white, fastExecOuterBlack, fastExecOuterWhite, localReducer);

        return fastExecOutNode.ExecuteFast();
    }

    public void PositionNodes()
    {
        foreach (var node in nodes)
        {
            // TODO: connector based on node.transform.position and node.next.transform.position and node.blackLinks
        }
    }

    public void AddNode(Reducer nodeReducer, Vector3 position, MouseNode mouseNode)
    {
        var newNode = Instantiate(nodePrefab, position, Quaternion.identity).GetComponent<Node>();
        position.x /= distanceBetweenNodes;
        newNode.reducer = nodeReducer;
        newNode.spriteRenderer.sprite = nodeReducer.sprite;
        newNode.id = nodeIdCounter;
        newNode.mouseNode = mouseNode;
        nodeIdCounter++;
        nodes.Add(newNode);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
