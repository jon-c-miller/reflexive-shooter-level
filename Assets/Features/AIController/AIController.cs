using UnityEngine;

/// <summary> Detects when player enters and exits the combat area, and manages enemy AI accordingly. </summary>
public class AIController : MonoBehaviour, IListener, IKeepsTargets
{
    [SerializeField] AIControllerV view;
    [SerializeField] AIControllerM model = new();

    public Vector3 ICurrentTargetsPosition => model.CurrentTarget != null ? model.CurrentTarget.ICurrentPosition : Vector3.zero;

    public void ILaunchAtTarget(Vector3 launchPosition)
    {
        if (model.CurrentTarget != null)
        {
            // Calculate the launch direction and launch a pooled projectile
            Vector3 launchDirection = (model.CurrentTarget.ICurrentPosition - launchPosition).normalized;

            // Nudge the launch position forward along the direction vector to avoid hitting the AI's model
            launchPosition += launchDirection * 0.5f;

            view.GetProjectile().Launch(launchDirection, launchPosition, model.AIProjectileLaunchVelocity, model.AIProjectileDamage);
        }
    }

    public void IOnNotify(Notifies notifyID, params object[] data)
    {
        switch (notifyID)
        {
            case Notifies.AIControllerSetAIActiveStatus:
                model.CurrentTarget = (ICanBeTargeted)data[0];
                model.PlayerIsInCombatArea = !model.PlayerIsInCombatArea;
                SetAIActiveStatus(model.PlayerIsInCombatArea);
                if (model.ShowLogs) Debug.Log($"Obtained target {model.CurrentTarget}. Setting AI active status to {model.PlayerIsInCombatArea}...");
                break;

            case Notifies.AIControllerSpawnUnits:
                int currentLevel = (int)data[0];
                // Set AI stats based on level, and spawn units equal to the current level
                model.RemainingUnits = currentLevel;
                model.PlayerIsInCombatArea = false;
                SetAIStatsBasedOnLevel(currentLevel);
                SpawnAI();
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
                break;
        }
    }

    void SpawnAI()
    {
        model.CurrentLevelUnits.Clear();
        // Spawn the desired amount
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
                i--;
                continue;
            }
        }
    }

    void SetAIStatsBasedOnLevel(int currentLevel)
    {
        // Set stat increases for each level beyond 1
        model.AIProjectileLaunchVelocity += 0.2f * (currentLevel - 1);
    }

    void SetAIActiveStatus(bool isActive)
    {
        for (int i = 0; i < model.CurrentLevelUnits.Count; i++)
        {
            model.CurrentLevelUnits[i].SetAIActiveStatus(isActive);
        }
    }
}