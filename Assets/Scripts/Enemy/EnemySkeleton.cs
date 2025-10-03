﻿using UnityEngine;

public class EnemySkeleton : Enemy, ICounterable
{
    public bool CanBeCountered { get => canBeStunned; }

    protected override void Awake()
    {
        base.Awake();

        idleState = new EnemyIdleState(this, stateMachine, "isIdle");
        moveState = new EnemyMoveState(this, stateMachine, "isMove");
        attackState = new EnemyAttackState(this, stateMachine, "isAttack");
        battleState = new EnemyBattleState(this, stateMachine, "isBattle");
        deadState = new EnemyDeadState(this, stateMachine, "idle");
        stunnedState = new EnemyStunnedState(this, stateMachine, "isStunned");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    public void HandleCounter()
    {
        if (CanBeCountered == false)
        {
            return;
        }
        stateMachine.ChangeState(stunnedState);
    }
}
