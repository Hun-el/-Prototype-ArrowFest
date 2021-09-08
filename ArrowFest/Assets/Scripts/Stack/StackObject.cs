using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class StackObject : MonoBehaviour 
{
    public List<IStackObject> activeObjects = new List<IStackObject>();

    public void AddStack(IStackObject stack)
    {
        activeObjects.Add(stack);
    }

    public void AddStack(GameObject go)
    {
        try
        {
            AddStack(go.GetComponent<IStackObject>());
        }
        catch
        {

        }
    }

    public void RemoveStack()
    {
        if(activeObjects.Count>0)
            activeObjects.RemoveAt(activeObjects.Count-1);
    }

    public IStackObject GetLastStack()
    { 
        return activeObjects[activeObjects.Count - 1];
    }
    
    public List<IStackObject> stackObjects { get => activeObjects; }
    [HideInInspector] public int StackCount;
    //public int StackCount { get => activeObjects.Count; }

}
