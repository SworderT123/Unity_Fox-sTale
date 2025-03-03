using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LSManager : MonoBehaviour
{
    public static LSManager instance;

    [Header("LevelSetting")]
    public bool isLevelLoad;
    public MapPoint[] connectedPoints;

    //[Header("FadeSetting")]
    //[SerializeField] private bool isFadeIn, isFadeOut;
    //[SerializeField] private float fadeSpeed;
    //[SerializeField] private Image fadeScreen;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Unlock the levels that have been completed by order
        for (int i = 0; i < PlayerPrefs.GetInt("UnlockedLevel", 1); i++)
        {
            // Debug.Log("UnlockedLevel: " + PlayerPrefs.GetInt("UnlockedLevel", 1));
            connectedPoints[i].isLocked = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*if (isFadeIn)
        //{
        //    fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
        //    if (fadeScreen.color.a == 0f)
        //    {
        //        isFadeIn = false;
        //    }
        //}
        //else if (isFadeOut)
        //{
        //    fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
        //    if (fadeScreen.color.a == 1f)
        //    {
        //        isFadeOut = false;
        //    }
        //} */
    }

    /* From black to clear
    //public void FadeIn()
    //{
    //    isFadeIn = true;
    //    isFadeOut = false;
    //}
    //// From clear to black
    //public void FadeOut()
    //{
    //    isFadeIn = false;
    //    isFadeOut = true;
    //}
    */

    public void LoadLevel(string loadLevelName)
    {
        isLevelLoad = true;
        StartCoroutine(LoadLevelCo(loadLevelName));
    }

    IEnumerator LoadLevelCo(string loadLevelName)
    {
        MusicEffect.instance.StopLevelSelectMusic();
        FadeUI.instance.FadeOut();
        yield return new WaitForSeconds(1f / FadeUI.instance.fadeSpeed + .2f);
        isLevelLoad = false;

        MusicEffect.instance.PlayGameMusic(PlayerPrefs.GetFloat("GameVolume", 0.75f));
        // Destroy(LSUIController.instance.gameObject);
        SceneManager.LoadScene(loadLevelName);

        /* Destroy seems to be a bad idea here, because the LSUIController is a singleton

        // FadeOut();
        //while (isFadeOut)
        //{
        //    yield return null;
        //}
        //yield return new WaitForSeconds(1f / fadeSpeed + .2f);
        //Destroy(LSUIController.instance.gameObject);

        //AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(loadLevelName);
        //asyncOperation.allowSceneActivation = false;
        //while (!asyncOperation.isDone)
        //{
        //    if (asyncOperation.progress >= .9f)
        //    {
        //        asyncOperation.allowSceneActivation = true;

        //    }
        //    yield return null;

        //}
        //FadeIn();
        //while (isFadeIn)
        //{
        //    yield return null;
        //}
        //// yield return new WaitForSeconds(1f / fadeSpeed + .2f);
        //isLevelLoad = false;
        */
    }

}
