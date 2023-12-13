using System.Collections.Generic;
using UnityEngine;

public class PlayerFiringV : MonoBehaviour
{
    [SerializeField] PlayerProjectile projectilePrefab;
    [SerializeField] Transform projectiles;
    [SerializeField] Transform reticle;

    [HideInInspector] public Transform Cam;
    
    public Transform LaunchPoint;

    Queue<PlayerProjectile> projectilePool = new();

    public void GetInput(PlayerFiringM model)
    {
        if (Input.GetKeyDown(model.LaunchKey))
        {
            LaunchProjectile(model);
        }
    }

    public void UpdateLaunchPointPosition(Vector3 newPosition, float catchupRate)
    {
        // Lerp the launch point towards the new launch position
        LaunchPoint.localPosition = Vector3.Lerp(LaunchPoint.localPosition, newPosition, catchupRate * Time.deltaTime);
    }

    public void UpdateReticlePositionAndScale(Vector3 newPosition, float newScale, float catchupRate)
    {
        // Lerp the reticle towards the new hit point position
        reticle.transform.localScale = new(newScale, newScale, newScale);
        reticle.localPosition = Vector3.Lerp(reticle.localPosition, newPosition, catchupRate * Time.deltaTime);
    }

    public void UpdateReticlePositionAndScale(Vector3 newPosition, float newScale)
    {
        // Overload to set the position directly when raycast fails to hit
        reticle.transform.localScale = new(newScale, newScale, newScale);
        reticle.localPosition = newPosition;
    }

    void LaunchProjectile(PlayerFiringM model)
    {
        // Move the fire point only when firing if desired (allows for 'drone-type ally' where firing agent is separate from player)
        if (model.DetachedFirePoint)
        {
            LaunchPoint.localPosition = model.LaunchPoint;
        }

        // Launch a projectile based on the calculated direction and the launch point's current position
		if (projectilePool.Count < 1)
        {
			AddProjectile(1);
        }
        PlayerProjectile projectile = projectilePool.Dequeue();
        projectile.Launch(model.LaunchDirection, LaunchPoint.localPosition, model.LaunchVelocity, model.Damage);
    }

	void AddProjectile(int amount)
	{
        // Instantiate and enqueue x amount of projectiles (when queue is empty or during preallocation)
		for (int i = 0; i < amount; i++)
		{
			PlayerProjectile projectile = Instantiate(projectilePrefab, Vector3.zero, Quaternion.identity, projectiles.transform);
			projectile.name = "Player Projectile";
			ReturnProjectile(projectile);
		}
	}

	void ReturnProjectile(PlayerProjectile projectile)
	{
		projectile.SetActiveStatus(false);
		projectilePool.Enqueue(projectile);
	}

	void Awake()
	{
        // Preallocate projectile pool
		AddProjectile(9);
	}

	void OnEnable()
	{
		PlayerProjectile.OnReturnPlayerProjectile += ReturnProjectile;
	}

	void OnDisable()
	{
		PlayerProjectile.OnReturnPlayerProjectile -= ReturnProjectile;
	}
}