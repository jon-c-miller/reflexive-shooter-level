using UnityEngine;

public class SoundController : MonoBehaviour, IListener
{
    [SerializeField] SoundControllerV view;
    [SerializeField] SoundControllerM model = new();

    public void IOnNotify(Notifies notifyID, params object[] data)
    {
        switch (notifyID)
        {
            case Notifies.SoundControllerOnPlayerHit:
                view.PlayerHit.PlayOneShot(view.PlayerHitClip);
                break;
        }
    }
}