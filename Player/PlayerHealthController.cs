using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;// create the singleton instance to ensure one class just has one instance

    public int currentHealth; // Set the current health and max health of the player

    public float invincibleLength; // Set the invincible length of the player
    public float invincibleFrame; // Set the invincible frame of the player
    public SpriteRenderer theSR; // Display the sprite's invincible frame

    public GameObject deathEffect; // Effect when the player dies

    public bool isOut; // Check if the player is out of the zone
    public bool isSpiked; // Check if the player need to be transported

    public bool isDead;

    // Awake is called before the Start function
    void Awake()
    {
        // Set the singleton instance
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        isDead = false;

        // Initialize the current health of the player
        if (GlobalUI.instance.isRestart)
        {
            Debug.Log("CurrentHP: " + GlobalUI.instance.currentHP);
            GlobalUI.instance.currentHP = GlobalUI.instance.maxHP;

            currentHealth = GlobalUI.instance.currentHP;
            PlayerPrefs.SetInt("CurrentHP", currentHealth);

            GlobalUI.instance.isRestart = false;
        }
        else
        {
            Debug.Log("1CurrentHP: " + GlobalUI.instance.currentHP);
            Debug.Log("2CurrentHP: " + PlayerPrefs.GetInt("CurrentHP", GlobalUI.instance.currentHP));
            GlobalUI.instance.currentHP = PlayerPrefs.GetInt("CurrentHP", GlobalUI.instance.currentHP);
            Debug.Log("3CurrentHP: " + GlobalUI.instance.currentHP);
            currentHealth = GlobalUI.instance.currentHP;
        }

        currentHealth = PlayerPrefs.GetInt("CurrentHP", GlobalUI.instance.currentHP);

        // currentHealth = PlayerPrefs.GetInt("CurrentHP", GlobalUI.instance.maxHP);
        UIController.instance.UpdateHealthDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        /* ...
        //if (currentHealth <= 0)
        //{
        //    currentHealth = 0;
        //    isDead = true;
        //    if (deathEffect != null)
        //    {
        //        Debug.Log("Death Effect");
        //        Instantiate(deathEffect, transform.position, transform.rotation);
        //    }
        //    AudioEffect.instance.PlayAudio(8);
        //}
        */

        if (invincibleFrame > 0)
        {
            // Set the sprite's transparency effect
            theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b, 0.7f);

            invincibleFrame -= Time.deltaTime;
        }
        else
        {
            theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b, 1f);
        }

        // JumpTheZone();

    }


    // Function to damage the player
    public void DealDamage(int damage)
    {
        // If the player's invincible frame is less than or equal to 0 then be damaged
        if (invincibleFrame <= 0)
        {
            invincibleFrame = 0;

            GlobalUI.instance.currentHP -= damage;
            currentHealth = GlobalUI.instance.currentHP;

            PlayerPrefs.SetInt("CurrentHP", currentHealth);
            PlayerPrefs.Save();

            if (currentHealth > 0)
            {
                // When not be damaged, set the invincible frame that eqaul to the invincible length
                invincibleFrame = invincibleLength;
            }
            else
            {
                currentHealth = 0;
                isDead = true;
                if (deathEffect != null)
                {
                    Debug.Log("Death Effect");
                    Instantiate(deathEffect, transform.position, transform.rotation);
                }
                MusicEffect.instance.StopAllMusic();

                AudioEffect.instance.PlayAudio(8);
            }

            // deal with the knockback effect
            if (!isOut && !isDead)
            {
                //Debug.Log("KnockBack");
                PlayerController.instance.KnockBack_Spike();
                AudioEffect.instance.PlayAudio(9);
            }

            if (isSpiked && !isDead)
            {
                isSpiked = false;
                PlayerController.instance.TransportPlayer();
            }
        }
        else
        {
            // ...
        }
        // Update the health display
        UIController.instance.UpdateHealthDisplay();
    }

    // Detection does not frequently take precedence over trigger detection
    /* public void JumpTheZone()
    {
        if (PlayerController.instance.theRB.position.y < -6.0f)
        {
            currentHealth--;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                gameObject.SetActive(false);
            }
            PlayerController.instance.TransportPlayer();
        }
    }
   */

}
