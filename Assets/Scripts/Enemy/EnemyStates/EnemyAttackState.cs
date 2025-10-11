using UnityEngine;

public class EnemyAttackState : EnemyState
{
    public EnemyAttackState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        SyncAttackSpeed();
    }

    public override void Update()
    {
        base.Update();

        if (isTriggerCalled)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
