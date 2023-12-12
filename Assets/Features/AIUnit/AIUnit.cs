using UnityEngine;

/// <summary> Holds an individual AI unit's logic and state. </summary>
public class AIUnit : MonoBehaviour, ICanBeHit
{
    [SerializeField] AIUnitV view;
    [SerializeField] AIUnitM model = new();

	public static event System.Action<AIUnit> OnReturnAIUnit;

    const string PLAYERTAG = "Player";

    public void IHit(int amount)
    {
        // Keep hit amount to within current health
        if (amount > model.Health)
        {
            amount = model.Health;
        }
        model.Health -= amount;

        // Return unit if health depleted
        if (model.Health == 0)
        {
            OnReturnAIUnit?.Invoke(this);
        }
    }

    public void SetActiveStatus(bool isActive)
    {
        model.IsActive = isActive;
        gameObject.SetActive(isActive);
    }

    public void Initialize(IKeepsTargets controller, Vector3 position)
    {
        model.AttackDelay = 1.5f;
        model.ScoreValue = 5;
        model.CurrentTime = model.TargetVisibleCheckDelay;
        model.AIController = controller;
        view.Model.transform.localPosition = position;

        gameObject.SetActive(true);
        Debug.Log($"{this.name} initialized.");
    }

    public void SetStatsBasedOnLevel(int currentLevel)
    {
        model.AttackDelay -= 0.03f * currentLevel;
        model.ScoreValue += 5;
    }

    void CheckPlayerVisible()
    {
        if (model.AIController != null)
        {
            // Fire raycast to see if player in line of sight, setting TargetIsVisible based on whether hit player tag or not
            Vector3 direction = model.AIController.ICurrentTargetsPosition - view.Model.position;

            // Nudge the from position forward along the direction vector to avoid hitting the AI's model
            Vector3 from = view.Model.position + (direction.normalized * 0.5f);

            Debug.DrawRay(from, direction, Color.red);
            if (Physics.Raycast(from, direction, out RaycastHit hit))
            {
                model.TargetIsVisible = hit.collider.CompareTag(PLAYERTAG);
            }
        }

        model.CurrentTime = model.TargetVisibleCheckDelay;
    }

    void LaunchAtPlayer()
    {
        if (model.TargetIsVisible)
        {
            // Fire projectile if controller valid and player still visible, and reset the attack delay
            model.AIController?.ILaunchAtTarget(view.Model.position);
            model.CurrentTime = model.AttackDelay;
        }
        else
        {
            // Otherwise transition back to visible checking be updating the delay
            model.CurrentTime = model.TargetVisibleCheckDelay;
        }
    }

    void Update()
    {
        if (!model.IsActive)
        {
            return;
        }

        // Repeatedly fire at player as long as it is visible
        if (model.TargetIsVisible)
        {
            if (model.CurrentTime < 0)
            {
                CheckPlayerVisible();
                LaunchAtPlayer();
            }
            model.CurrentTime -= Time.deltaTime;
            return;
        }

        // Check for player visibility if not visible
        if (model.CurrentTime < 0)
        {
            CheckPlayerVisible();
        }
        model.CurrentTime -= Time.deltaTime;
    }
}