using UnityEngine;

public class SoundController : MonoBehaviour, IListener
{
    [SerializeField] SoundControllerV view;
    [SerializeField] SoundControllerM model = new();

    public void IOnNotify(Notifies notifyID, params object[] data)
    {
        switch (notifyID)
        {
            case Notifies.PlaySound:
                SoundIDs soundToPlay = (SoundIDs)data[0];
                PlaySoundBasedOnID(soundToPlay);
                break;
        }
    }

    void PlaySoundBasedOnID(SoundIDs soundToPlay)
    {
        switch (soundToPlay)
        {
            case SoundIDs.PlayerHit:
                view.PlayerHit.PlayOneShot(view.PlayerHitClip);
                break;
            case SoundIDs.PlayerFire:
                break;
            case SoundIDs.EnemyHit:
                break;
            case SoundIDs.EnemyFire:
                break;
        }
    }
}