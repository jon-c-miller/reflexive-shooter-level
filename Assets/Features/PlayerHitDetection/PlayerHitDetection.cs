using UnityEngine;

/// <summary> Handles providing hit detection for incoming AI projectiles and combat area entry/exit. </summary>
public class PlayerHitDetection : MonoBehaviour, IListener, ICanBeHit
{
    [SerializeField] PlayerHitDetectionV view;
    [SerializeField] PlayerHitDetectionM model = new();

    public void IHit(int amount)
    {
        if (!model.IsActive)
        {
            return;
        }

        // Keep hit amount to within current health
        if (amount > model.Health)
        {
            amount = model.Health;
        }
        model.Health -= amount;

        // Notify the HUD of updated health and play hit sound
        NotifyHandler.N.QueueNotify(Notifies.HUDControllerUpdateHealthDisplay, model.Health);
        NotifyHandler.N.QueueNotify(Notifies.OnPlaySound, SoundIDs.PlayerHit);

        // Defeat if health depleted
        if (model.Health == 0)
        {
            model.IsActive = false;
            NotifyHandler.N.QueueNotify(Notifies.OnLevelFailed);
        }
    }

    public void IOnNotify(Notifies notifyID, params object[] data)
    {
        switch (notifyID)
        {
            case Notifies.PlayerHitSetActiveStatus:
                bool isActive = (bool)data[0];
                model.IsActive = isActive;
                break;

            case Notifies.PlayerMovementAnnounceSelf:
                model.Player = (ICanBeTargeted)data[0];
                break;

            case Notifies.PlayerHitSetStatsBasedOnLevel:
                int currentLevel = (int)data[0];
                // Grant a base health of 3, plus 1 health for every 2 levels reached
                model.Health = 3 + (currentLevel / 2);
                NotifyHandler.N.QueueNotify(Notifies.HUDControllerUpdateHealthDisplay, model.Health);
                break;
        }
    }

    void Update()
    {
        if (model.IsActive)
        {
            view.KeepHitDetectionWithPlayer(model);
        }
    }
}