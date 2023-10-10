using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        player.Anim.Play("player_run");
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
        
        player.CheckShouldFlip(xInput);
        if (dashInput && player.DashState.CanDash()) {
            player.InputHandler.UseDashInput();
            stateMachine.ChangeState(player.DashState);
        } else if (xInput == 0) {
            player.DecelerateX();
            if (player.CurrentVelocity.x == 0f) {
                stateMachine.ChangeState(player.IdleState);
            }
        } else {
            player.AccelerateX();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
