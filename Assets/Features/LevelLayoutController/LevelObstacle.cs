using UnityEngine;

public class LevelObstacle : MonoBehaviour, IObstacle
{
    [SerializeField] float deviationLimitX;
    [SerializeField] float deviationLimitZ;

    public void IRandomizePositionBasedOnDeviationLimits()
    {
        float randomX = Random.Range(-deviationLimitX, deviationLimitX) + transform.localPosition.x;
        float randomZ = Random.Range(-deviationLimitZ, deviationLimitZ) + transform.localPosition.z;
        transform.localPosition = new(randomX, transform.localPosition.y, randomZ);
    }
}