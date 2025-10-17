using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightSquare : MonoBehaviour
{
    public bool active;
    public SpriteRenderer spriteRenderer;
    public MouseNode mouseNode;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.enabled = active;
        if (!active) return;

        transform.localScale = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position + new Vector3(0, 0, 10);

        if (Input.GetMouseButton(0))
        {

        }
        else
        {
            List<Node> allNodes = mouseNode.solution.currentReducer.nodes;
            HashSet<Node> selection = new HashSet<Node>();

            Vector3 bottomLeft;
            Vector3 absScale = new Vector3(Math.Abs(transform.localScale.x), Math.Abs(transform.localScale.y));

            if (transform.localScale.x > 0)
            {
                if (transform.localScale.y > 0)
                {
                    bottomLeft = transform.position;
                }
                else
                {
                    bottomLeft = new Vector3(transform.position.x, transform.position.y + transform.localScale.y);
                }
            }
            else
            {
                if (transform.localScale.y > 0)
                {
                    bottomLeft = new Vector3(transform.position.x + transform.localScale.x, transform.position.y);
                }
                else
                {
                    bottomLeft = transform.position + transform.localScale;
                }
            }

            active = false;

            Vector3 checkPos;
            float checkRad = Node.radius;
            foreach (var node in allNodes)
            {
                checkPos = node.transform.position;

                if (checkPos.x + checkRad < bottomLeft.x)
                {
                    continue;
                }
                else if (checkPos.x <= bottomLeft.x)
                {
                    if (checkPos.y + checkRad < bottomLeft.y)
                    {
                        continue;
                    }
                    else if (checkPos.y <= bottomLeft.y)
                    {
                        if (Vector3.Distance(checkPos, bottomLeft) < checkRad)
                        {
                            selection.Add(node);
                        }
                    }
                    else if (checkPos.y <= bottomLeft.y + absScale.y)
                    {
                        selection.Add(node);
                    }
                    else if (checkPos.y - checkRad <= bottomLeft.y + absScale.y)
                    {
                        if (Vector3.Distance(checkPos, new Vector3(bottomLeft.x, bottomLeft.y + absScale.y)) < checkRad)
                        {
                            selection.Add(node);
                        }
                    }
                }
                else if (checkPos.x <= bottomLeft.x + absScale.x)
                {
                    if (checkPos.y + checkRad < bottomLeft.y)
                    {
                        continue;
                    }
                    else if (checkPos.y - checkRad <= bottomLeft.y + absScale.y)
                    {
                        selection.Add(node);
                    }
                }
                else if (checkPos.x - checkRad <= bottomLeft.x + absScale.x)
                {
                    if (checkPos.y + checkRad < bottomLeft.y)
                    {
                        continue;
                    }
                    else if (checkPos.y <= bottomLeft.y)
                    {
                        if (Vector3.Distance(checkPos, new Vector3(bottomLeft.x + absScale.x, bottomLeft.y)) < checkRad)
                        {
                            selection.Add(node);
                        }
                    }
                    else if (checkPos.y <= bottomLeft.y + absScale.y)
                    {
                        selection.Add(node);
                    }
                    else if (checkPos.y - checkRad <= bottomLeft.y + absScale.y)
                    {
                        if (Vector3.Distance(checkPos, new Vector3(bottomLeft.x + absScale.x, bottomLeft.y + absScale.y)) < checkRad)
                        {
                            selection.Add(node);
                        }
                    }
                }
            }

            mouseNode.selectedNodes.UnionWith(selection);
            foreach (var node in selection)
            {
                node.SetHighlighted(true);
            }
        }
    }

    public void Enable(Vector3 newPos)
    {
        transform.position = newPos;
        active = true;
    }
}
