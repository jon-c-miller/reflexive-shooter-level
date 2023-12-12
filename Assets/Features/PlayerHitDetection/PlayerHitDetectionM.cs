using UnityEngine;

[System.Serializable]
public class PlayerHitDetectionM
{
    [Header("Configuration")]
    public int Health;

    [Header("Current")]
    public bool IsActive;

    public ICanBeTargeted Player;
}