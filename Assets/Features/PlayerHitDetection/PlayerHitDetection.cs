using UnityEngine;

public class PlayerHitDetection : MonoBehaviour, IListener, ICanBeHit
{
    [SerializeField] PlayerHitDetectionV view;
    [SerializeField] PlayerHitDetectionM model = new();

    public void IHit(int amount)
    {
        // Keep hit amount to within current health
        if (amount > model.Health)
        {
            amount = model.Health;
        }
        model.Health -= amount;

        // Defeat if health depleted
        if (model.Health == 0)
        {
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