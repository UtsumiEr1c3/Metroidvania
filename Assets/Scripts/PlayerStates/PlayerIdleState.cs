using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(0, rb.linearVelocity.y);
    }

    /// <summary>
    /// Updates the current state of the player and transitions to the move state if movement input is detected.
    /// </summary>
    public override void Update()
    {
        base.Update();

        if (player.moveInput.x == player.facingDir && player.isWallDetected)
        {
            return;
        }

        if (player.moveInput.x != 0)
        {
            stateMachine.ChangeState(player.moveState);
        }
    }
}
