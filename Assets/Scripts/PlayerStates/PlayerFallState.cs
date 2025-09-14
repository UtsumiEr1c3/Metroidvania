using UnityEngine;

public class PlayerFallState : PlayerAiredState
{
    public PlayerFallState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        // if player detecting the ground below, if yes, go to idle state
        if (player.isGroundDetected)
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (player.isWallDetected)
        {
            stateMachine.ChangeState(player.wallSlideState);
        }
    }
}
