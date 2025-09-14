using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (player.moveInput.x == 0  || player.isWallDetected)
        {
            stateMachine.ChangeState(player.idleState);
        }


        player.SetVelocity(player.moveInput.x * player.moveSpeed, rb.linearVelocity.y);
    }
}
