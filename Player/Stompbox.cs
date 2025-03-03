using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stompbox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "LandEnemy")
        {
            // Debug.Log("LandEnemy");
            HandleEnemyTrigger<LandEnemy>(other);
        }
        else if (other.tag == "FlyEnemy")
        {
            HandleEnemyTrigger<FlyEnemy>(other);
        }
        else if (other.tag == "BossTank")
        {
            HandleEnemyTrigger<TankController>(other);
        }
    }

    private void HandleEnemyTrigger<T>(Collider2D other) where T : EnemyManager
    {
        T enemy = other.GetComponentInParent<T>(); // Get the parent of the enemy
        if (enemy != null && enemy.gameObject.activeInHierarchy) // Check if the enemy is not null and active
        {
            enemy.health--;
            // Debug.Log("Enemy Health: " + enemy.health);
            enemy.HealthManager(enemy);

            // Restore the player's jump count when stom
            if (typeof(T) == typeof(FlyEnemy))
            {
                PlayerController.instance.jumpCount = 2;
                // Debug.Log("Player Jump Count: " + PlayerController.instance.jumpCount);
            }

            if (typeof(T) == typeof(TankController))
            {
                other.GetComponentInParent<TankController>().currentState = TankController.TankState.Hurt;
            }
        }
        else
        {
            Destroy(enemy.gameObject); // Destroy the enemy
        }
        // Player will jump higher when stomping on the enemy
        PlayerController.instance.KnockBack_Enemy<T>(enemy);
    }
}
