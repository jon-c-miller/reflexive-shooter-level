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

    /// <summary> ICanBeTargeted newTarget </summary>
    AIControllerSetAIActiveStatus,

    AIControllerDisableCurrentUnits,

    /// <summary> int currentLevel </summary>
    AIControllerSpawnUnits,

    OnAIUnitDestroyed,

    OnLevelComplete,

    OnLevelFailed,
}