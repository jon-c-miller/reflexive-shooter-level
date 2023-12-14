using UnityEngine;

public class LevelEntryDetector : MonoBehaviour
{
    [SerializeField] bool collisionEnablesAI;

    void OnTriggerEnter(Collider other)
    {
        // Notify AIController to update its target and AI active status when player enters trigger area
        // Physics layer collision is limited to Player layer only, making tag checking redundant
        other.TryGetComponent(out ICanBeTargeted target);
        NotifyHandler.N.QueueNotify(Notifies.AIControllerSetAIActiveStatus, target, collisionEnablesAI);

        // Enable player firing ability based on combat entry or exit
        NotifyHandler.N.QueueNotify(Notifies.PlayerFiringSetActiveStatus, collisionEnablesAI);
        Debug.Log($"AIControllerV: {other.name} entered collider.");
    }
}