using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundControllerM
{
    [Header("Configuration")]
    public int FrameDelayForSameSounds = 4;
    public int MaxQueueCountPerSound = 2;

    [Header("Current")]
    public int QueuedEnemyFireSoundsCount;
    public int QueuedPlayerHitSoundsCount;
}

public enum SoundIDs
{
    PlayerHit,
    PlayerFire,
    EnemyHit,
    EnemyFire,
    LevelComplete,
    LevelFail,
}