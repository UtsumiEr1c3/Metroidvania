using UnityEngine;

public class EnemyGroundedState : EnemyState
{
    public EnemyGroundedState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        // if enemy detect player, state machine change to battle state
        if (enemy.PlayerDetected() == true)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
