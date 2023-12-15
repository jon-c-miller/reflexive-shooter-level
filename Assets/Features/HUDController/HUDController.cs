using UnityEngine;

/// <summary> Handles displaying onscreen feedback related to player status and current level progress. </summary>
public class HUDController : MonoBehaviour, IListener
{
    [SerializeField] HUDControllerV view;
    [SerializeField] HUDControllerM model = new();

    public void IOnNotify(Notifies notifyID, params object[] data)
    {
        switch (notifyID)
        {
            case Notifies.HUDControllerSetActiveStatus:
                bool isActive = (bool)data[0];
                view.SetUIActiveStatus(isActive);
                view.SetTextSizeAndColor(model);
                break;

            case Notifies.OnScoreDisplayUpdated:
                int currentScore = (int)data[0];
                view.UpdateScoreDisplay(currentScore);
                break;

            case Notifies.OnAICountUpdated:
                int unitsRemaining = (int)data[0];
                view.UpdateUnitsRemainingDisplay(unitsRemaining);
                break;

            case Notifies.OnPlayerHealthUpdated:
                int healthRemaining = (int)data[0];
                view.UpdateHealthDisplay(healthRemaining);
                break;

            case Notifies.OnLevelDisplayUpdated:
                int currentLevel = (int)data[0];
                view.UpdateLevelDisplay(currentLevel);
                break;
        }
    }
}