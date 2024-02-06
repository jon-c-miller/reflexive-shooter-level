using UnityEngine;

public class LevelObstacle : MonoBehaviour, IObstacle
{
    [SerializeField] float deviationLimitX;
    [SerializeField] float deviationLimitZ;

    Vector3 originalPosition;

    public void IRandomizePositionBasedOnDeviationLimits()
    {
        float randomX = Random.Range(-deviationLimitX, deviationLimitX) + originalPosition.x;
        float randomZ = Random.Range(-deviationLimitZ, deviationLimitZ) + originalPosition.z;
        transform.localPosition = new(randomX, originalPosition.y, randomZ);
    }

    void Awake() => originalPosition = transform.localPosition;
}