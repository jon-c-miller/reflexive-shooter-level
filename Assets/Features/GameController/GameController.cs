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
            case Notifies.OnUpdateScore:
                int valueChange = (int)data[0];
                model.Score += valueChange;
                NotifyHandler.N.QueueNotify(Notifies.HUDControllerUpdateScoreDisplay, model.Score);
                break;

            case Notifies.OnLevelComplete:
                Debug.Log($"Level {model.Level} complete! Starting next level...");

                // Reset player location and stats and start next level
                model.Level++;
                NotifyHandler.N.QueueNotify(Notifies.PlayerHitSetStatsBasedOnLevel, model.Level);
                NotifyHandler.N.QueueNotify(Notifies.PlayerMovementReturnToStart);
                NotifyHandler.N.QueueNotify(Notifies.AIControllerSpawnUnits, model.Level);
                NotifyHandler.N.QueueNotify(Notifies.HUDControllerUpdateUnitsRemainingDisplay, model.Level);
                break;

            case Notifies.OnLevelFailed:
                // Reset player location and stats and restart level
                NotifyHandler.N.QueueNotify(Notifies.AIControllerDisableCurrentUnits);
                NotifyHandler.N.QueueNotify(Notifies.PlayerHitSetStatsBasedOnLevel, model.Level);
                NotifyHandler.N.QueueNotify(Notifies.PlayerMovementReturnToStart);
                NotifyHandler.N.QueueNotify(Notifies.AIControllerSpawnUnits, model.Level);

                // Deduct from the score 10 per current level (keeping it at 0 or above) and update HUD
                int updatedScore = model.Score - 10 * model.Level;
                model.Score = updatedScore > 0 ? updatedScore : 0;
                NotifyHandler.N.QueueNotify(Notifies.HUDControllerUpdateScoreDisplay, model.Score);
                NotifyHandler.N.QueueNotify(Notifies.HUDControllerUpdateUnitsRemainingDisplay, model.Level);
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
            NotifyHandler.N.QueueNotify(Notifies.PlayerHitSetStatsBasedOnLevel, model.Level);
            NotifyHandler.N.QueueNotify(Notifies.PlayerHitSetActiveStatus, true);
        }
        if (model.ActivatePlayerMovement)
        {
            NotifyHandler.N.QueueNotify(Notifies.PlayerMovementReturnToStart);
            NotifyHandler.N.QueueNotify(Notifies.PlayerMovementSetActiveStatus, true);
        }
    }
}