using UnityEngine;

[System.Serializable]
public class GameControllerM
{
    [Header("Configuration")]
    public bool DebugMode;
    public bool ActivatePlayerCamera;
    public bool ActivatePlayerFiring;
    public bool ActivatePlayerHitDetection;
    public bool ActivatePlayerHUD;
    public bool ActivatePlayerMovement;
    public bool ActivateAIController;
    [Space]
    public float ScreenFadeRate;

    [Header("Current")]
    public int Score;
    public int Level;
}