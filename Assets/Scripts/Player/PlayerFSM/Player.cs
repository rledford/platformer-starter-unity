using System;
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
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallGrabState WallGrabState { get; private set; }
    public PlayerWallClimbState WallClimbState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerDiveState DiveState {get; private set; }
    
    [SerializeField]
    private PlayerData playerData;
    #endregion
    
    #region Check Transforms
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private Transform wallCheck;
    #endregion
    
    #region Other Variables
    public Vector2 CurrentVelocity { get; private set; }
    public int FacingDirection { get; private set; }
    private Vector2 workspace;
    private RaycastHit2D[] wallCollisions = new RaycastHit2D[1];
    #endregion

    #region Unity Callback Functions
    private void Awake() {
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, playerData);
        MoveState = new PlayerMoveState(this, StateMachine, playerData);
        JumpState = new PlayerJumpState(this, StateMachine, playerData);
        InAirState = new PlayerInAirState(this, StateMachine, playerData);
        DashState = new PlayerDashState(this, StateMachine, playerData);
        WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData);
        WallGrabState = new PlayerWallGrabState(this, StateMachine, playerData);
        WallClimbState = new PlayerWallClimbState(this, StateMachine, playerData);
        WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData);
        DiveState = new PlayerDiveState(this, StateMachine, playerData);
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

    public void AccelerateX() {
        float vx = Mathf.Clamp(CurrentVelocity.x + FacingDirection * playerData.moveSpeed * playerData.moveAccel * Time.deltaTime, -playerData.moveSpeed, playerData.moveSpeed);
        SetVelocityX(vx);
    }

    public void DecelerateX() {
        float vx = Math.Abs(CurrentVelocity.x) - playerData.moveSpeed * playerData.moveDecel * Time.deltaTime;

        if (vx < 0.1f) {
            SetVelocityX(0f);
        } else {
            SetVelocityX(vx * FacingDirection);
        }
    }

    public void SetVelocityY(float vy) {
        workspace.Set(CurrentVelocity.x, vy);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocity(float speed, Vector2 angle, int direction) {
        workspace.Set(angle.x * speed * direction, angle.y * speed);
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

    public bool CheckIsTouchingWallForward() {
        return checkTouchingWall(FacingDirection);
    }
    public bool CheckIsTouchingWallBehind() {
        return checkTouchingWall(-FacingDirection);
    }

    private bool checkTouchingWall(int direction) {
        return Physics2D.RaycastNonAlloc(wallCheck.position, Vector2.right * direction, wallCollisions, playerData.wallCheckRange, playerData.whatIsGround) > 0;
    }

    public void CheckShouldFlip(int xInput) {
        if (xInput != 0 && xInput != FacingDirection) {
            Flip();
        }
    }

    public bool CheckIsFalling() => CurrentVelocity.y < 0.1f;
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
        Gizmos.DrawRay(wallCheck.position, Vector2.right * playerData.wallCheckRange * FacingDirection);
	}
    #endregion
}
