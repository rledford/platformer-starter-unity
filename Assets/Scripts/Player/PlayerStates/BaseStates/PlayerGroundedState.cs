using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    protected int xInput;
    protected bool downInput;
    protected bool dashInput;
    private bool jumpInput;
    private bool grabInput;
    private bool isTouchingWall;
    private bool isGrounded;

    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = player.CheckIsTouchingGround();
        isTouchingWall = player.CheckIsTouchingWallForward();
    }

    public override void Enter()
    {
        base.Enter();

        player.SetGravityScale(playerData.gravityScale);
        player.JumpState.ResetJumpsLeft();
        player.DiveState.ResetDivesLeft();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        xInput = player.InputHandler.MoveX;
        downInput = player.InputHandler.MoveY < 0;
        jumpInput = player.InputHandler.JumpInput;
        dashInput = player.InputHandler.DashInput;
        grabInput = player.InputHandler.GrabInput;

        if (jumpInput && player.JumpState.CanJump() && !downInput) {
            stateMachine.ChangeState(player.JumpState);
        } else if (!isGrounded) {
            player.InAirState.StartCoyote();
            stateMachine.ChangeState(player.InAirState);
        } else if (isTouchingWall && grabInput) {
            stateMachine.ChangeState(player.WallGrabState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

}
