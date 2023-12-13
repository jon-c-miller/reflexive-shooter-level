using UnityEngine;

/// <summary> Handles coordinating game flow and activating each module. </summary>
public class GameController : MonoBehaviour, IListener
{
    [SerializeField] GameControllerV view;
    [SerializeField] GameControllerM model;

    public void IOnNotify(Notifies notifyID, params object[] data)
    {
        switch (notifyID)
        {
            case Notifies.OnLevelComplete:
                Debug.Log($"Level {model.Level} complete! Starting next level...");

                // Reset player location and start next level
                model.Level++;
                NotifyHandler.N.QueueNotify(Notifies.PlayerMovementReturnToStart);
                NotifyHandler.N.QueueNotify(Notifies.AIControllerSpawnUnits, model.Level);
                break;

            case Notifies.OnLevelFailed:
                // Reset player location and restart level

                break;
        }
    }

    void Start()
    {
        if (model.ActivateAIController)
        {
            NotifyHandler.N.QueueNotify(Notifies.AIControllerSpawnUnits, model.Level);
        }
        if (model.ActivatePlayerCamera)
        {
            NotifyHandler.N.QueueNotify(Notifies.PlayerCamSetActiveStatus, true);
        }
        if (model.ActivatePlayerFiring)
        {
            NotifyHandler.N.QueueNotify(Notifies.PlayerFiringSetActiveStatus, true);
        }
        if (model.ActivatePlayerHitDetection)
        {
            NotifyHandler.N.QueueNotify(Notifies.PlayerHitSetActiveStatus, true);
        }
        if (model.ActivatePlayerMovement)
        {
            NotifyHandler.N.QueueNotify(Notifies.PlayerMovementReturnToStart);
            NotifyHandler.N.QueueNotify(Notifies.PlayerMovementSetActiveStatus, true);
        }
    }
}