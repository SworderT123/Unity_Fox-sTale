using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlickeringImage : MonoBehaviour
{
    private Image image;
    public Color origColor;
    [Header("FlashSetting")]
    public float flickerSpeed;
    public float flickerTime;

    [Range(0, 1)] public float lentghOfFlash;
    [Header("TargetSetting")]
    [Range(0, 1)] public float targetColorR, targetColorG, targetColorB;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        origColor = image.color;
        StartCoroutine(Flicker());
    }

    private IEnumerator Flicker()
    {
        float currentTime = 0;
        while (currentTime < flickerTime)
        {
            currentTime += Time.deltaTime;
            // Mathf.PingPong returns a loop's value from 0 to 1 and back to 0
            float flickerAmount = Mathf.PingPong(Time.time * flickerSpeed, lentghOfFlash);
            image.color = new Color(Mathf.Lerp(origColor.r, targetColorR, flickerAmount),
                                                    Mathf.Lerp(origColor.g, targetColorG, flickerAmount),
                                                    Mathf.Lerp(origColor.b, targetColorB, flickerAmount),
                                                    origColor.a);
            yield return null;
        }
        image.color = origColor;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
