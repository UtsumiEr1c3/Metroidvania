﻿using System;
using System.Collections;
using UnityEngine;

public class Player : Entity
{
    public static event Action OnPlayerDeath;
    public PlayerInputSet input { get; private set; }

    // --- player state ---
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerFallState fallState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerBasicAttackState basicAttackState { get; private set; }
    public PlayerJumpAttackState jumpAttackState { get; private set; }
    public PlayerDeadState deadState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }

    [Header("Attack details")]
    public Vector2[] attackVelocity; // movement velocity when player is attacking
    public Vector2 jumpAttackVelocity; // movement velocity when player is jump attacking
    public float attackVelocityDuration = .1f;
    public float comboResetTime = 1; // player will continue combo attack in 1 second, finish combo when out of 1 second
    private Coroutine queuedAttackCo;

    [Header("Movement details")]
    public float moveSpeed;
    public float jumpForce = 5;
    public Vector2 wallJumpForce;

    [Range(0, 1)]
    public float inAirMoveMultiplier = .7f; // Should be from 0 to 1;
    [Range(0, 1)]
    public float wallSlideSlowMultiplier = .7f;
    [Space]
    public float dashDuration = .25f;
    public float dashSpeed = 20;

    public Vector2 moveInput { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        input = new PlayerInputSet();

        idleState = new PlayerIdleState(this, stateMachine, "isIdle");
        moveState = new PlayerMoveState(this, stateMachine, "isMove");
        jumpState = new PlayerJumpState(this, stateMachine, "isJumpFall");
        fallState = new PlayerFallState(this, stateMachine, "isJumpFall");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "isWallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "isJumpFall");
        dashState = new PlayerDashState(this, stateMachine, "isDash");
        basicAttackState = new PlayerBasicAttackState(this, stateMachine, "isBasicAttack");
        jumpAttackState = new PlayerJumpAttackState(this, stateMachine, "isJumpAttack");
        deadState = new PlayerDeadState(this, stateMachine, "isDead");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "isCounterAttack");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    private void OnEnable()
    {
        input.Enable();

        input.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Movement.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnDisable()
    {
        input.Disable();
    }

    public override void EntityDeath()
    {
        base.EntityDeath();

        OnPlayerDeath?.Invoke();
        stateMachine.ChangeState(deadState);
    }

    public void EnterAttackStateWithDelay()
    {
        if (queuedAttackCo != null)
        {
            StopCoroutine(queuedAttackCo);
        }

        queuedAttackCo = StartCoroutine(EnterAttackStateWithDelayCo());
    }

    private IEnumerator EnterAttackStateWithDelayCo()
    {
        yield return new WaitForEndOfFrame();
        stateMachine.ChangeState(basicAttackState);
    }
}
