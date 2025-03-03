using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalUI : MonoBehaviour
{
    public static GlobalUI instance;

    public int gemsCountInTotal;
    public int currentHP, maxHP;

    public bool isRestart; // Check if the player restarts the game
    /// <summary>
    /// Check if the player plays the game for the first time
    /// </summary>
    public static bool isFirstTime;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(transform.root.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP = 3;
#if UNITY_EDITOR
        //  currentHP = maxHP = 100;
#endif

        isRestart = false;
        isFirstTime = true;
        //Debug.Log("isFirstTime: " + isFirstTime);

    }

    // Update is called once per frame
    void Update()
    {
        if (currentHP <= 0)
        {
            currentHP = 0;
            PlayerHealthController.instance.isDead = true;

            // Reload the scene after 2 seconds
            ReloadScene();
        }
    }

    private void ReloadScene()
    {
        // Reset the current health
        GlobalUI.instance.currentHP = GlobalUI.instance.maxHP;
        PlayerPrefs.SetInt("CurrentHP", GlobalUI.instance.currentHP);
        PlayerPrefs.Save();

        StartCoroutine(ReloadSceneCo());
    }

    IEnumerator ReloadSceneCo()
    {
        FadeUI.instance.FadeOut();
        PlayerController.instance.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
