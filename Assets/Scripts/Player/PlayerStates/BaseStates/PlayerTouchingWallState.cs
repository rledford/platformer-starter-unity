using System;
using UnityEngine;

public class PlayerTouchingWallState : PlayerState
{
    protected bool isGrounded;
    protected bool isTouchingWallForward;
    protected bool grabInput;
    protected bool jumpInput;
    protected int xInput;
    protected int yInput;

    public PlayerTouchingWallState(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
    {
    }

        public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = player.CheckIsTouchingGround();
        isTouchingWallForward = player.CheckIsTouchingWallForward();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.MoveX;
        yInput = player.InputHandler.MoveY;
        grabInput = player.InputHandler.GrabInput;
        jumpInput = player.InputHandler.JumpInput;

        if (jumpInput) {
            player.WallJumpState.PrepareWallJumpDirection(isTouchingWallForward);
            stateMachine.ChangeState(player.WallJumpState);
        } else if (isGrounded && !grabInput) {
                stateMachine.ChangeState(player.IdleState);
        } else if (!isTouchingWallForward || (xInput != player.FacingDirection && !grabInput)) {
            stateMachine.ChangeState(player.InAirState);
        }
    }
}
