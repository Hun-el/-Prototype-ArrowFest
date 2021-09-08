using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

    static List<PoolClass> pools = new List<PoolClass>();
    public static GameObject InstantiniatePool(this IPoolObject IPoolObject)
    {
        PoolClass newPoolC = FindPoolClass(IPoolObject.gameObject_.name);

        if (newPoolC.pools.Count > 0)
        {
            GameObject pg = newPoolC.pools[0];
            pg.SetActive(true);
            newPoolC.pools.RemoveAt(0);
            return pg;
        }
        GameObject go =  GameObject.Instantiate(IPoolObject.gameObject_); go.name = IPoolObject.gameObject_.name; go.transform.parent = newPoolC.Parent;
        return go;
    }

    public static void DestroyPool(this IPoolObject IPoolObject)
    {
        PoolClass newPoolC = FindPoolClass(IPoolObject.gameObject_.name);
        newPoolC.pools.Add(IPoolObject.gameObject_);
        IPoolObject.gameObject_.transform.DOScale(new Vector3(0,0,0),Random.Range(0f,1f)).SetEase(Ease.OutBounce).OnComplete(()=>{ IPoolObject.gameObject_.SetActive(false); });;
    }

    static PoolClass FindPoolClass(string type)
    {
        foreach (var item in pools)
        {
            if (type == item.TypePool)
                return item;
        }
        GameObject parentt = new GameObject(); parentt.name = type + "s";
        PoolClass newPoolc = new PoolClass(type,parentt.transform);
        pools.Add(newPoolc);
        return newPoolc;
    }
}

class PoolClass
{
    string type;
    Transform parent;
    public PoolClass(string _type,Transform _parent)
    {
        type = _type; parent = _parent;
    }

    public string TypePool
    {
        get => type;
    }
    public Transform Parent { get => parent; }
    public  List<GameObject> pools = new List<GameObject>();
}
