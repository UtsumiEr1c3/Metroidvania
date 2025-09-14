using UnityEngine;

public class Enemy : Entity
{
    // --- Enemy state ---
    public EnemyIdleState idleState;
    public EnemyMoveState moveState;
    public EnemyAttackState attackState;

    [Header("Movement details")] 
    public float idleTime = 2;
    public float moveSpeed = 1.4f;
    [Range(0, 2)] public float moveAnimSpeedMultiplier = 1;
}
