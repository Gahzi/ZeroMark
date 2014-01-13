using UnityEngine;
using System.Collections;
using KBConstants;

/// <summary>
/// Container for the three properties sent by the factories to the factory group
/// </summary>
public struct TowerInfo
{
    public ItemType[] itemType;
    public Team team;
    public Vector3 spawnLocation;
    public Quaternion spawnRotation;
}
