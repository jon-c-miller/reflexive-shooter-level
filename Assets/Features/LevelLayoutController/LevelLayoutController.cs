using System.Collections.Generic;
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
                for (int i = 0; i < view.LevelObstacles.Length; i++)
                {
                    view.LevelObstacles[i].IRandomizePositionBasedOnDeviationLimits();
                }
                break;
        }
    }
}