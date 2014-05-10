using KBConstants;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class BankZone : Zone
{
    public FloatingText textPrefab;

    public override void Start()
    {
        base.Start();
        team = KBConstants.Team.None;
        rigidbody.isKinematic = true;
        collider.isTrigger = true;
        ObjectPool.CreatePool(textPrefab);
    }

    private void OnDrawGizmos()
    {
    }
}