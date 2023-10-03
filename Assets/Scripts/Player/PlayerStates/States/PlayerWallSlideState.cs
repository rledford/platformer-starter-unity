using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    private bool isGrounded;
    private bool isOnWall;
    private bool jumpInput;
    private int xInput;
    private int wallDirection;
    private float endTime;

    public PlayerWallSlideState(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
    {
    }

    public override void Enter()
    {
        base.Enter();
        wallDirection = player.FacingDirection;
        player.SetVelocityX(wallDirection * 0.2f);
        player.JumpState.AllowOneJump();
    }

    public override void Exit()
    {
        base.Exit();
        endTime = Time.time;
    }
    public override void DoChecks()
    {
        base.DoChecks();

        xInput = player.InputHandler.MoveX;
        player.CheckShouldFlip(xInput);
        isGrounded = player.CheckIsTouchingGround();
        isOnWall = player.CheckIsTouchingWall();
        jumpInput = player.InputHandler.JumpInput;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isGrounded) {
            if (xInput != 0) {
                stateMachine.ChangeState(player.MoveState);
            } else {
                stateMachine.ChangeState(player.IdleState);
            }
        } else if (jumpInput && player.JumpState.CanJump()) {
            player.InputHandler.UseJumpInput();
            stateMachine.ChangeState(player.JumpState);
        } else if (!isOnWall) {
            stateMachine.ChangeState(player.InAirState);
        } else {
            player.SetVelocityY(-playerData.wallSlideSpeed);
        }
    }
}
