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
            
            case Notifies.OnLevelStart:
                // Increase level and initialize modules
                model.Level++;
                InitializeModulesForLevelStart();
                break;

            case Notifies.OnLevelRestart:
                // Disable any remaining units before spawning new ones during module initialization
                NotifyHandler.N.QueueNotify(Notifies.AIControllerDisableCurrentUnits);

                // Deduct from the score 10 per current level (keeping it at 0 or above) and update HUD
                int updatedScore = model.Score - 10 * model.Level;
                model.Score = updatedScore > 0 ? updatedScore : 0;
                NotifyHandler.N.QueueNotify(Notifies.HUDControllerUpdateScoreDisplay, model.Score);
                InitializeModulesForLevelStart();
                break;

            case Notifies.OnLevelComplete:
                // Fade out the view and pass in a start next level delegate for when fade is complete
                Debug.Log($"Level {model.Level} complete! Starting next level...");
                NotifyHandler.N.QueueNotify(Notifies.PlayerHitSetActiveStatus, false);
                NotifyHandler.N.QueueNotify(Notifies.PlayerFiringSetActiveStatus, false);
                NotifyHandler.N.QueueNotify(Notifies.PlayerMovementSetActiveStatus, false);
                view.ScreenFader.ExecuteFade(false, model.ScreenFadeRate, OnViewFadeToTransparentNextLevel);
                break;

            case Notifies.OnLevelFailed:
                Debug.Log($"Level {model.Level} failed. Restarting level...");
                NotifyHandler.N.QueueNotify(Notifies.PlayerFiringSetActiveStatus, false);
                NotifyHandler.N.QueueNotify(Notifies.PlayerMovementSetActiveStatus, false);
                NotifyHandler.N.QueueNotify(Notifies.AIControllerSetAIActiveStatus, null, false);
                view.ScreenFader.ExecuteFade(false, model.ScreenFadeRate, OnViewFadeToTransparentRestartLevel);
                break;
        }
    }

    void InitializeModulesForLevelStart()
    {
        NotifyHandler.N.QueueNotify(Notifies.AIControllerSpawnUnits, model.Level);
        // NotifyHandler.N.QueueNotify(Notifies.PlayerFiringSetActiveStatus, true);
        NotifyHandler.N.QueueNotify(Notifies.PlayerHitSetActiveStatus, true);
        NotifyHandler.N.QueueNotify(Notifies.PlayerHitSetStatsBasedOnLevel, model.Level);
        NotifyHandler.N.QueueNotify(Notifies.PlayerMovementSetActiveStatus, true);
        NotifyHandler.N.QueueNotify(Notifies.PlayerMovementReturnToStart);
        NotifyHandler.N.QueueNotify(Notifies.HUDControllerUpdateUnitsRemainingDisplay, model.Level);
        NotifyHandler.N.QueueNotify(Notifies.HUDControllerUpdateLevelDisplay, model.Level);
    }

    void OnViewFadeToTransparentRestartLevel()
    {
        NotifyHandler.N.QueueNotify(Notifies.OnLevelRestart);
        view.ScreenFader.ExecuteFade(true, model.ScreenFadeRate, null);
    }

    void OnViewFadeToTransparentNextLevel()
    {
        NotifyHandler.N.QueueNotify(Notifies.OnLevelStart);
        view.ScreenFader.ExecuteFade(true, model.ScreenFadeRate, null);
    }

    void Start()
    {
        // Either activate specified modules (for testing, etc.), or begin the game by starting the current level
        if (model.DebugMode)
        {
            if (model.ActivateAIController)
            {
                NotifyHandler.N.QueueNotify(Notifies.AIControllerSpawnUnits, model.Level);
            }
            if (model.ActivatePlayerCamera)
            {
                NotifyHandler.N.QueueNotify(Notifies.PlayerCamSetActiveStatus, true);
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
            if (model.ActivatePlayerHUD)
            {
                NotifyHandler.N.QueueNotify(Notifies.HUDControllerSetActiveStatus, true);
                NotifyHandler.N.QueueNotify(Notifies.HUDControllerUpdateLevelDisplay, model.Level);
            }
            if (model.ActivatePlayerFiring)
            {
                NotifyHandler.N.QueueNotify(Notifies.PlayerFiringSetActiveStatus, true);
            }
        }
        else
        {
            OnViewFadeToTransparentNextLevel();
            NotifyHandler.N.QueueNotify(Notifies.HUDControllerSetActiveStatus, true);
            NotifyHandler.N.QueueNotify(Notifies.PlayerHitSetActiveStatus, true);
            NotifyHandler.N.QueueNotify(Notifies.PlayerCamSetActiveStatus, true);
            NotifyHandler.N.QueueNotify(Notifies.PlayerMovementReturnToStart);
            NotifyHandler.N.QueueNotify(Notifies.PlayerMovementSetActiveStatus, true);
        }
    }
}