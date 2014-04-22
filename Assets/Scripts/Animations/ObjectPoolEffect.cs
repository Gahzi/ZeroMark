using System.Collections;
using UnityEngine;

/// <summary>
/// Base class for objects used in effects that auto-return to objectpool
/// </summary>
public class ObjectPoolEffect : MonoBehaviour
{

    private void Start()
    {
    }

    public virtual void Init(int lifetime)
    {
        StartCoroutine(Recycle(lifetime));
    }

    public virtual void Init()
    {
        StartCoroutine(Recycle(KBConstants.GameConstants.decalLifetime));
    }

    private IEnumerator Recycle(int lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        ObjectPool.Recycle(this);
    }
}