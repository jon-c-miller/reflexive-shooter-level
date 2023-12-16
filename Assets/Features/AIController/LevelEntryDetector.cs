using UnityEngine;

public class LevelEntryDetector : MonoBehaviour
{
    [SerializeField] bool collisionEnablesAI;

    void OnTriggerEnter(Collider other)
    {
        // Notify AIController to update its target and AI active status when player enters trigger area
        // Physics layer collision is limited to Player layer only, making tag checking redundant
        other.TryGetComponent(out ICanBeTargeted target);

        if (collisionEnablesAI)
        {
            NotifyHandler.N.QueueNotify(Notifies.AIControllerSetAIActiveStatus, target, true);
            NotifyHandler.N.QueueNotify(Notifies.OnEnterCombatArea);
        }
        else
        {
            NotifyHandler.N.QueueNotify(Notifies.OnExitCombatArea);
        }
    }
}