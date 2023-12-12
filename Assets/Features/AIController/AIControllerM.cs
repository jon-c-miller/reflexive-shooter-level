using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AIControllerM
{
    [Header("Configuration")]
    public int SpawnCount = 1;
    public float AIProjectileLaunchVelocity = 15f;
    public int AIProjectileDamage = 1;
    public Vector3 SpawnRangeValueLimits = new();
    [Space]

    [Header("Current")]
    public int CurrentLevel;
    public bool PlayerIsInCombatArea;
    public List<AIUnit> AIUnits = new();

    public ICanBeTargeted CurrentTarget;
}