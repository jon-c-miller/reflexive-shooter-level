using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelLayoutControllerV : MonoBehaviour
{
    [SerializeField] GameObject[] obstacleObjects;

    public IObstacle[] LevelObstacles;

    void Awake()
    {
        // Get the IObstacle interface from the obstacle objects collection
        for (int i = 0; i < obstacleObjects.Length; i++)
        {
            if (obstacleObjects[i].TryGetComponent(out IObstacle obstacleInterface))
            {
                LevelObstacles.Append(obstacleInterface);
            }
        }
    }
}