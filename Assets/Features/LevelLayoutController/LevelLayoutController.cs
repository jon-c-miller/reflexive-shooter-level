using UnityEngine;

public class LevelLayoutController : MonoBehaviour, IListener
{
    [SerializeField] LevelLayoutControllerV view;
    [SerializeField] LevelLayoutControllerM model = new();

    public void IOnNotify(Notifies notifyID, params object[] data)
    {
        switch (notifyID)
        {
            case Notifies.LevelLayoutRandomize:
                if (!model.EnableLayoutRandomization) return;

                for (int i = 0; i < view.LevelObstacles.Count; i++)
                {
                    view.LevelObstacles[i].IRandomizePositionBasedOnDeviationLimits();
                }
                break;
        }
    }
}