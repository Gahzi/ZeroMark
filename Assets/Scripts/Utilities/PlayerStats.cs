using UnityEngine;
using System.Collections;

public struct PlayerStats
{
    public enum PlayerStatNames { Health, LBRotationSpeed, UBRotationSpeed, MovementSpeed};
    public float[] statArray;

    public int health;
    public float speed;
    public float lowerbodyRotationSpeed;
    public float upperbodyRotationSpeed;
    public float regerationRate;
}
