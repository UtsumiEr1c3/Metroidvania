using UnityEngine;

public abstract class EntityState
{
    protected StateMachine stateMachine;
    protected string animBoolName; 
    
    protected Animator anim;
    protected Rigidbody2D rb;

    protected float stateTimer;
    protected bool isTriggerCalled;

    protected EntityState(StateMachine stateMachine, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        anim.SetBool(animBoolName, true);
        isTriggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        anim.SetBool(animBoolName, false);
    }

    public void CallAnimationTrigger()
    {
        isTriggerCalled = true;
    }
}
