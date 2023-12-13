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
                model.Level++;

                // Reset player location and start next level
                
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