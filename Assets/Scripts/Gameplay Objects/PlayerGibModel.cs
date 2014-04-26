using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGibModel : ObjectPoolEffect
{

    public PlayerGibPart[] part;

    // Use this for initialization
    private void Start()
    {
        for (int i = 0; i < part.Length; i++)
        {
            ObjectPool.CreatePool(part[i]);
        }
    }

    public override void Init()
    {
        base.Init();
        for (int i = 0; i < part.Length; i++)
        {
            PlayerGibPart p = ObjectPool.Spawn(part[i], transform.position);
            p.Init();
        }
    }

    //protected override IEnumerator Reset(int _lifetime)
    //{
    //    base.Reset(_lifetime);
    //    yield return new WaitForSeconds(0.0f);
    //    for (int i = 0; i < part.Length; i++)
    //    {
    //        part[i].transform.localPosition = transformList[i].localPosition;
    //    }
    //}
}