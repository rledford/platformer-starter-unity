
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/BaseData")]
public class PlayerData : ScriptableObject
{
    [Header("Gravity")]
    public float gravityScale = 9f;
    public float fallGravityScale = 15f;
    public float maxFallSpeed = 15f;
    
    [Header("Move State")]
    public float moveSpeed = 10f;
    public float moveAccel = 5f;
    public float moveDecel = 5f;

    [Header("Jump State")]
    public float jumpHeight = 6;
    public int numJumps = 1;

    [Header("In Air State")]
    public float coyoteTime = 0.2f;
    public float variableJumpHeightMultiplier = 0.5f;

    [Header("Dash State")]
    public float dashSpeed = 30f;
    public float dashTime = 0.3f;
    public float dashCooldownTime = 0.5f;

    [Header("Wall Slide State")]
    public float wallSlideSpeed = 2f;

    [Header("Wall Climb State")]
    public float wallClimbSpeed = 1f;

    [Header("Wall Jump State")]
    public float wallJumpSpeed = 10f;
    public Vector2 wallJumpAngle = new Vector2(1,2);
    public float wallJumpTime = 0.4f;

    [Header("Dive State")]
    public float diveSpeed = 30f;

    [Header("Check Variables")]
    public float wallCheckRange = 0.5f;
    public float groundCheckRadius = 0.3f;
    public LayerMask whatIsGround;
}
