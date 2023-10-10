public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        
        player.SetVelocityX(0f);
        player.Anim.Play("player_idle");
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

        if (dashInput && player.DashState.CanDash()) {
            player.InputHandler.UseDashInput();
            stateMachine.ChangeState(player.DashState);
        } else if (xInput != 0) {
            stateMachine.ChangeState(player.MoveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
