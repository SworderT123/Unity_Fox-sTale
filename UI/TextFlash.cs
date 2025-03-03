using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class TextFlash : MonoBehaviour
{
    public Text text;
    public float flashSpeed;

    private float origAlpha;
    [Range(0, 1)] public float targetAlpha;
    [Range(0, 1)] public float lentghOfFlash;

    private bool isFlashing;

    // 防止因对象状态为 false 时 协程被强迫停止，当再次进入触发器时重新启动协程
    private void OnEnable()
    {
        if (text != null && text.gameObject.activeSelf)
            if (!isFlashing)
            {
                StartCoroutine(Flash());
                isFlashing = true;
            }
    }

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        origAlpha = text.color.a;

        StartCoroutine(Flash());
    }

    private IEnumerator Flash()
    {
        while (true)
        {
            if (text == null || !gameObject.activeSelf)
            {
                yield break;
            }

            float flashAmount = Mathf.PingPong(Time.time * flashSpeed, lentghOfFlash);
            text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(origAlpha, targetAlpha, flashAmount));
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
