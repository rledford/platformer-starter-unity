using UnityEngine;

public class Player : MonoBehaviour
{

    #region Components
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D RB { get; private set; }
    #endregion

    #region State Variables
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    
    [SerializeField]
    private PlayerData playerData;
    #endregion
    
    #region Check Transforms
    [SerializeField]
    private Transform groundCheck;
    #endregion
    
    #region Other Variables
    public Vector2 CurrentVelocity { get; private set; }
    public int FacingDirection { get; private set; }
    private Vector2 workspace;
    #endregion

    #region Unity Callback Functions
    private void Awake() {
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, playerData);
        MoveState = new PlayerMoveState(this, StateMachine, playerData);
        JumpState = new PlayerJumpState(this, StateMachine, playerData);
        InAirState = new PlayerInAirState(this, StateMachine, playerData);
        DashState = new PlayerDashState(this, StateMachine, playerData);
    }

    private void Start() {
        RB = GetComponent<Rigidbody2D>();
        InputHandler = GetComponent<PlayerInputHandler>();
        StateMachine.Initialize(IdleState);
        FacingDirection = 1;
    }

    private void Update() {
        CurrentVelocity = RB.velocity;
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate() {
        StateMachine.CurrentState.PhysicsUpdate();
    }
    #endregion

    #region Set Functions
    public void SetVelocityX(float vx) {
        workspace.Set(vx, CurrentVelocity.y);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocityY(float vy) {
        workspace.Set(CurrentVelocity.x, vy);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetGravityScale(float gs) {
        RB.gravityScale = gs;
    }
    #endregion

    #region  Check Functions
    public bool CheckIsTouchingGround() {
        return Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
    }
    public void CheckShouldFlip(int xInput) {
        if (xInput != 0 && xInput != FacingDirection) {
            Flip();
        }
    }

    public bool CheckIsFalling() => CurrentVelocity.y < 0.01f;
    #endregion

    #region Other Functions
    private void Flip() {
        FacingDirection *= -1;
        transform.Rotate(0f, 180f, 0f);
    }
    #endregion

    #region Gizmos
    private void OnDrawGizmosSelected()
    {
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(groundCheck.position, playerData.groundCheckRadius);
	}
    #endregion
}
