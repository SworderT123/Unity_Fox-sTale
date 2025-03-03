using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTheZone : MonoBehaviour
{
    private float waitTime;

    // Start is called before the first frame update
    void Start()
    {
        waitTime = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerHealthController.instance.isOut = true; // Set the player is out of the zone
            PlayerHealthController.instance.DealDamage(1);
            PlayerHealthController.instance.isOut = false; // Set the player is in the zone

            FadeUI.instance.FadeOut(); // Fade to black when the player enters the zone

            if (PlayerHealthController.instance.currentHealth > 0)
            {
                #region It is difficult to deal with delayed frame updates
                //waitCounter = waitTime;
                //if (waitCounter <= 0)
                //{
                //    PlayerController.instance.TransportPlayer();
                //}
                #endregion

                StartCoroutine(WaitAndTransport());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StopCoroutine(WaitAndTransport()); // Stop the coroutine when the player leaves the zone
        }
    }

    IEnumerator WaitAndTransport()
    {
        yield return new WaitForSeconds(waitTime);
        FadeUI.instance.FadeIn(); // Fade from black when the player is transported
        PlayerController.instance.TransportPlayer();
    }

}
