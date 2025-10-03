using UnityEngine;

public class EnemySkeleton : Enemy, ICounterable
{
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

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.V))
        {
            HandleCounter();
        }
    }

    [ContextMenu("Stun enemy")]
    public void HandleCounter()
    {
        if (canBeStunned == false)
        {
            return;
        }
        stateMachine.ChangeState(stunnedState);
    }
}
