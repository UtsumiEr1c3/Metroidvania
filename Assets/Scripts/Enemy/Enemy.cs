using System;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Enemy : Entity
{
    // --- Enemy state ---
    public EnemyIdleState idleState;
    public EnemyMoveState moveState;
    public EnemyAttackState attackState;
    public EnemyBattleState battleState;
    public EnemyDeadState deadState;
    public EnemyStunnedState stunnedState;

    [Header("Battle details")] 
    public float battleMoveSpeed = 3;
    public float attackDistance = 2;
    public float battleTimeDuration = 5f;
    public float minRetreatDistance = 1;
    public Vector2 retreatVelocity;

    [Header("Stunned state details")] 
    public float stunnedDuration = 1;
    public Vector2 stunnedVelocity = new Vector2(7, 7);
    [SerializeField] protected bool canBeStunned;

    [Header("Movement details")] 
    public float idleTime = 2;
    public float moveSpeed = 1.4f;
    [Range(0, 2)] public float moveAnimSpeedMultiplier = 1;

    [Header("Player detection")] 
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private float playerCheckDistance = 10f;
    public Transform player { get; private set; }

    private void OnEnable()
    {
        Player.OnPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        Player.OnPlayerDeath -= HandlePlayerDeath;
    }

    protected override IEnumerator SlowDownEntityCo(float duration, float slowMultiplier)
    {
        float originalMoveSpeed = moveSpeed;
        float originalBattleSpeed = battleMoveSpeed;
        float originalAnimSpeed = anim.speed;

        float speedMultiplier = 1 - slowMultiplier;

        moveSpeed *= speedMultiplier;
        battleMoveSpeed *= speedMultiplier;
        anim.speed *= speedMultiplier;

        yield return new WaitForSeconds(duration);

        moveSpeed = originalMoveSpeed;
        battleMoveSpeed = originalBattleSpeed;
        anim.speed = originalAnimSpeed;
    }

    public void EnableConterWindow(bool enable)
    {
        canBeStunned = enable;
    }

    public override void EntityDeath()
    {
        base.EntityDeath();

        stateMachine.ChangeState(deadState);
    }

    /// <summary>
    /// this function subscribe OnPlayerDeath, will be called when player dead
    /// </summary>
    private void HandlePlayerDeath()
    {
        stateMachine.ChangeState(idleState);
    }

    /// <summary>
    /// try enter battle state
    /// </summary>
    /// <param name="player"></param>
    public void TryEnterBattleState(Transform player)
    {
        if (stateMachine.currentState == battleState || stateMachine.currentState == attackState)
        {
            return;
        }
        this.player = player;
        stateMachine.ChangeState(battleState);
    }

    public Transform GetPlayerReference()
    {
        if (player == null)
        {
            player = PlayerDetected().transform;
        }

        return player;
    }

    /// <summary>
    /// performs a raycast to check if player is in distance and direction
    /// </summary>
    /// <returns></returns>
    public RaycastHit2D PlayerDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerCheck.position, Vector2.right * facingDir, playerCheckDistance, whatIsPlayer | whatIsGround);

        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            return default;
        }
        return hit;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (facingDir * playerCheckDistance), playerCheck.position.y));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (facingDir * attackDistance), playerCheck.position.y));
        Gizmos.color = Color.green;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (facingDir * minRetreatDistance), playerCheck.position.y));
    }
}
