using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transport : MonoBehaviour
{
    private SpriteRenderer theSR;
    public GameObject connectedObjs;

    public bool isRange;

    public Text showText;

    public GameObject transportEffect;

    // Start is called before the first frame update
    void Start()
    {
        theSR = GetComponent<SpriteRenderer>();

        showText.text = "Press E to transport";
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
                Instantiate(transportEffect, PlayerController.instance.transform.position, transform.rotation);
                StartCoroutine(TransportEffect());
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

    private void TransportPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = connectedObjs.transform.position + new Vector3(0, 1, 0);
    }

    IEnumerator TransportEffect()
    {
        PlayerController.instance.theSR.enabled = false;
        yield return new WaitForSeconds(0.5f);
        PlayerController.instance.theSR.enabled = true;
        TransportPlayer();
    }

}
