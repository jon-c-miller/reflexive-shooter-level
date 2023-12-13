using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AIControllerM
{
    [Header("Configuration")]
    public float AIProjectileLaunchVelocity = 15f;
    public int AIProjectileDamage = 1;
    public int UnitScoreValue = 5;
    public Vector3 SpawnRangeValueLimits = new();
    [Space]
    public bool ShowLogs;

    [Header("Current")]
    public int RemainingUnits = 1;
    public bool PlayerIsInCombatArea;
    public List<AIUnit> CurrentLevelUnits = new();

    public ICanBeTargeted CurrentTarget;
}