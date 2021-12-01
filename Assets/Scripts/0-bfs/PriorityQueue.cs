
using System.Collections.Generic;
using System;
using UnityEngine;
public class PriorityQueue<T>
{
    private List<Tuple<T,int>> elements = new List<Tuple<T,int>>();

    public int Count
    {
        get { return elements.Count; }
    }

    public void Enqueue(T item, int priority)
    {
        elements.Add(Tuple.Create(item,priority));
    }

    public T Dequeue()
    {
        
        int bestindex = 0;
        for(int i = 0; i < elements.Count; i++)
        {
            if(elements[i].Item2 < elements[bestindex].Item2)
                bestindex = i;
        }
        T bestitem = elements[bestindex].Item1;
        elements.RemoveAt(bestindex);
        
        return bestitem;
    }

    
}