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
                view.PlayerFire.PlayOneShot(view.PlayerFireClip);
                break;
            case SoundIDs.EnemyHit:
                view.EnemyHit.PlayOneShot(view.EnemyHitClip);
                break;
            case SoundIDs.EnemyFire:
                view.EnemyFire.PlayOneShot(view.EnemyFireClip);
                break;
            case SoundIDs.LevelComplete:
                view.LevelComplete.Play();
                break;
            case SoundIDs.LevelFail:
                view.LevelFail.Play();
                break;
        }
    }
}