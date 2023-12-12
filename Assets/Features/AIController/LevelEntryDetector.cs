using UnityEngine;

public class LevelEntryDetector : MonoBehaviour
{
    const string PLAYERTAG = "Player";

    void OnTriggerExit(Collider other)
    {
        // Notify AIController to update its target and reverse AI active status when player exits (passes through) trigger area
        Debug.Log($"AIControllerV: {other.name} exited collider.");
        if (other.CompareTag(PLAYERTAG))
        {
            Debug.Log($"AIControllerV: setting {other.name} as target...");
            other.TryGetComponent(out ICanBeTargeted target);
            NotifyHandler.N.QueueNotify(Notifies.AIControllerSetAIActiveStatus, target);
        }
    }
}