using UnityEngine;

public class EnemySkeleton : Enemy
{
    protected override void Awake()
    {
        base.Awake();

        idleState = new EnemyIdleState(this, stateMachine, "isIdle");
        moveState = new EnemyMoveState(this, stateMachine, "isMove");
        attackState = new EnemyAttackState(this, stateMachine, "isAttack");
        battleState = new EnemyBattleState(this, stateMachine, "isBattle");
        deadState = new EnemyDeadState(this, stateMachine, "idle");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }
}
