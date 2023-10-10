using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallClimbState : PlayerTouchingWallState
{
    public PlayerWallClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.Anim.Play("player_wall_climb");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isExitingState) {
            return;
        }

        player.SetVelocityY(playerData.wallClimbSpeed);

        if (yInput != 1) {
            stateMachine.ChangeState(player.WallGrabState);
        }
    }
}
