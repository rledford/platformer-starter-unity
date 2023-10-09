using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    private int xInput;
    private bool downInput;
    private bool jumpInput;
    private bool jumpInputCanceled;
    private bool isJumping;
    private bool dashInput;
    private bool grabInput;
    private bool isGrounded;
    private bool isFalling;
    private bool isTouchingWallForward;
    private bool isTouchingWallBehind;
    private bool wasTouchingWallForward;
    private bool wasTouchingWallBehind;
    private bool hasCoyote;
    private bool hasWallJumpCoyote;
    private float wallJumpCoyoteStartTime;
    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        wasTouchingWallForward = false;
        wasTouchingWallBehind = false;
        isTouchingWallForward = false;
        isTouchingWallBehind = false;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = player.CheckIsTouchingGround();
        isFalling = player.CheckIsFalling();
        isTouchingWallForward = player.CheckIsTouchingWallForward();
        isTouchingWallBehind = player.CheckIsTouchingWallBehind();

        wasTouchingWallForward = isTouchingWallForward;
        wasTouchingWallBehind = isTouchingWallBehind;

        if (!hasWallJumpCoyote && !isTouchingWallForward && !isTouchingWallBehind && (wasTouchingWallForward || wasTouchingWallBehind)) {
            StartWallJumpCoyote();
        }

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
        UpdateWallJumpCoyote();

        xInput = player.InputHandler.MoveX;
        downInput = player.InputHandler.MoveY < 0;
        jumpInput = player.InputHandler.JumpInput;
        jumpInputCanceled = player.InputHandler.JumpInputCanceled;
        dashInput = player.InputHandler.DashInput;
        grabInput = player.InputHandler.GrabInput;

        if (dashInput && player.DashState.CanDash()) {
            player.InputHandler.UseDashInput();
            stateMachine.ChangeState(player.DashState);
            return;
        }

        CheckJumpMultiplier();

        if (isGrounded) {
            if (isFalling) {
                if (xInput != 0) {
                    stateMachine.ChangeState(player.MoveState);
                } else {
                    stateMachine.ChangeState(player.IdleState);
                }
            }
        } else if ((isTouchingWallForward || isTouchingWallBehind) && jumpInput || hasWallJumpCoyote) {
            wallJumpCoyoteStartTime = 0;
            player.WallJumpState.PrepareWallJumpDirection(player.CheckIsTouchingWallForward());
            stateMachine.ChangeState(player.WallJumpState);
        } else if (isTouchingWallForward && grabInput) {
            stateMachine.ChangeState(player.WallGrabState);
        } else if (isTouchingWallForward && xInput == player.FacingDirection && isFalling) {
            stateMachine.ChangeState(player.WallSlideState);
        } else if (jumpInput) {
            if (downInput) {
                if (player.DiveState.CanDive()) {
                    stateMachine.ChangeState(player.DiveState);
                }
            } else if (player.JumpState.CanJump()) {
                stateMachine.ChangeState(player.JumpState);
            }
        } else {
            player.CheckShouldFlip(xInput);

            if (xInput != 0) {
                player.AccelerateX();
            } else {
                player.DecelerateX();
            }
            // player.SetVelocityX(playerData.moveSpeed * xInput);
        }
    }

    private void CheckJumpMultiplier() {
        if (isJumping) {
            if (jumpInputCanceled) {
                player.SetVelocityY(player.CurrentVelocity.y * playerData.variableJumpHeightMultiplier);
                isJumping = false;
            } else if (player.CurrentVelocity.y <= 0) {
                isJumping = false;
            }
        }
    }

    private void UpdateCoyote() {
        if (hasCoyote && Time.time > startTime + playerData.coyoteTime) {
            hasCoyote = false;
            player.JumpState.DecrementJumpsLeft();
        }
    }

    private void UpdateWallJumpCoyote() {
        if (hasWallJumpCoyote && Time.time > wallJumpCoyoteStartTime + playerData.coyoteTime) {
            hasWallJumpCoyote = false;
        }
    }

    public void StartCoyote() {
        hasCoyote = true;
        wallJumpCoyoteStartTime = Time.time;
    }

    public void StartWallJumpCoyote() {
        hasWallJumpCoyote = true;
    }

    public void SetIsJumping() => isJumping = true;
}
