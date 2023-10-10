using UnityEngine;

public class PlayerDashState : PlayerAbilityState
{
    private int facingDirection;
    private float endTime;

    public PlayerDashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
    {
        facingDirection = player.FacingDirection;
        endTime = 0;
    }

    public override void Enter()
    {
        base.Enter();
        
        isAbilityDone = false;
        facingDirection = player.FacingDirection;

        player.SetGravityScale(0);
        player.SetVelocityX(facingDirection * playerData.dashSpeed);
        player.SetVelocityY(0);

        player.Anim.Play("player_dash");
    }

    public override void Exit()
    {
        base.Exit();
        player.SetVelocityX(facingDirection * playerData.moveSpeed);
        endTime = Time.time;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time > startTime + playerData.dashTime) {
            isAbilityDone = true;
        }
    }

    public bool CanDash() {
        return Time.time > endTime + playerData.dashCooldownTime;
    }
}
