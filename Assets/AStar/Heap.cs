using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Heap<T> where T:IHeapItem<T> {
    T[] items;
    int currentItemCount;//当前树的大小
    //指定树的容量
    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    public void Add(T item)
    {
        //先插入到最后
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        //向上排序
        SortUp(item);
        currentItemCount++;
    }
    //移除根节点，向下排序
    public T RemoveFirst()
    {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    public int Count()
    {
            return currentItemCount;

    }

    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }
    //向下排序,寻找子节点
    void SortDown(T item)
    {
        while (true)
        {
            int childIndexLeft = item.HeapIndex * 2 + 1;//左叶
            int childIndexRight = item.HeapIndex * 2 + 2;//右叶
            int swapIndex = 0;
            //如果还存在子节点
            if (childIndexLeft<currentItemCount)
            {
                swapIndex = childIndexLeft;
                if (childIndexRight<currentItemCount)
                {
                    if (items[childIndexLeft].CompareTo(items[childIndexRight])>0)
                    {
                        swapIndex = childIndexRight;//得到小的节点
                    }
                }
                if (item.CompareTo(items[swapIndex])>0)//和小的节点比较
                {
                    Swap(item, items[swapIndex]);
                }//如果子节点大，返回
                else
                {
                    return;
                }
            }//如果没有子节点了，返回
            else
            {
                return;
            }
        }
    }
    //向上排序，寻找父节点
    void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;

        while (true)
        {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem)<0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }
            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }
    //交换数据和指针
    void Swap(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}
public interface IHeapItem<T>:IComparable<T>
{
int HeapIndex
    {
        get;
        set;
    }
}
