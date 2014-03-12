using UnityEngine;
using System.Collections;

public class SendToPool : MonoBehaviour
{

    public float delay;
    
    private void Start()
    {
        StartCoroutine(Recycle());
    }

    private IEnumerator Recycle()
    {
        yield return new WaitForSeconds(delay);
        ObjectPool.Recycle(this);
    }
}