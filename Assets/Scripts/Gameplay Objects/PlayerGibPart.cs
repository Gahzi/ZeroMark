using UnityEngine;

public class PlayerGibPart : ObjectPoolEffect
{
    public override void Init()
    {
        base.Init();
        //rigidbody.AddExplosionForce(10.0f, Vector3.down, 10.0f);
    }
}