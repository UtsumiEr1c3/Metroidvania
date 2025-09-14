using UnityEngine;

public class PlayerJumpAttackState : PlayerState
{
    private bool touchedGround;

    public PlayerJumpAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        touchedGround = false;

        player.SetVelocity(player.jumpAttackVelocity.x * player.facingDir, player.jumpAttackVelocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (player.isGroundDetected && touchedGround == false)
        {
            touchedGround = true;
            anim.SetTrigger("jumpAttackTrigger");
            player.SetVelocity(0, rb.linearVelocity.y);
        }

        if (isTriggerCalled && player.isGroundDetected)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

}
