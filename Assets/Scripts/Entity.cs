using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    protected StateMachine stateMachine;
    
    private bool isFacingRight = true;
    public int facingDir { get; private set; } = 1;

    [Header("Collision detection")]
    [SerializeField] private float groundCheckDistance; // the raycast distance to check if entity is on the ground
    [SerializeField] private float wallCheckDistance; // the raycast distance to check if entity is contact to a wall
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform primaryWallCheck; // the head of entity
    [SerializeField] private Transform secondaryWallCheck; // the foot of entity
    public bool isGroundDetected { get; private set; }
    public bool isWallDetected { get; private set; }

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stateMachine = new StateMachine();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        HandleCollisionDetection();
        stateMachine.UpdateActiveState();
    }

    public void CallAnimationTrigger()
    {
        stateMachine.currentState.CallAnimationTrigger();
    }

    /// <summary>
    /// set entity velocity with new value
    /// </summary>
    /// <param name="xVelocity"></param>
    /// <param name="yVelocity"></param>
    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        HandleFlip(xVelocity);
    }

    /// <summary>
    /// if moving right && not facing right, flip transform.
    /// if moving left && facing right, flip transform.
    /// </summary>
    /// <param name="xVelcoity"></param>
    private void HandleFlip(float xVelcoity)
    {
        if (xVelcoity > 0 && isFacingRight == false)
        {
            Flip();
        }
        else if (xVelcoity < 0 && isFacingRight)
        {
            Flip();
        }
    }

    /// <summary>
    /// Flips the object horizontally by rotating it 180 degrees around the Y-axis.
    /// </summary>
    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        isFacingRight = !isFacingRight;
        facingDir = facingDir * -1;
    }

    /// <summary>
    /// Performs collision detection to determine if the entity is grounded or in contact with a wall.
    /// </summary>
    /// <remarks>This method updates the state of the entity by checking for ground and wall collisions using
    /// raycasts. The results are stored in the <c>isGroundDetected</c> and <c>isWallDetected</c> fields.</remarks>
    private void HandleCollisionDetection()
    {
        isGroundDetected = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        isWallDetected = Physics2D.Raycast(primaryWallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround)
                    && Physics2D.Raycast(secondaryWallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));
        Gizmos.DrawLine(primaryWallCheck.position, primaryWallCheck.position + new Vector3(wallCheckDistance * facingDir, 0));
        Gizmos.DrawLine(secondaryWallCheck.position, secondaryWallCheck.position + new Vector3(wallCheckDistance * facingDir, 0));
    }
}
