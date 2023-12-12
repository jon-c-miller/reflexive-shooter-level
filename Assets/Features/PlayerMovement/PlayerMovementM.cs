using UnityEngine;

[System.Serializable]
public class PlayerMovementM
{
    [Header("Configuration")]
    public float ForwardMoveRate = 25f;
    public float StrafeRate = 20f;
    public float StopRate = 0.1f;
    [Space]
    public float JumpForce = 5f;
    public float MidJumpMovementDamping = 0.3f;

    [Header("Keybindings")]
    public KeyCode MoveForwardKey = KeyCode.W;
    public KeyCode MoveBackwardKey = KeyCode.S;
    public KeyCode MoveLeftKey = KeyCode.A;
    public KeyCode MoveRightKey = KeyCode.D;
    public KeyCode JumpKey = KeyCode.Space;

    [Header("Current")]
    public bool IsActive;
    public Vector3 TargetMovement;
}