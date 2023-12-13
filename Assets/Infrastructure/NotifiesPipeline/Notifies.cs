public enum Notifies
{
    /// <summary> bool isActive </summary>
    PlayerCamSetActiveStatus,

    /// <summary> Transform camTransform </summary>
    PlayerCamAnnounceSelfTransform,

    /// <summary> bool isActive </summary>
    PlayerHitSetActiveStatus,

    /// <summary> bool isActive </summary>
    PlayerMovementSetActiveStatus,

    /// <summary> ICanBeTargeted self </summary>
    PlayerMovementAnnounceSelf,

    PlayerMovementReturnToStart,

    /// <summary> bool isActive </summary>
    PlayerFiringSetActiveStatus,

    /// <summary> ICanBeTargeted newTarget </summary>
    AIControllerSetAIActiveStatus,

    /// <summary> int currentLevel </summary>
    AIControllerSpawnUnits,

    OnAIUnitDestroyed,

    OnLevelComplete,

    OnLevelFailed,
}