public enum Notifies
{
    /// <summary> bool isActive </summary>
    PlayerCamSetActiveStatus,

    /// <summary> Transform camTransform </summary>
    PlayerCamAnnounceSelfTransform,

    /// <summary> bool isActive </summary>
    PlayerMovementSetActiveStatus,

    /// <summary> ICanBeTargeted self </summary>
    PlayerMovementAnnounceSelf,

    /// <summary> bool isActive </summary>
    PlayerFiringSetActiveStatus,

    /// <summary> ICanBeTargeted newTarget </summary>
    AIControllerSetAIActiveStatus,

    AIControllerSpawnUnits,

    OnLevelComplete,

    OnLevelFailed,
}