public interface ILaunchesProjectiles
{
    bool ICheckTargetVisible(UnityEngine.Vector3 originPosition);
    void ILaunchAtTarget(UnityEngine.Vector3 launchPosition);
}