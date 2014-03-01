using UnityEngine;

/// <summary>
/// Base class for gameplay object that require tracking of inside/outside for players and/or other gameplay objects.
/// </summary>
public abstract class Zone : KBGameObject
{
    public override void Start()
    {
        base.Start();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }

}