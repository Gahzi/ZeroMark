using UnityEngine;
using System.Collections;
public class KillTag : KBGameObject
{
    public GameObject source;

    public override void Start()
    {
        base.Start();
        StartCoroutine(RemoveSource());
    }

    private IEnumerator RemoveSource()
    {
        yield return new WaitForSeconds(1);
        source = null;
    }
}