public interface IKeepsTargets
{
    UnityEngine.Vector3 ICurrentTargetsPosition { get; }
    void ILaunchAtTarget(UnityEngine.Vector3 launchPosition);
}