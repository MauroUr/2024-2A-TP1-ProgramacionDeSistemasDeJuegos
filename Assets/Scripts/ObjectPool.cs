using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Behaviour
{
    private Dictionary<bool, LinkedList<T>> TStates;

    public ObjectPool(){
        TStates = new Dictionary<bool, LinkedList<T>>()
        {
            { true, new LinkedList<T>() },
            { false, new LinkedList<T>() }
        };
    }

    public void Register(T T)
    {
        if (T.gameObject.activeSelf)
            TStates[true].AddLast(T);
        else
            TStates[false].AddLast(T);
    }

    public void MarkAsInactive(T T)
    {
        if (TStates[true].Remove(T))
            TStates[false].AddLast(T);
    }

    public void MarkAsActive(T T)
    {
        if (TStates[false].Remove(T))
            TStates[true].AddLast(T);
    }

    public T FindInactive()
    {
        if (TStates[false].Count > 0)
            return TStates[false].First.Value;
        return null;
    }

    public void Clear()
    {
        TStates[false].Clear();
        TStates[true].Clear();
        TStates.Clear();
    }
}
