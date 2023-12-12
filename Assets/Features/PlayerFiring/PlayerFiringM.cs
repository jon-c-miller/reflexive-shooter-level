using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerFiringM
{
    [Header("Configuration")]
    public float LaunchVelocity = 35f;
    public Vector3 CameraOffset = new(0.5f, 0.1f, 0.7f);
    public float FireDirectionEndDistance = 750f;
    [Space]
    public float ReticleHiddenDistance = 0.3f;
    public float ReticleHitSizeModifier = 0.01f;
    [Space]
    public List<string> ValidHitTags = new();
    [Space]
    public bool DetachedFirePoint;
    public float FirePointCatchupRate = 25f;
    [Space]
    public bool ShowDebugRayToHit;
    public bool LogHitObject;

    [Header("Keybindings")]
    public KeyCode LaunchKey = KeyCode.Mouse0;

    [Header("Current")]
    public bool IsActive;
    public Vector3 ProjectedEndPoint;
    public Vector3 LaunchPoint;

    public Vector3 LaunchDirection => (ProjectedEndPoint - LaunchPoint).normalized;
}