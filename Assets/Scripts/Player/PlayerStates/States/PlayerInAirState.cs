using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    private int xInput;
    private bool jumpInput;
    private bool dashInput;
    private bool isGrounded;
    private bool isFalling;
    private bool hasCoyote;
    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = player.CheckIsTouchingGround();
        isFalling = player.CheckIsFalling();

        if (isFalling) {
            player.SetGravityScale(playerData.fallGravityScale);
            if (Mathf.Abs(player.CurrentVelocity.y) > playerData.maxFallSpeed) {
                player.SetVelocityY(-playerData.maxFallSpeed);
            }
        } else {
            player.SetGravityScale(playerData.gravityScale);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        UpdateCoyote();

        xInput = player.InputHandler.MoveX;
        jumpInput = player.InputHandler.JumpInput;
        dashInput = player.InputHandler.DashInput;

        if (dashInput) {
            stateMachine.ChangeState(player.DashState);
            return;
        }

        if (isGrounded && isFalling) {
            if (xInput != 0) {
                stateMachine.ChangeState(player.MoveState);
            } else {
                stateMachine.ChangeState(player.IdleState);
            }
        } else if (jumpInput && player.JumpState.CanJump()) {
            player.InputHandler.ConsumeJumpInput();
            stateMachine.ChangeState(player.JumpState);
        } else {
            player.CheckShouldFlip(xInput);
            player.SetVelocityX(playerData.moveSpeed * xInput);
        }
    }

    private void UpdateCoyote() {
        if (hasCoyote && Time.time > startTime + playerData.coyoteTime) {
            hasCoyote = false;
            player.JumpState.DecrementJumpsLeft();
        }
    }

    public void StartCoyote() {
        hasCoyote = true;
    }
}
