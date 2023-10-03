
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

    [Header("In Air")]
    public float coyoteTime = 0.2f;

    [Header("Dash")]
    public float dashSpeed = 30f;
    public float dashTime = 0.3f;
    public float dashCooldownTime = 0.5f;

    [Header("Wall Slide")]
    public float wallSlideSpeed = 2f;
    public float wallSlideCooldownTime = 0.2f;

    [Header("Check Variables")]
    public float wallCheckRadius = 0.1f;
    public float groundCheckRadius = 0.3f;
    public LayerMask whatIsGround;
}
