using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DetectionLandEnemy : MonoBehaviour
{
    [Header("DetectionSettings")]
    [SerializeField] private float maxDistance;
    [SerializeField] private float viewAngle;
    [SerializeField] private LayerMask obstructionMask;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (EnemyManager.flyEnemiesList != null)
        //{
        //    Debug.Log("Fly enemy list is not null: " + EnemyManager.flyEnemiesList.Count);
        //}

        LandEnemy closestLandEnemy = FindClosestEnemy(EnemyManager.landEnemiesList);
        FlyEnemy closestFlyEnemy = FindClosestEnemy(EnemyManager.flyEnemiesList);
        /*if (closestLandEnemy != null)
        {
            Debug.Log("Closest enemy is " + closestLandEnemy.name);
        }*/

        // Update UI
        Vector3 playerPos = transform.position;
        float? distanceLand = null;
        if (closestLandEnemy != null)
        {
            distanceLand = Vector3.Distance(playerPos, closestLandEnemy.transform.position);
        }

        float? distanceFly = null;
        if (closestFlyEnemy != null)
        {
            distanceFly = Vector3.Distance(playerPos, closestFlyEnemy.transform.position);
        }

        if (distanceLand.HasValue && distanceFly.HasValue)
        {
            if (distanceFly < distanceLand)
            {
                UIController.instance.UpdateEnemyDisplay(closestFlyEnemy);
            }
            else
            {
                UIController.instance.UpdateEnemyDisplay(closestLandEnemy);
            }
        }
        else
        {
            if (distanceLand.HasValue)
            {
                UIController.instance.UpdateEnemyDisplay(closestLandEnemy);
            }
            else if (distanceFly.HasValue)
            {
                UIController.instance.UpdateEnemyDisplay(closestFlyEnemy);
            }
            else
            {
                UIController.instance.UpdateEnemyDisplay<EnemyManager>(null);
            }
        }
    }

    T FindClosestEnemy<T>(List<T> enemies) where T : EnemyManager
    {
        T closest = null;
        float minAngle = float.MaxValue; // Can find any value that less than it
        Vector3 playerPos = transform.position;

        foreach (T enemy in enemies)
        {
            Vector3 enemyPos = enemy.transform.position;
            Vector3 dirToEnemy = (enemyPos - playerPos).normalized; // Get direction that player to the enemy
            Debug.DrawLine(playerPos, enemyPos);

            float angle = Vector3.Angle(PlayerController.instance.playerForward, dirToEnemy);
            float distance = Vector3.Distance(playerPos, enemyPos);

            // 输出敌人信息
            /*Debug.Log($"检测敌人: {enemy.name}\n" +
                      $"角度: {angle}°\n" +
                      $"距离: {distance}m\n" +
                      $"是否在视野内: {angle < viewAngle / 2}\n" +
                      $"是否在距离内: {distance < maxDistance}");*/

            // Angle and distance are in range
            if (angle < viewAngle / 2 && distance < maxDistance)
            {
                // No obstruction
                if (!IsObstructed(playerPos, enemyPos))
                {
                    // Get the closest
                    if (angle < minAngle)
                    {
                        minAngle = angle;
                        closest = enemy;
                    }
                }
            }
        }
        return closest;
    }

    bool IsObstructed(Vector3 start, Vector3 end)
    {
        return Physics2D.Linecast(start, end, obstructionMask, maxDistance);
    }
}
