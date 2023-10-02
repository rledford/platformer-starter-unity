using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerAbilityState
{
    private int facingDirection;

    public PlayerDashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
    {
        facingDirection = player.FacingDirection;
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("dashing");
        isAbilityDone = false;

        player.SetGravityScale(0);
        facingDirection = player.FacingDirection;
        player.SetVelocityX(facingDirection * playerData.dashSpeed);
        player.SetVelocityY(0);
    }

    public override void Exit()
    {
        base.Exit();
        player.SetVelocityX(facingDirection * playerData.moveSpeed);
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time > startTime + playerData.dashTime) {
            Debug.Log("dash complete");
            isAbilityDone = true;
        }
    }
}
