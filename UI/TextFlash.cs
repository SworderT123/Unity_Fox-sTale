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

    // ��ֹ�����״̬Ϊ false ʱ Э�̱�ǿ��ֹͣ�����ٴν��봥����ʱ��������Э��
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
