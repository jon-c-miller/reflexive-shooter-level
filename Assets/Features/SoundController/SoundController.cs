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
                QueueSound(soundToPlay);
                break;
        }
    }

    public void QueueSound(SoundIDs id)
    {
        if (id == SoundIDs.EnemyFire)
        {
            // Increase the amount of 'queued' fire sounds if the count is less than max simultaneously queued sound count
            if (model.QueuedEnemyFireSoundsCount < model.MaxQueueCountPerSound)
                model.QueuedEnemyFireSoundsCount++;
        }
        else if (id == SoundIDs.PlayerHit)
        {
            if (model.QueuedPlayerHitSoundsCount < model.MaxQueueCountPerSound)
                model.QueuedPlayerHitSoundsCount++;
        }
        else
        {
            PlaySoundBasedOnID(id);
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

    void HandleHighFrequencySoundQueues()
    {
        if (model.QueuedEnemyFireSoundsCount > 0 && Time.frameCount % model.FrameDelayForSameSounds == 0)
        {
            // Limit playing the 'queued' sound to every x frames; ensures that sound overlapping doesn't exceed voice limit
            model.QueuedEnemyFireSoundsCount--;
            PlaySoundBasedOnID(SoundIDs.EnemyFire);
        }
        if (model.QueuedPlayerHitSoundsCount > 0 && Time.frameCount % model.FrameDelayForSameSounds == 0)
        {
            model.QueuedPlayerHitSoundsCount--;
            PlaySoundBasedOnID(SoundIDs.PlayerHit);
        }
    }

    void Update() => HandleHighFrequencySoundQueues();
}