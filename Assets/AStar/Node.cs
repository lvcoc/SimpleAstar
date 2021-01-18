using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node :IHeapItem<Node> {
    public bool _canwalk;//是不是障碍物
    public Vector3 _worldPos;//世界坐标
    public int _gridX, _gridY;

    public int gCost;//初始点
    public int hCost;//目标点
    //预估值
    public int fCost
    {
        get { return gCost + hCost; }
    }

    public Node parent;//父节点

    
    //构造
    public Node(bool Canwalk, Vector3 Postion, int x, int y)
    {
        _canwalk = Canwalk;
        _worldPos = Postion;
        _gridX = x;
        _gridY = y;
    }
    int heapIndex;
    //内部构造一个指针
    public int HeapIndex
    {
        get {
            return heapIndex;
        }set
        {
            heapIndex = value;
        }
    }
    //实现内部方法
    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare==0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return compare;
    }
}
