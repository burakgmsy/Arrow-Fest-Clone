using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PoolSystem
{
    static PoolSystem()
    {
        ResetPools();
    }
    public static void ResetPools()
    {
        pools = new List<PoolClass>();
    }
    // Start is called before the first frame update
    static List<PoolClass> pools = new List<PoolClass>();
    public static GameObject InstantiniatePool(this IPool IPool)
    {
        PoolClass newPoolC = FindPoolClass(IPool.gameObject_.name);


        if (newPoolC.pools.Count > 0)
        {
            GameObject pg = newPoolC.pools[0];
            pg.SetActive(true);
            newPoolC.pools.RemoveAt(0);
            return pg;
        }
        GameObject go = GameObject.Instantiate(IPool.gameObject_); go.name = IPool.gameObject_.name; go.transform.parent = newPoolC.Parent;
        return go;
    }
    public static void DestroyPool(this IPool IPool)
    {
        PoolClass newPoolC = FindPoolClass(IPool.gameObject_.name);
        newPoolC.pools.Add(IPool.gameObject_);
        IPool.gameObject_.SetActive(false);
    }
    public static List<GameObject> GetAllPool(this GameObject gameObject, string type)
    {
        return FindPoolClass(type).pools;
    }
    static PoolClass FindPoolClass(string type)
    {
        foreach (var item in pools)
        {
            if (type == item.TypePool)
                return item;
        }
        GameObject parentt = new GameObject(); parentt.name = type + "s";
        PoolClass newPoolc = new PoolClass(type, parentt.transform);
        pools.Add(newPoolc);
        return newPoolc;
    }
    public static string GetPoolType(this IPool poolObject)
    {
        return FindPoolClass(poolObject.gameObject_.name).TypePool;
    }

}
class PoolClass
{
    string type;
    Transform parent;
    public PoolClass(string _type, Transform _parent)
    {
        type = _type; parent = _parent;
    }
    public string TypePool
    {
        get => type;
    }
    public Transform Parent { get => parent; }
    public List<GameObject> pools = new List<GameObject>();
}
