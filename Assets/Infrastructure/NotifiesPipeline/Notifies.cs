public enum Notifies
{
    /// <summary> bool isActive </summary>
    AIControllerSetAIActiveStatus,

    /// <summary> bool isActive </summary>
    HUDControllerSetActiveStatus,

    /// <summary> bool isActive </summary>
    PlayerCamSetActiveStatus,

    /// <summary> bool isActive </summary>
    PlayerFiringSetActiveStatus,

    /// <summary> bool isActive </summary>
    PlayerMovementSetActiveStatus,


    /// <summary> ICanBeTargeted newTarget </summary>
    AIControllerSetTarget,

    /// <summary> Transform camTransform </summary>
    PlayerCamAnnounceSelfTransform,

    /// <summary> n/a </summary>
    PlayerCamInitialize,

    /// <summary> bool isActive </summary>
    PlayerHitSetActiveStatus,

    /// <summary> int currentLevel </summary>
    PlayerHitSetStatsBasedOnLevel,

    /// <summary> ICanBeTargeted self </summary>
    PlayerMovementAnnounceSelf,

    /// <summary> n/a </summary>
    PlayerMovementInitialize,

    /// <summary> n/a </summary>
    AIControllerDisableCurrentUnits,

    /// <summary> int currentLevel </summary>
    AIControllerSpawnUnits,


    /// <summary> SoundIDs soundToPlay </summary>
    PlaySound,

    /// <summary> int valueChange </summary>
    UpdateScore,


    /// <summary> int unitsRemaining </summary>
    OnAICountUpdated,

    /// <summary> int unitsRemaining </summary>
    OnAIUnitDestroyed,

    /// <summary> n/a </summary>
    OnEnterCombatArea,

    /// <summary> n/a </summary>
    OnExitCombatArea,

    /// <summary> n/a </summary>
    OnLevelStart,

    /// <summary> n/a </summary>
    OnLevelRestart,

    /// <summary> n/a </summary>
    OnLevelComplete,

    /// <summary> n/a </summary>
    OnLevelFailed,

    /// <summary> int healthRemaining </summary>
    OnPlayerHealthUpdated,

    /// <summary> int newScore </summary>
    OnScoreDisplayUpdated,

    /// <summary> int currentLevel </summary>
    OnLevelDisplayUpdated,
}