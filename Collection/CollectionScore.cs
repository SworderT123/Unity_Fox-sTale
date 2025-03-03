using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCollection : MonoBehaviour
{
    public bool isGem; // Score up
    public bool isCherry; // Health up

    public GameObject pickupEffect; // Effect when collected

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
        if (other.tag == "Player")
        {
            if (isGem)
            {
                LevelManager.instance.gemsCollectedCount++;
                // Save the number of gems collected in the current level
                PlayerPrefs.SetInt(LevelManager.instance.currentLevel + "GemsCollected", LevelManager.instance.gemsCollectedCount);

                GlobalUI.instance.gemsCountInTotal++;
                Destroy(gameObject);
                UIController.instance.UpdateGemsDisplay();

                Instantiate(pickupEffect, transform.position, transform.rotation); // Create the effect when collected
                AudioEffect.instance.PlayAudio(6); // Play the audio effect
            }
            else if (isCherry)
            {
                LevelManager.instance.cherriesCollectedCount++;
                Destroy(gameObject);

                if (LevelManager.instance.cherriesCollectedCount == 3)
                {
                    if (PlayerHealthController.instance.currentHealth < GlobalUI.instance.maxHP)
                    {
                        LevelManager.instance.cherriesCollectedCount = 0;
                        GlobalUI.instance.currentHP++;
                        PlayerPrefs.SetInt("CurrentHP", GlobalUI.instance.currentHP);
                        PlayerHealthController.instance.currentHealth = GlobalUI.instance.currentHP;
                        UIController.instance.UpdateHealthDisplay();
                    }
                }

                Instantiate(pickupEffect, transform.position, transform.rotation);
                AudioEffect.instance.PlayAudio(7);
            }
        }
    }
}
