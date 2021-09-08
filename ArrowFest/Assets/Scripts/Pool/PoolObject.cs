using System.Collections;
using UnityEngine;

public class PoolObject : MonoBehaviour, IPoolObject , IStackObject
{
    GameObject IPoolObject.gameObject_ => gameObject;
    [SerializeField]bool autoDestroyWithTime = false;
    [SerializeField] [Min(0)] float time = 1f;

    private void Awake()
    {
        if (autoDestroyWithTime)
            StartCoroutine(AutoDestoyPool());            
    }
    
    IEnumerator AutoDestoyPool()
    {
        yield return new WaitForSeconds(time);
        this.DestroyPool();
    }
    
}
