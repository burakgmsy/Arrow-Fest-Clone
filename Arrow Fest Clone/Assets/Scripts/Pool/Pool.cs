using System.Collections;
using UnityEngine;

public class Pool : MonoBehaviour, IPool, IStack
{
    GameObject IPool.gameObject_ => gameObject;
    [SerializeField] bool autoDestroyWithTime = false;
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