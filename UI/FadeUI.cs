using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeUI : MonoBehaviour
{
    public static FadeUI instance;

    [Header("FadeScreenUI")]
    public Image fadeScreen;
    public float fadeSpeed = 2f;
    public bool shouldFadeToBlack, shouldFadeFromBlack;
    [Header("FadeTextUI")]
    public Text m_FadeText;
    public float fadeTextSpeed = 3f;
    public bool shouldFadeTextIn, shouldFadeTextOut;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject.transform.root);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (fadeScreen != null)
            fadeScreen.gameObject.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        if (fadeScreen != null)
        {
            // Mathf.MoveTowards is used in frame Update to make the fade effect smooth
            if (shouldFadeToBlack)
            {
                fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
                if (fadeScreen.color.a == 1f)
                {
                    shouldFadeToBlack = false;
                }
            }
            if (shouldFadeFromBlack)
            {
                fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
                if (fadeScreen.color.a == 0f)
                {
                    shouldFadeFromBlack = false;
                }
            }
        }

        if (m_FadeText != null)
        {
            if (shouldFadeTextIn)
            {
                m_FadeText.gameObject.SetActive(true);
                m_FadeText.color = new Color(m_FadeText.color.r, m_FadeText.color.g, m_FadeText.color.b, Mathf.MoveTowards(m_FadeText.color.a, 1f, fadeTextSpeed * Time.deltaTime));
                if (m_FadeText.color.a == 1f)
                {
                    shouldFadeTextIn = false;
                }
            }
            if (shouldFadeTextOut)
            {
                // Debug.Log("shouldFadeTextOut");
                m_FadeText.color = new Color(m_FadeText.color.r, m_FadeText.color.g, m_FadeText.color.b, Mathf.MoveTowards(m_FadeText.color.a, 0f, fadeTextSpeed * Time.deltaTime));
                if (m_FadeText.color.a == 0f)
                {
                    shouldFadeTextOut = false;
                    m_FadeText.gameObject.SetActive(false);
                }
            }
        }
    }

    public void GetText(Text fadeText)
    {
        m_FadeText = fadeText;
    }

    public void FadeOut()
    {
        shouldFadeToBlack = true;
        shouldFadeFromBlack = false;
    }

    public void FadeOutText()
    {
        shouldFadeTextIn = false;
        shouldFadeTextOut = true;
        // Debug.Log("Text Fade Out");
    }

    public void FadeIn()
    {
        shouldFadeToBlack = false;
        shouldFadeFromBlack = true;
    }

    public void FadeInText()
    {
        shouldFadeTextIn = true;
        shouldFadeTextOut = false;
    }
}
