using UnityEngine;
using System.Collections;
using KBConstants;

/// <summary>
/// Container for the three properties sent by the factories to the factory group
/// </summary>
public struct TowerSpawnInfo
{
    public ItemType[] itemType;
    public Team team;
}
