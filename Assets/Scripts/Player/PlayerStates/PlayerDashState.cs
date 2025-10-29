using UnityEngine;

public class PlayerDashState : PlayerState
{
    private float originalGravityScale;
    private int dashDir;

    public PlayerDashState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        skillManager.dash.OnStartEffect();
        player.vfx.DoImageEchoEffect(player.dashDuration);

        // change dash derection if input
        if (player.moveInput.x != 0)
        {
            dashDir = (int)player.moveInput.x;
        }
        else
        {
            dashDir = player.facingDir;
        }

        stateTimer = player.dashDuration;
        originalGravityScale = rb.gravityScale;
        rb.gravityScale = 0;
    }


    public override void Update()
    {
        base.Update();
        CancelDashIfNeeded();
        player.SetVelocity(player.dashSpeed * dashDir, 0);


        if (stateTimer < 0)
        {
            if (player.isGroundDetected)
            {
                stateMachine.ChangeState(player.idleState);
            }
            else
            {
                stateMachine.ChangeState(player.fallState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();

        skillManager.dash.OnEndEffect();

        player.SetVelocity(0, 0);
        rb.gravityScale = originalGravityScale;
    }

    private void CancelDashIfNeeded()
    {
        if (player.isWallDetected)
        {
            if (player.isGroundDetected)
            {
                stateMachine.ChangeState(player.idleState);
            }
            else
            {
                stateMachine.ChangeState(player.wallSlideState);
            }
        }
    }
}
