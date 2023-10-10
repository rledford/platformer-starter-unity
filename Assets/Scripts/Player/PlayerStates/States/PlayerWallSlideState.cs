using UnityEngine;

public class PlayerWallSlideState : PlayerTouchingWallState
{
    public PlayerWallSlideState(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.Anim.Play("player_wall_slide");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isExitingState) {
            return;
        }

        player.SetVelocityY(-playerData.wallSlideSpeed);

        if (grabInput && yInput == 0) {
            stateMachine.ChangeState(player.WallGrabState);
        }
    }
}
