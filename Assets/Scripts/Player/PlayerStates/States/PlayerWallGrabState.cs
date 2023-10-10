using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallGrabState : PlayerTouchingWallState
{
    private Vector3 holdPosition;

    public PlayerWallGrabState(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
    {
    }

    public override void Enter()
    {
        base.Enter();

        holdPosition = player.transform.position;

        player.Anim.Play("player_wall_grab");

        HoldPosition();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isExitingState) {
            return;
        }

        HoldPosition();

        if (yInput > 0) {
            stateMachine.ChangeState(player.WallClimbState);
        } else if (yInput < 0 || !grabInput) {
            stateMachine.ChangeState(player.WallSlideState);
        }
    }

    private void HoldPosition() {
        player.SetVelocityX(0);
        player.SetVelocityY(0);
        player.transform.position = holdPosition;
    }
}
