using UnityEngine;

public class TargetPoint : MonoBehaviour, ICanBeTargeted
{
    public Vector3 ICurrentPosition => transform.localPosition;
}