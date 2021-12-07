using System;
using System.Collections.Generic;
using UnityEngine;

public class Stack : MonoBehaviour
{
    // Start is called before the first frame update
    public List<IStack> activeObjects = new List<IStack>();
    public void AddStack(IStack stack)
    {
        activeObjects.Add(stack);
    }
    public void AddStack(GameObject go)
    {
        try
        {
            AddStack(go.GetComponent<IStack>());
        }
        catch
        {
            throw new Exception("This GameObject not include any IStackObject");
        }
    }
    public void RemoveStack(IStack stack)
    {
        activeObjects.Remove(stack);
    }
    public void RemoveStack()
    {
        if (activeObjects.Count > 0)
            activeObjects.RemoveAt(activeObjects.Count - 1);
    }
    public void RemoveStacks(int count)
    {
        for (int i = 0; i < count; i++)
        {
            RemoveStack();
        }
    }
    public IStack GetLastStack()
    {
        return activeObjects[activeObjects.Count - 1];
    }
    public List<IStack> stackObjects { get => activeObjects; }
    public int StackCount { get => activeObjects.Count; }
}
