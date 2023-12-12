using UnityEngine;

public class PlayerHitDetectionV : MonoBehaviour
{
    public Transform TargetPoint;

    public void KeepHitDetectionWithPlayer(PlayerHitDetectionM model)
    {
        if (model.Player != null)
        {
            Vector3 playerPosition = model.Player.ICurrentPosition;
            TargetPoint.localPosition = playerPosition;
        }
    }
}