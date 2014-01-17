using UnityEngine;
using System.Collections;

public struct PlayerStats
{
    public enum PlayerStatNames { Health, Attack, AttackRange, CaptureSpeed, LBRotationSpeed, UBRotationSpeed, MovementSpeed, VisionRange};
    public float[] statArray;

    public int level;
    public int health;
    public float speed;
    public float attack;
    public int captureSpeed;
    public int attackRange;
    public float visionRange;
    public float lowerbodyRotationSpeed;
    public float upperbodyRotationSpeed;
}
