using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerAbilityState
{
    private int wallJumpDirection;
    public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // float jumpVelocity = Mathf.Sqrt(-2 * Physics2D.gravity.y * playerData.gravityScale * playerData.jumpHeight);
        player.JumpState.ResetJumpsLeft();
        player.SetVelocity(playerData.wallJumpSpeed, playerData.wallJumpAngle, wallJumpDirection);
        player.CheckShouldFlip(wallJumpDirection);
        player.JumpState.DecrementJumpsLeft();
        player.InputHandler.UseJumpInput();
        player.Anim.Play("player_jump");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        UpdateWallJumpTime();
    }

    public void PrepareWallJumpDirection(bool isTouchingWall) {
        wallJumpDirection = isTouchingWall ? -player.FacingDirection : player.FacingDirection;
    }

    private void UpdateWallJumpTime() {
        isAbilityDone = Time.time >= startTime + playerData.wallJumpTime;
    }
}
