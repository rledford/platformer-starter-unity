using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    private int numJumpsLeft;
    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
    {
        numJumpsLeft = playerData.numJumps;
    }

    public override void Enter()
    {
        base.Enter();

        float jumpForce = Mathf.Sqrt(-2 * Physics2D.gravity.y * playerData.gravityScale * playerData.jumpHeight);
        
        player.SetGravityScale(playerData.gravityScale);
        player.SetVelocityY(jumpForce);

        isAbilityDone = true;
        numJumpsLeft--;
    }

    public bool CanJump() => numJumpsLeft > 0;

    public void ResetJumpsLeft() {
        numJumpsLeft = playerData.numJumps;
    }

    public void DecrementJumpsLeft() => numJumpsLeft--;

    public void AllowOneJump() {
        numJumpsLeft = 1;
    }
}
