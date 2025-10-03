using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private PlayerCombat combat;
    private bool isCounteredSomebody;

    public PlayerCounterAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        combat = player.GetComponent<PlayerCombat>();
    }

    public override void Enter()
    {
        base.Enter();

        isCounteredSomebody = combat.IsCounterAttackPerformed();
        stateTimer = combat.GetCounterRecoveryDuration();
        anim.SetBool("isCounterAttackPerformed", isCounteredSomebody);
    }

    public override void Update()
    {
        base.Update();

        if (isTriggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (stateTimer < 0 && isCounteredSomebody == false)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
