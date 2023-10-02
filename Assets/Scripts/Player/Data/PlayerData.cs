
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/BaseData")]
public class PlayerData : ScriptableObject
{
    [Header("Gravity")]
    public float gravityScale = 9f;
    public float fallGravityScale = 15f;
    public float maxFallSpeed = 15f;
    
    [Header("Move State")]
    public float moveSpeed = 10;

    [Header("Jump State")]
    public float jumpHeight = 6;
    public int numJumps = 1;

    [Header("In Air")]
    public float coyoteTime = 0.2f;

    [Header("Dash")]
    public float dashSpeed = 30f;
    public float dashTime = 0.3f;
    public int numDashes = 1;

    [Header("Check Variables")]
    public float groundCheckRadius = 0.3f;
    public LayerMask whatIsGround;
}
