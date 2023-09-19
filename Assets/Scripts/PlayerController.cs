using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;

    [Header("Gravity")]
    public float gravityScale = 1;
    public float fallGravityScale = 1;
    [Space(10)]

    [Header("Grounding")]
    private bool _isGrounded = false;
    private RaycastHit2D[] _groundCollisions = new RaycastHit2D[1];
    public LayerMask groundLayerMask;
    public Transform groundCheckTransform;
    [Range(0f, 1)] public float groundCheckRange;
    [Space(10)]

    [Header("Face")]
    private float _facingX = 1f; // -1 left, 1 right

    [Header("Run")]
    private float _moveInputX = 0f;
    public float moveSpeed;
    [Space(10)]

    [Header("Jump")]
    private int _numJumps = 1;
    private bool _isJumping = false;
    private bool _isJumpCancelled = false;
    private float _jumpPressTime = 0;
    [Range(1, 3)] public int maxJumps = 1;
    public float jumpBufferTime = 0.1f;
    public float jumpHeight = 1;
    [Space(10)]

    [Header("Dash")]
    private bool _isDashing = false;
    private float _dashPressTime = 0;
    private float _dashTimer = 0;
    public float dashBufferTime = 0.1f;
    public float dashTime = 0.1f;
    [Space(10)]

    [Header("Fall")]
    private bool _isFalling = false;
    public float maxFallSpeed = 1;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start() {
    }

    private void Update() {
        GatherInput();
        CheckGround();
        UpdateMove();
        UpdateJump();
        UpdateDash();
        UpdateFall();
    }

    private void FixedUpdate() {

    }

    private void GatherInput() {
        // Move
        if (CanMove()) {
            float leftInput = Input.GetKey(KeyCode.A) ? -1 : 0;
            float rightInput = Input.GetKey(KeyCode.D) ? 1 : 0;
            _moveInputX = leftInput + rightInput;
            // Face
            _facingX = _moveInputX != 0 ? _moveInputX : _facingX;
        }
        // Jump
        if (Input.GetKeyDown(KeyCode.Space)) {
            _jumpPressTime = Time.time;
        } else if (Input.GetKeyUp(KeyCode.Space)) {
            if (_isJumping) {
                _isJumpCancelled = true;
            }
        }
        // Dash
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            _dashPressTime = Time.time;
        }
    }

    private void UpdateMove() {
        if (CanMove()) {
            rb.velocity = new Vector2(_moveInputX * moveSpeed, rb.velocity.y);
        }
    }

    private void UpdateJump() {
        if (_isJumping) {
            if (_isJumpCancelled && rb.velocity.y > 0.1) {
                print("canceling jump");
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
        }

        if (_isGrounded && rb.velocity.y < 0.1) {
            _isJumping = false;
            _isJumpCancelled = false;
            _numJumps = 0;
        }

        if (_isFalling && _numJumps == 0 && !_isGrounded) {
            print("preventing one less jump");
            _numJumps += 1;
        }

        if (CanJump()) {
            print("jumping");
            rb.gravityScale = gravityScale;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            _numJumps += 1;
            _isJumping = true;
            _isJumpCancelled = false;
            _jumpPressTime = 0;
            float jumpForce = Mathf.Sqrt(-2 * Physics2D.gravity.y * gravityScale * jumpHeight);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void UpdateDash() {
        if (_isDashing) {
            _dashTimer += Time.deltaTime;
            if (_dashTimer >= dashTime) {
                print("stopping dash");
                _isDashing = false;
            }
            rb.velocity = new Vector2(_facingX * moveSpeed * 3, 0);
        } else {
            if (CanDash()) {
                print("dashing");
                rb.gravityScale = 0;
                rb.velocity = new Vector2(rb.velocity.x, 0);
                _isDashing = true;
                _dashTimer = 0;
            }
        }
    }

    private void UpdateFall() {
        bool wasFalling = _isFalling;
        _isFalling = !_isDashing && !_isGrounded && rb.velocity.y < 0.1f;

        if (!wasFalling && _isFalling) {
            print("falling");
        }
        
        if (_isFalling) {
            rb.gravityScale = fallGravityScale;
            if (rb.velocity.y > maxFallSpeed) {
                rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
            }
        }
    }

    private void CheckGround() {
        _isGrounded = Physics2D.RaycastNonAlloc(groundCheckTransform.position, Vector2.down, _groundCollisions, groundCheckRange, groundLayerMask) > 0;
    }

    private bool CanMove() {
        return !_isDashing;
    }

    private bool CanJump() {
        bool jumpPressed = _jumpPressTime > 0 && Time.time - _jumpPressTime <= jumpBufferTime;
        bool hasRemainingJumps = _numJumps < maxJumps;
        return !_isDashing && jumpPressed && (_isGrounded || hasRemainingJumps);
    }

    private bool CanDash() {
        return !_isDashing && _dashPressTime > 0 && Time.time - _dashPressTime <= dashBufferTime;
    }

    #region Gizmos
    private void OnDrawGizmosSelected()
    {
		// Gizmos.color = Color.green;
		// Gizmos.DrawWireCube(groundCheckPoint.position, groundCheckSize);
		// Gizmos.color = Color.blue;
		// Gizmos.DrawWireCube(rightWallCheckPoint.position, wallCheckSize);
		// Gizmos.DrawWireCube(leftWallCheckPoint.position, wallCheckSize);
	}
    #endregion
}
