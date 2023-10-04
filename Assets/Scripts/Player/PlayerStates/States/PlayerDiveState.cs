using UnityEngine;

public class PlayerDiveState : PlayerAbilityState
{
    private bool isGrounded;
    private int numDivesLeft;

    public PlayerDiveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
    {
        numDivesLeft = 1;
    }

    public override void Enter()
    {
        base.Enter();

        numDivesLeft--;
        player.SetVelocityX(0f);
        player.SetVelocityY(-playerData.diveSpeed);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        player.SetVelocityY(-playerData.diveSpeed);
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = player.CheckIsTouchingGround();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isGrounded) {
            isAbilityDone = true;
        }
    }

    public void ResetDivesLeft() {
        numDivesLeft = 1;
    }

    public bool CanDive() {
        return numDivesLeft > 0;
    }
}
