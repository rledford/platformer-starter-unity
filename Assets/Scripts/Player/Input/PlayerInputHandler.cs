using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 RawMoveInput { get; private set; }
    public int MoveX { get; private set; }
    public int MoveY { get; private set; }
    public bool JumpInput { get; private set; }
    public bool DashInput { get; private set; }

    [SerializeField]
    private float jumpInputBufferTime = 0.2f;
    private float jumpInputStartTime;

    [SerializeField]
    private float dashInputBufferTime = 0.2f;
    private float dashInputStartTime;

    private void Update() {
        CheckJumpInputBufferTime();
        CheckDashInputBufferTime();
    }

    public void OnMoveInput(InputAction.CallbackContext ctx) {
        RawMoveInput = ctx.ReadValue<Vector2>();
        MoveX = (int)(RawMoveInput * Vector2.right).normalized.x;
        MoveY = (int)(RawMoveInput * Vector2.up).normalized.y;
    }

    public void OnJumpInput(InputAction.CallbackContext ctx) {
        if (ctx.started) {
            JumpInput = true;
            jumpInputStartTime = Time.time;
        }
    }

    public void OnDashInput(InputAction.CallbackContext ctx) {
        if (ctx.started) {
            DashInput = true;
            dashInputStartTime = Time.time;
        } else if (ctx.canceled) {
            DashInput = false;
        }
    }

    public void UseJumpInput() => JumpInput = false;
    public void UseDashInput() => DashInput = false;

    private void CheckJumpInputBufferTime() {
        if (JumpInput && Time.time >= jumpInputStartTime + jumpInputBufferTime) {
            JumpInput = false;
        }
    }

    private void CheckDashInputBufferTime() {
        if (DashInput && Time.time > dashInputStartTime + dashInputBufferTime) {
            DashInput = false;
        }
    }
}
