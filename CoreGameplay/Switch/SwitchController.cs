using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchController : MonoBehaviour
{
    private SpriteRenderer theSR;
    public Sprite switchOn;
    public Sprite switchOff;

    public GameObject connectedObjs;

    public bool isPressed;
    public bool isRange;

    public Text showText;

    // Start is called before the first frame update
    void Start()
    {
        theSR = GetComponent<SpriteRenderer>();

        showText.text = "Press E to switch";
        showText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRange)
        {
            TriggerShowText.ShowText(transform, showText);

            if (Input.GetKeyDown(KeyCode.E))
            {
                isPressed = !isPressed;

                SwitchState();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            isRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            isRange = false;
            TriggerShowText.HideText(showText);
        }
    }

    private void SwitchState()
    {
        if (isPressed)
        {
            theSR.sprite = switchOn;
        }
        else
        {
            theSR.sprite = switchOff;
        }

        if (connectedObjs.activeSelf)
        {
            connectedObjs.SetActive(false);
        }
        else
        {
            connectedObjs.SetActive(true);
        }
    }

}
