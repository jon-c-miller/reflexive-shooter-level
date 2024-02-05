using UnityEngine;

/// <summary> Detects when player enters and exits the combat area, and manages enemy AI accordingly. </summary>
public class AIController : MonoBehaviour, IListener, ILaunchesProjectiles
{
    [SerializeField] AIControllerV view;
    [SerializeField] AIControllerM model = new();
    
    const string PLAYERTAG = "Player";

    public void ILaunchAtTarget(Vector3 launchPosition)
    {
        if (model.CurrentTarget != null)
        {
            // Calculate the launch direction and launch a pooled projectile
            Vector3 launchDirection = (model.CurrentTarget.ICurrentPosition - launchPosition).normalized;

            // Nudge the launch position forward along the direction vector to avoid hitting the AI's model
            launchPosition += launchDirection * 0.5f;

            view.GetProjectile().Launch(launchDirection, launchPosition, model.AIProjectileLaunchVelocity, model.AIProjectileDamage);
            
            // Play enemy fire sound
            NotifyHandler.N.QueueNotify(Notifies.PlaySound, SoundIDs.EnemyFire);
        }
    }

    public bool ICheckTargetVisible(Vector3 originPosition)
    {
        // Try to get the target's current position
        Vector3 targetPosition = model.CurrentTarget != null ? model.CurrentTarget.ICurrentPosition : Vector3.zero;

        // Fire raycast to see if target in line of sight, setting TargetIsVisible based on whether hit player tag or not
        Vector3 direction = targetPosition - originPosition;

        // Nudge the from position forward along the direction vector to avoid hitting the AI's model
        Vector3 from = originPosition + (direction.normalized * 0.5f);

        // Display the raycast to the player if desired
        if (model.ShowFireTraceFromAIUnits)
        {
            Debug.DrawRay(from, direction, Color.red);
        }

        // Try to detect the player via raycast
        if (Physics.Raycast(from, direction, out RaycastHit hit))
        {
            return hit.collider.CompareTag(PLAYERTAG);
        }
        else
        {
            return false;
        }
    }

    public void IOnNotify(Notifies notifyID, params object[] data)
    {
        switch (notifyID)
        {
            case Notifies.AIControllerSetAIActiveStatus:
                bool isActive = (bool)data[0];

                // Ignore attempts to update AI status without any change from true to false
                if (model.AIIsActive == isActive)
                {
                    return;
                }

                model.AIIsActive = isActive;
                SetAIActiveStatus(isActive);
                if (model.ShowLogs) Debug.Log($"Obtained target {model.CurrentTarget}. Setting AI active status to {isActive}...");
                break;

            case Notifies.AIControllerSetTarget:
                model.CurrentTarget = (ICanBeTargeted)data[0];
                break;

            case Notifies.AIControllerSpawnUnits:
                int currentLevel = (int)data[0];
                // Set AI stats based on level, and spawn units equal to the current level
                model.RemainingUnits = currentLevel;
                model.AIIsActive = false;
                SetAIStatsBasedOnLevel(currentLevel);
                SpawnAI();
                // NotifyHandler.N.QueueNotify(Notifies.OnAICountUpdated, model.RemainingUnits);
                break;

            case Notifies.AIControllerDisableCurrentUnits:
                for (int i = 0; i < model.CurrentLevelUnits.Count; i++)
                {
                    model.CurrentLevelUnits[i].RecycleUnit();
                }
                break;

            case Notifies.OnAIUnitDestroyed:
                model.RemainingUnits--;
                if (model.RemainingUnits <= 0)
                {
                    NotifyHandler.N.QueueNotify(Notifies.OnLevelComplete);
                }

                // Notify of updated score and units remaining
                NotifyHandler.N.QueueNotify(Notifies.OnAICountUpdated, model.RemainingUnits);
                NotifyHandler.N.QueueNotify(Notifies.UpdateScore, model.UnitScoreValue);
                break;
        }
    }

    void SpawnAI()
    {
        model.CurrentLevelUnits.Clear();
        // Spawn the desired amount
        int tries = 999;
        for (int i = 0; i < model.RemainingUnits; i++)
        {
            if (model.ShowLogs) Debug.Log($"Attempting to spawn...");
            Vector3 spawnPosition = view.SpawnCenter.localPosition;

            // Get a random position based on deviation values
            spawnPosition.x += Random.Range(-model.SpawnRangeValueLimits.x, model.SpawnRangeValueLimits.x);
            spawnPosition.y += Random.Range(-model.SpawnRangeValueLimits.y, model.SpawnRangeValueLimits.y);
            spawnPosition.z += Random.Range(-model.SpawnRangeValueLimits.z, model.SpawnRangeValueLimits.z);

            if (model.ShowLogs) Debug.Log($"Found a random spawn position at {spawnPosition}.");

            // Check for collision at the intended spawn point, retrying if there was a collision
            if (!Physics.CheckSphere(spawnPosition, 1))
            {
                if (model.ShowLogs) Debug.Log($"Spawning unit...");
                AIUnit unit = view.GetAIUnit();
                model.CurrentLevelUnits.Add(unit);
                unit.Initialize(this, spawnPosition);
                unit.SetStatsBasedOnLevel(model.RemainingUnits);
            }
            else
            {
                // Ignore the current attempt if the spawn failed (and prevent an endless loop)
                i--;
                tries--;
                if (tries < 1) break;
                else continue;
            }
        }
    }

    void SetAIStatsBasedOnLevel(int currentLevel)
    {
        // Set stat increases for each level beyond 1
        model.AIProjectileLaunchVelocity = 15f + 0.2f * (currentLevel - 1);
        model.UnitScoreValue = 5 + 1 * (currentLevel - 1);
    }

    void SetAIActiveStatus(bool isActive)
    {
        for (int i = 0; i < model.CurrentLevelUnits.Count; i++)
        {
            model.CurrentLevelUnits[i].SetAIActiveStatus(isActive);
        }
    }
}