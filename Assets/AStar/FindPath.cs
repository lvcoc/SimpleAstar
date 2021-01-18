using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;

public class FindPath : MonoBehaviour
{


    public Transform player;
    public Transform target;
    private Grid _gird;
    // Use this for initialization
    void Start()
    {
        _gird = GetComponent<Grid>();
       
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.J))
        FindingPath(player.position,target.position);
    }
    
    void FindingPath(Vector3 StartPos,Vector3 EndPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Node startNode = _gird.GetFromPositon(StartPos);
        Node endNode = _gird.GetFromPositon(EndPos);
        Heap<Node> openSet = new Heap<Node>(_gird.MaxSize);//根据地图大小实例化堆的容量
        HashSet<Node> closeSet = new HashSet<Node>();
        openSet.Add(startNode);
        while (openSet.Count()>0)
        {
            //Node CurrentNode = openSet[0];

            //for (int i = 0; i < openSet.Count; i++)
            //{
            //    if (openSet[i].fCost<CurrentNode.fCost||
            //        (openSet[i].fCost==CurrentNode.fCost&&openSet[i].hCost<CurrentNode.hCost))
            //    {
            //        CurrentNode = openSet[i];
            //    }
            //}
            Node CurrentNode = openSet.RemoveFirst();
            //openSet.Remove(CurrentNode);
            closeSet.Add(CurrentNode);

            if (CurrentNode==endNode)
            {
                sw.Stop();
                //print(sw.ElapsedMilliseconds);
                GeneratePaht(startNode,endNode);
            }
            foreach (var node in _gird.GetNerbourhood(CurrentNode))
            {
                if (!node._canwalk || closeSet.Contains(node)) continue;
                int newCost = CurrentNode.gCost + GetDistanceNodes(CurrentNode,node);
                if (newCost < node.gCost||!openSet.Contains(node))
                {
                    node.gCost = newCost;
                    node.hCost = GetDistanceNodes(node, endNode);
                    node.parent = CurrentNode;
                    if (!openSet.Contains(node))
                    {
                        openSet.Add(node);
                    }
                }
            }
        }
    }

    private void GeneratePaht(Node startNode,Node endNode)
    {
        List<Node> path = new List<Node>();
        Node temp = endNode;
        while (temp!=startNode)
        {
            path.Add(temp);
            temp = temp.parent;
        }
        path.Reverse();//反转
        _gird.path = path;
    }

    private int GetDistanceNodes(Node a, Node b)
    {
        int cntX = Mathf.Abs(a._gridX - b._gridX);
        int cntY = Mathf.Abs(a._gridY - b._gridY);
        if (cntX>cntY)
        {
           // return 10 * cntY + 10 * (cntX - cntY);
            return 14 * cntY + 10 * (cntX - cntY);
        }
        else
        {
            //  return 10* cntX + 10 * (cntY - cntX);
            return 14 * cntX + 10 * (cntY - cntX);
        }
    }

   
}
