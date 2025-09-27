using UnityEngine;

public class PlayerJumpState : PlayerAiredState
{
    public PlayerJumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // make object go up, increase Y velocity
        player.SetVelocity(rb.linearVelocity.x, player.jumpForce);
    }


    public override void Update()
    {
        base.Update();

        // to be sure we are not in jumpAttackState when we transfer to fall state
        if (rb.linearVelocity.y < 0 && stateMachine.currentState != player.jumpAttackState)
        {
            stateMachine.ChangeState(player.fallState);
        }
    }
}
