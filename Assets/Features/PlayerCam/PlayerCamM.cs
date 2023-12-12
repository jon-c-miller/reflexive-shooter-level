using UnityEngine;

[System.Serializable]
public class PlayerCamM
{
    [Header("Configuration")]
    public float ViewHeight = 1.5f;
    public float HorizontalSensitivity = 1f;
    public float VerticalSensitivity = 1.8f;
    [Space]
    public bool ClampVerticalRotation;
    public float VerticalRotationMin = -75f;
    public float VerticalRotationMax = 75f;
    [Space]
    public bool SmoothInput;
    public float SmoothingStrength = 2f;

    [Header("Current")]
    public bool IsActive;
    public float VerticalInput;
    public float HorizontalInput;
    public Quaternion TargetRotation = new();

    public ICanBeTargeted Player;
}