using UnityEngine;

public class LevelLayoutController : MonoBehaviour, IListener
{
    [SerializeField] LevelLayoutControllerV view;
    [SerializeField] LevelLayoutControllerM model = new();

    delegate void ChainLinkDelegate();
    ChainLinkDelegate chainLink;

    public void IOnNotify(Notifies notifyID, params object[] data)
    {
        switch (notifyID)
        {
            case Notifies.LevelLayoutRandomize:
                if (model.EnableLevelRotation)
                {
                    // Rotate the level in multiples of 90 degrees randomly
                    int rotationTurns = Random.Range(0, 4);
                    int randomYRotation = 90 * rotationTurns;
                    view.transform.localRotation = Quaternion.Euler(new Vector3(0, randomYRotation, 0));
                }

                if (model.EnableLayoutRandomization)
                {
                    for (int i = 0; i < view.LevelObstacles.Count; i++)
                    {
                        view.LevelObstacles[i].IRandomizePositionBasedOnDeviationLimits();
                    }
                }
                break;
        }
    }
}