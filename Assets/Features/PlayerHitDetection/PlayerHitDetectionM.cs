using UnityEngine;

[System.Serializable]
public class PlayerHitDetectionM
{
    [Header("Configuration")]
    public int Health = 3;

    [Header("Current")]
    public bool IsActive;

    public ICanBeTargeted Player;
}