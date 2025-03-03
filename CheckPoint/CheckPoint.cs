using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public SpriteRenderer theSR; // the renderer of the checkpoint
    public Sprite cpOn, cpOff; // the state of the checkpoint when it is on and off

    private bool isOn; // the state of the checkpoint
    // Start is called before the first frame update
    void Start()
    {
        isOn = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if the player collides with the checkpoint
        if (other.CompareTag("Player"))
        {
            if (isOn == false)
            {
                AudioEffect.instance.PlayAudio(5); // Play the audio effect
            }
            // set the checkpoint to on
            theSR.sprite = cpOn;
            isOn = true;

            // set the previous checkpoint to off
            CheckPointController.instance.DeactivateCheckPoints();
        }
    }

    public void ResetCheckPoint()
    {
        // set the checkpoint invisible
        gameObject.SetActive(false);
    }
}
