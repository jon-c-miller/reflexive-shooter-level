using UnityEngine;

[System.Serializable]
public class GameControllerM
{
    [Header("Configuration")]
    public bool ActivatePlayerCamera;
    public bool ActivatePlayerFiring;
    public bool ActivatePlayerHitDetection;
    public bool ActivatePlayerHUD;
    public bool ActivatePlayerMovement;
    [Space]
    public bool ActivateAIController;

    [Header("Current")]
    public int Score;
    public int Level;
}