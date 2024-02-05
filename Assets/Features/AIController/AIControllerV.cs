using System.Collections.Generic;
using UnityEngine;

public class AIControllerV : MonoBehaviour
{
    [SerializeField] AIProjectile projectilePrefab;
    [SerializeField] Transform projectiles;
    [Space]
    [SerializeField] AIUnit unitPrefab;
    [SerializeField] Transform units;
    [Space]
    public Transform SpawnCenter;
	public GameObject NoSpawnArea;
    
    Queue<AIProjectile> projectilePool = new();
    Queue<AIUnit> unitPool = new();

    public AIProjectile GetProjectile()
    {
		if (projectilePool.Count < 1)
        {
			AddProjectile(1);
        }

        return projectilePool.Dequeue();
    }

    public AIUnit GetAIUnit()
    {
		if (unitPool.Count < 1)
        {
			AddUnit(1);
        }

        return unitPool.Dequeue();
    }

	void AddProjectile(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			AIProjectile projectile = Instantiate(projectilePrefab, Vector3.zero, Quaternion.identity, projectiles.transform);
			projectile.name = "AI Projectile";
			ReturnProjectile(projectile);
		}
	}

    void AddUnit(int amount)
    {
		for (int i = 0; i < amount; i++)
		{
			AIUnit unit = Instantiate(unitPrefab, Vector3.zero, Quaternion.identity, units.transform);
			unit.name = "AI Unit";
			ReturnUnit(unit);
		}
    }

	void ReturnProjectile(AIProjectile projectile)
	{
		projectile.SetActiveStatus(false);
        projectile.transform.localPosition = Vector3.zero;
		projectilePool.Enqueue(projectile);
	}

    void ReturnUnit(AIUnit unit)
    {
		unit.SetAIActiveStatus(false);
		unit.SetObjectActiveStatus(false);
		unitPool.Enqueue(unit);
    }

	void Awake()
	{
        // Preallocate pools
		AddProjectile(9);
        AddUnit(9);
	}

	void OnEnable()
	{
		AIProjectile.OnReturnAIProjectile += ReturnProjectile;
        AIUnit.OnReturnAIUnit += ReturnUnit;
	}

	void OnDisable()
	{
		AIProjectile.OnReturnAIProjectile -= ReturnProjectile;
        AIUnit.OnReturnAIUnit -= ReturnUnit;
	}
}