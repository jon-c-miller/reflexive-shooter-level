using UnityEngine;

[System.Serializable]
public class AIUnitM
{
    [Header("Configuration")]
    public bool IsActive;
    [Space]
    public int Health = 1;
    public float AttackDelay = 1.5f;
    public float TargetVisibleCheckDelay = 0.4f;

    [Header("Current")]
    public bool TargetIsVisible;
    public float CurrentTime;

    public IKeepsTargets AIController;
}