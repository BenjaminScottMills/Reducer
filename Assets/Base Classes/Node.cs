using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Reducer reducer;
    public uint id;
    public float yPos;
    public Node next;
    public Node bPrev;
    public Node wPrev;
    public bool blackLink;
    public SpriteRenderer spriteRenderer;

    public void LoadFromSerialised(SolutionSerialise.NodeSerialise ns, List<Node> nextRow, List<Reducer> reducers, Reducer local)
    {
        id = ns.id;
        yPos = ns.yPos;
        blackLink = ns.blackLink;
        bPrev = null;
        wPrev = null;

        if (ns.redId < 50)
        {
            switch (ns.redId)
            {
                case (int)Reducer.SpecialReducers.local:
                    reducer = local;
                    break;
                default:
                    reducer = null;
                    break;
            }
        }
        else
        {
            reducer = reducers.First(r => r.id == ns.redId);
        }

        spriteRenderer.sprite = reducer.sprite;

        next = nextRow?.FirstOrDefault(n => n.id == ns.id);
        if (next != null)
        {
            if (blackLink)
            {
                next.bPrev = this;
            }
            else
            {
                next.wPrev = this;
            }
        }
    }

    public Reducer ExecuteFast()
    {
        return reducer.ExecuteFast(bPrev.ExecuteFast(), wPrev.ExecuteFast());
    }

    public static Node FastExecMakeNode(Node toAdd, Reducer black, Reducer white, Reducer outerBlack, Reducer outerWhite, Reducer local) // keep in mind args may be null. If they are null that means don't change the reducers.
    {
        if (toAdd == null) return null;

        var outNode = new Node();
        if (toAdd.reducer.id == (int)Reducer.SpecialReducers.black && black != null)
        {
            outNode.reducer = black;
        }
        else if (toAdd.reducer.id == (int)Reducer.SpecialReducers.white && white != null)
        {
            outNode.reducer = white;
        }
        else if (toAdd.reducer.id == (int)Reducer.SpecialReducers.outerBlack && outerBlack != null)
        {
            outNode.reducer = outerBlack;
        }
        else if (toAdd.reducer.id == (int)Reducer.SpecialReducers.outerWhite && outerWhite != null)
        {
            outNode.reducer = outerWhite;
        }
        else if (toAdd.reducer.id == (int)Reducer.SpecialReducers.local && local != null)
        {
            outNode.reducer = local;
        }
        else
        {
            outNode.reducer = toAdd.reducer;
        }

        outNode.bPrev = FastExecMakeNode(toAdd.bPrev, black, white, outerBlack, outerWhite, local);
        outNode.wPrev = FastExecMakeNode(toAdd.wPrev, black, white, outerBlack, outerWhite, local);
        return outNode;
    }

    public void RealignLinks()
    {
        transform.position = new Vector3(transform.position.x, yPos);
        // make the links correctly positioned.
    }

    public void Translate(Vector3 displacement)
    {
        transform.position += displacement;
        yPos += displacement.y;
        // add displacement to forward link.
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
