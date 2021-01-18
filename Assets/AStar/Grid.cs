using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public bool is2d=true;
    public bool isGiamoz;

    private Node[,] grid;//
    public Vector2 gridSize;//地图大小
    public float nodeRadius;//节点半径
    private float nodeDiameter;//节点直径

    public LayerMask WhatLayer;//障碍层
    public GameObject player;
    public int gridCntX, gridCntY;//格子数量

    public List<Node> path = new List<Node>();

    public Vector3 mapRight=new Vector3(1,0,0);
    public Vector3 mapUp = new Vector3(0, 1, 0);
    public Vector3 mapCenter;
    // Use this for initialization
    void Start()
    {
        if(is2d)
            mapUp= new Vector3(0, 1, 0);
        else
            mapUp = new Vector3(0, 0, 1);
        nodeDiameter = nodeRadius * 2;
        gridCntX = Mathf.RoundToInt(gridSize.x / nodeDiameter);//一行几个网格
        gridCntY = Mathf.RoundToInt(gridSize.y / nodeDiameter);//一列几个网格
        grid = new Node[gridCntX, gridCntY];//初始化网格二维数组
        CreatGrid();
    }

    public int MaxSize
    {
        get
        {
            return gridCntX * gridCntY;
        }
    }

    private void CreatGrid()
    {
        Vector3 startPoint = mapCenter - gridSize.x / 2 * mapRight
            - mapUp * gridSize.y / 2;//从地图左下角开始

        for (int i = 0; i < gridCntX; i++)
        {
            for (int j = 0; j < gridCntY; j++)
            {
                Vector3 worldPoint = startPoint + mapRight * (i * nodeDiameter + nodeRadius)
                    + mapUp * (j * nodeDiameter + nodeRadius);//世界坐标
                bool walkable = true;
                if (is2d)
                {
                    walkable = !Physics2D.Raycast(worldPoint, worldPoint, 0.1f, WhatLayer);//球形射线检测障碍物
                }
                else
                {
                    walkable = !Physics.CheckSphere(worldPoint, nodeRadius, WhatLayer);//球形射线检测障碍物
                }
                
                grid[i, j] = new Node(walkable, worldPoint, i, j);
            }
        }
    }


    public Node GetFromPositon(Vector3 positon)
    {
        Vector3 mapLocalPos = positon - mapCenter;
        float percentX = (mapLocalPos.x + gridSize.x / 2) / gridSize.x;
        float percentY = (mapLocalPos.z + gridSize.y / 2) / gridSize.y;
        if (is2d)
        {
            percentY = (mapLocalPos.y + gridSize.y / 2) / gridSize.y;
        }
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridCntX - 1) * percentX);
        int y = Mathf.RoundToInt((gridCntY - 1) * percentY);
        return grid[x, y];
    }
    private void OnDrawGizmos()
    {
        //if (Physics2D.Raycast(checkPos.position, checkPos.position, 0.1f, WhatLayer))
        //{
        //    Debug.Log("true");
        //}
        if (is2d)
        {
            Gizmos.DrawWireCube(mapCenter, new Vector3(gridSize.x, gridSize.y, 1));
        }
        else
        {
            Gizmos.DrawWireCube(mapCenter, new Vector3(gridSize.x, 1, gridSize.y ));
        }
        
        if (grid == null) return;
        Node playernode = GetFromPositon(player.transform.position);
        if (isGiamoz)
        {
            if (path != null)
            {
                foreach (var node in path)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(node._worldPos, Vector3.one * (nodeDiameter - .1f));
                }
            }
        }
        else
        {
            foreach (var node in grid)
            {
                Gizmos.color = node._canwalk ? Color.white : Color.red;
                Gizmos.DrawCube(node._worldPos, Vector3.one * (nodeDiameter - .1f));

            }

            if (playernode != null && playernode._canwalk)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(playernode._worldPos, Vector3.one * (nodeDiameter - .1f));
            }
        }

    }
    public List<Node> GetNerbourhood(Node node)
    {
        List<Node> neibourhood = new List<Node>();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if ((i == 0 && j == 0) || (i == -1 && (j == 1 || j == -1) || (i == 1 && (j == 1 || j == -1)))) continue;
                // if ((i == 0 && j == 0)) continue;
                int tempX = node._gridX + i;
                int tempY = node._gridY + j;
                if (tempX < gridCntX && tempX > 0 && tempY > 0 && tempY < gridCntY)
                {
                    neibourhood.Add(grid[tempX, tempY]);
                }
            }
        }
        return neibourhood;
    }

    public Vector3 GetFirstNode()
    {
        return path[0]._worldPos;
    }
}
