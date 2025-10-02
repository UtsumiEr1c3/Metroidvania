using UnityEngine;

public class EnemyDeadState : EnemyState
{
    public EnemyDeadState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        anim.enabled = false;

        rb.gravityScale = 12;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 15);

        enemy.GetComponent<Collider2D>().enabled = false; 

        stateMachine.SwitchOffStateMachine();
    }
}
