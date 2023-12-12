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
            case Notifies.OnLevelComplete:
                // Set the spawn count to be the same as the next level value, and upgrade all AI units
                model.CurrentLevel++;
                model.SpawnCount = model.CurrentLevel;
                UpgradeAIBaseStats();
                break;

            case Notifies.AIControllerSetAIActiveStatus:
                model.CurrentTarget = (ICanBeTargeted)data[0];
                model.PlayerIsInCombatArea = !model.PlayerIsInCombatArea;
                SetAIActiveStatus(model.PlayerIsInCombatArea);
                Debug.Log($"Obtained target {model.CurrentTarget}. Setting AI active status to {model.PlayerIsInCombatArea}...");
                break;

            case Notifies.AIControllerSpawnUnits:
                SpawnAI();
                break;
        }
    }

    void SpawnAI()
    {
        model.AIUnits.Clear();
        for (int i = 0; i < model.SpawnCount; i++)
        {
            Debug.Log($"Attempting to spawn...");
            Vector3 spawnPosition = transform.localPosition;

            // Get a random position based on deviation values
            spawnPosition.x += Random.Range(-model.SpawnRangeValueLimits.x, model.SpawnRangeValueLimits.x);
            spawnPosition.y += Random.Range(-model.SpawnRangeValueLimits.y, model.SpawnRangeValueLimits.y);
            spawnPosition.z += Random.Range(-model.SpawnRangeValueLimits.z, model.SpawnRangeValueLimits.z);

            Debug.Log($"Found a random spawn position at {spawnPosition}.");

            // Check for collision at the intended spawn point, retrying if there was a collision
            if (!Physics.CheckSphere(spawnPosition, 1))
            {
                Debug.Log($"Spawning unit...");
                AIUnit unit = view.GetAIUnit();
                model.AIUnits.Add(unit);
                unit.Initialize(this, spawnPosition);
                unit.SetStatsBasedOnLevel(model.CurrentLevel);
                // unit.SetActiveStatus(true);
            }
            else
            {
                i--;
                continue;
            }
        }
    }

    void UpgradeAIBaseStats()
    {
        model.AIProjectileDamage += 1;
        model.AIProjectileLaunchVelocity += 0.2f;
    }

    void SetAIActiveStatus(bool isActive)
    {
        for (int i = 0; i < model.AIUnits.Count; i++)
        {
            model.AIUnits[i].SetActiveStatus(isActive);
        }
    }

    void Start()
    {
        NotifyHandler.N.QueueNotify(Notifies.AIControllerSpawnUnits);
    }
}