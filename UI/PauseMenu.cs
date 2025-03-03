
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;

    public GameObject pauseMenu, pauseSetting;

    public string mainMenu, levelSelect;

    public bool isPaused;
    private float resumeTimeScale;

    public bool isSettingOpen;

    public bool isResumeTime;

    // Abandonment scheme
    //private Rigidbody2D playerRb;
    //private Vector3 pausedVelocity;
    //private float pausedAngularVelocity;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(gameObject.transform.root);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //pauseMenu = GameObject.FindWithTag("PauseMenu");
        //pauseSetting = GameObject.FindWithTag("PauseSetting");

        pauseMenu.SetActive(false);
        pauseSetting.SetActive(false);

        levelSelect = "LevelSelect";
        mainMenu = "Main_Menu";
        //Debug.Log("SoundVolume£º" + PlayerPrefs.GetFloat("SoundVolume", 0.75f));
        //Debug.Log("GameVolume£º" + PlayerPrefs.GetFloat("GameVolume", 0.75f));
        isResumeTime = true;
        // playerRb = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
        MusicEffect.instance.StopLevelSelectMusic();
        MusicEffect.instance.StopTitleMusic();

        PauseSetting.instance.soundMixer.SetFloat("SoundVolume", MusicEffect.instance.LinearToDb(PlayerPrefs.GetFloat("SoundVolume", 0.75f)));
        PauseSetting.instance.gameMixer.SetFloat("GameVolume", MusicEffect.instance.LinearToDb(PlayerPrefs.GetFloat("GameVolume", 0.75f)));

        // Debug.Log("GameVolume£º" + PlayerPrefs.GetFloat("GameVolume", 0.75f));
        MusicEffect.instance.PlayGameMusic(PlayerPrefs.GetFloat("GameVolume", 0.75f));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isSettingOpen)
        {
            isPaused = !isPaused;

            if (isPaused)
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0f; // Stop the game's time
                // Debug.Log("Time3:" + Time.timeScale);
                //// Save the current state of motion
                //pausedVelocity = playerRb.velocity;
                //pausedAngularVelocity = playerRb.angularVelocity;
            }
            else
            {
                pauseMenu.SetActive(false);
            }
        }
        // Update if every frame
        // FindObjectOfType<DamegePlayer>().isHurted will return multiple instances

        if (!isPaused && isResumeTime)
        {
            /* Slow recovery time            
            // Calculate the amount of progress that needs to be added to each frame
            // and complete that resumeTimeScale from 0 to 1 during 3s
            resumeTimeScale += Time.unscaledDeltaTime / 1.5f;
            // Increases the value of this variable linearly from 0 to 1 depending on resumeTimeScale value
            Time.timeScale = Mathf.Lerp(0f, 1f, resumeTimeScale);
            */
            Time.timeScale = 1f;
        }

    }

    // Update rigidbody's state of motion in FixedUpdate
    //void FixedUpdate()
    //{
    //    if (!isPaused && resumeTimeScale < 1f)
    //    {
    //        playerRb.velocity = pausedVelocity;
    //        playerRb.angularVelocity = pausedAngularVelocity;
    //    }
    //}

    public void ToMainMenu()
    {
        /* ...
        // Destroy current scene's EventSystem
        // It will cause next scene's EventSystem is not working
        //EventSystem[] eventSystems = FindObjectsOfType<EventSystem>();
        //foreach (EventSystem es in eventSystems)
        //{
        //    if (es.gameObject != gameObject && eventSystems.Length > 1) // Keep what needs to be kept
        //    {
        //        Destroy(es.gameObject);
        //    }
        //}
        // Destroy the current scene's PauseMenu
        pauseMenu.SetActive(false);
        */

        // Resume time
        Time.timeScale = 1f;

        // MainMenu.instance.ToMainMenu();

        StartCoroutine(ToMainMenuCo());

        // Cancle the pause state
        isPaused = false;
    }

    IEnumerator ToMainMenuCo()
    {
        FadeUI.instance.FadeOut();
        MusicEffect.instance.StopAllMusic();

        yield return new WaitForSeconds(1f / FadeUI.instance.fadeSpeed + .2f);
        MusicEffect.instance.PlayTitleMusic(PlayerPrefs.GetFloat("MainMenuVolume", 0.75f));

        SceneManager.LoadScene(mainMenu);
    }

    public void ToLevelSelect()
    {
        // Resume time
        Time.timeScale = 1f;
        StartCoroutine(LoadLevelSelect());
        PlayerPrefs.SetString("CurrentLevel", SceneManager.GetActiveScene().name);

        // Cancle the pause state
        isPaused = false;
    }

    IEnumerator LoadLevelSelect()
    {
        FadeUI.instance.FadeOut();
        MusicEffect.instance.StopGameMusic();
        yield return new WaitForSeconds(1f / FadeUI.instance.fadeSpeed + .2f);
        MusicEffect.instance.PlayLevelSelectMusic(PlayerPrefs.GetFloat("GameVolume", 0.75f));
        /* 
         * We have to destroy the UIController instance to avoid the UI display error
         * And we also need to the FadeScreen keep active to make the black screen effect
         */
        Destroy(UIController.instance.gameObject);

        SceneManager.LoadScene(levelSelect);
    }

    public void ToSetting()
    {
        isSettingOpen = true;

        pauseMenu.SetActive(false);
        pauseSetting.SetActive(true);

        //Debug.Log("SoundVolume£º" + PlayerPrefs.GetFloat("SoundVolume", 0.75f));
        //Debug.Log("GameVolume£º" + PlayerPrefs.GetFloat("GameVolume", 0.75f));


        // Debug.LogError("SettingUI is not assigned or has been destroyed!");
    }

    public void CloseSetting()
    {
        isSettingOpen = false;
        // Close the setting menu
        pauseSetting.SetActive(false);
        pauseMenu.SetActive(true);
    }

}
