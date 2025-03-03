using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForShowText : MonoBehaviour
{
    public bool isRange;

    public Text showText;
    public string showTextString;

    // Start is called before the first frame update
    void Start()
    {
        showText.text = showTextString;
        showText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRange)
        {
            showText.enabled = true;
        }
        else
        {
            showText.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isRange = false;
            TriggerShowText.HideText(showText);
        }
    }
}
