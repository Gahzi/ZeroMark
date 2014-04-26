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
}