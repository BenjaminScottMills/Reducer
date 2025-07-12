using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;

public class Reducer : MonoBehaviour
{
    public enum SpecialReducers { nullRed, fire, earth, plant, water, combine, black, white, local, outerBlack, outerWhite};
    public string rName;
    public string description;
    public uint id;
    public uint nodeIdCounter;
    public List<List<Node>> cols = new List<List<Node>>();
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
        cols = new List<List<Node>>();
        for (int i = r.cols.Length - 1; i >= 0; i--)
        {
            var newCol = new List<Node>();
            var c = r.cols[i];
            foreach (var n in c)
            {
                var newNode = Instantiate(nodePrefab).GetComponent<Node>();
                newNode.LoadFromSerialised(n, cols.LastOrDefault(), reducers, child);
                newCol.Add(newNode);
            }
            cols.Add(newCol);
        }
        cols.Reverse();

        isChild = false;
        child.nodeIdCounter = r.childNodeIdCounter;
        child.isChild = true;

        child.cols = new List<List<Node>>();
        for (int i = r.cols.Length - 1; i >= 0; i--)
        {
            var newCol = new List<Node>();
            var c = r.cols[i];
            foreach (var n in c)
            {
                var newNode = Instantiate(nodePrefab).GetComponent<Node>();
                newNode.LoadFromSerialised(n, child.cols.LastOrDefault(), reducers, child);
                newCol.Add(newNode);
            }
            child.cols.Add(newCol);
        }
        child.cols.Reverse();
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
            localReducer.cols = child.cols;
            localReducer.fastExecOuterBlack = black;
            localReducer.fastExecOuterWhite = white;
        }

        Node fastExecOutNode = Node.FastExecMakeNode(cols.Last()[0], black, white, fastExecOuterBlack, fastExecOuterWhite, localReducer);

        return fastExecOutNode.ExecuteFast();
    }

    public void PositionNodes()
    {
        for (int i = cols.Count - 1; i >= 0; i--)
        {
            foreach (var node in cols[i])
            {
                node.transform.position = new Vector3(i * distanceBetweenNodes, node.yPos);
                // TODO: connector based on node.transform.position and node.next.transform.position and node.blackLinks
            }
        }
    }

    public void AddNode(Reducer nodeReducer, Vector3 position)
    {
        var newNode = Instantiate(nodePrefab).GetComponent<Node>();
        position.x /= distanceBetweenNodes;
        newNode.yPos = position.y;
        newNode.reducer = nodeReducer;
        newNode.spriteRenderer.sprite = nodeReducer.sprite;
        newNode.id = nodeIdCounter;
        nodeIdCounter++;
        if (position.x <= -0.5f)
        {
            cols.ForEach(c => c.ForEach(n => n.Translate(new Vector3(distanceBetweenNodes, 0))));
            cols.Insert(0, new List<Node>() { newNode });
            newNode.transform.position = new Vector3(0, newNode.yPos);
            Camera.main.transform.position += new Vector3(distanceBetweenNodes, 0);
        }
        else
        {
            int col = (int)(position.x + 0.5);
            if (col >= cols.Count)
            {
                col = cols.Count;
                cols.Add(new List<Node>() { newNode });
            }
            else
            {
                int insertIdx = cols[col].FindIndex(c => c.yPos > newNode.yPos);
                if (insertIdx == -1)
                {
                    insertIdx = cols[col].Count;
                }
                cols[col].Insert(insertIdx, newNode);
                for (int i = insertIdx + 1; i < cols[col].Count && cols[col][i].yPos - cols[col][i - 1].yPos <= distanceBetweenNodes; i++)
                {
                    cols[col][i].yPos = distanceBetweenNodes + cols[col][i - 1].yPos;
                    cols[col][i].RealignLinks();
                }
                for (int i = insertIdx - 1; i >= 0 && cols[col][i + 1].yPos - cols[col][i].yPos <= distanceBetweenNodes; i--)
                {
                    cols[col][i].yPos = cols[col][i + 1].yPos - distanceBetweenNodes;
                    cols[col][i].RealignLinks();
                }
            }

            newNode.transform.position = new Vector3(col * distanceBetweenNodes, newNode.yPos);
        }
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
