public enum Notifies
{
    /// <summary> bool isActive </summary>
    PlayerCamSetActiveStatus,

    /// <summary> Transform camTransform </summary>
    PlayerCamAnnounceSelfTransform,

    /// <summary> n/a </summary>
    PlayerCamInitialize,

    /// <summary> bool isActive </summary>
    PlayerHitSetActiveStatus,

    /// <summary> int currentLevel </summary>
    PlayerHitSetStatsBasedOnLevel,

    /// <summary> bool isActive </summary>
    PlayerMovementSetActiveStatus,

    /// <summary> ICanBeTargeted self </summary>
    PlayerMovementAnnounceSelf,

    /// <summary> n/a </summary>
    PlayerMovementInitialize,

    /// <summary> bool isActive </summary>
    PlayerFiringSetActiveStatus,

    /// <summary> ICanBeTargeted newTarget, bool isActive </summary>
    AIControllerSetAIActiveStatus,

    /// <summary> n/a </summary>
    AIControllerDisableCurrentUnits,

    /// <summary> int currentLevel </summary>
    AIControllerSpawnUnits,

    /// <summary> bool isActive </summary>
    HUDControllerSetActiveStatus,

    /// <summary> int unitsRemaining </summary>
    HUDControllerUpdateUnitsRemainingDisplay,

    /// <summary> int healthRemaining </summary>
    HUDControllerUpdateHealthDisplay,

    /// <summary> int currentScore </summary>
    HUDControllerUpdateScoreDisplay,

    /// <summary> int currentLevel </summary>
    HUDControllerUpdateLevelDisplay,

    /// <summary> int unitsRemaining </summary>
    OnAIUnitDestroyed,

    /// <summary> n/a </summary>
    OnLevelStart,

    /// <summary> n/a </summary>
    OnLevelRestart,

    /// <summary> n/a </summary>
    OnLevelComplete,

    /// <summary> n/a </summary>
    OnLevelFailed,

    /// <summary> int valueChange </summary>
    OnUpdateScore,

    /// <summary> SoundIDs soundToPlay </summary>
    OnPlaySound,
}