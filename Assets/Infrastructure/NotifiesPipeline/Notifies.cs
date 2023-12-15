public enum Notifies
{
    /// <summary> bool isActive </summary>
    PlayerCamSetActiveStatus,

    /// <summary> Transform camTransform </summary>
    PlayerCamAnnounceSelfTransform,

    /// <summary> bool isActive </summary>
    PlayerHitSetActiveStatus,

    /// <summary> int currentLevel </summary>
    PlayerHitSetStatsBasedOnLevel,

    /// <summary> bool isActive </summary>
    PlayerMovementSetActiveStatus,

    /// <summary> ICanBeTargeted self </summary>
    PlayerMovementAnnounceSelf,

    PlayerMovementReturnToStart,

    /// <summary> bool isActive </summary>
    PlayerFiringSetActiveStatus,

    /// <summary> ICanBeTargeted newTarget, bool isActive </summary>
    AIControllerSetAIActiveStatus,

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

    OnLevelStart,

    OnLevelRestart,

    OnLevelComplete,

    OnLevelFailed,

    /// <summary> int valueChange </summary>
    OnUpdateScore,
}