using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;

    private string levelName;

    public GameObject mainMenu, settingMenu;

    public Button lastStartButton;
    private bool isLastStartButton;

    public Text pologueText;

    public EventSystem eventSystem;

    // private TextMeshProUGUI showNameText; // TextMeshPro 的组件类型是 TextMeshProUGUI

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(transform.root.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

        pologueText = GameObject.FindWithTag("PologueText").GetComponent<Text>();
        FadeUI.instance.GetText(pologueText);

        StartCoroutine(InitMainMenu());
        // Debug.Log("STARTCOROUTINE");

        mainMenu = GameObject.FindWithTag("MainMenu");
        settingMenu = GameObject.FindWithTag("SettingMenu");

        // Initialize the UI
        mainMenu.SetActive(true);
        settingMenu.SetActive(false);

        //Debug.Log("MainMenuVolume：" + PlayerPrefs.GetFloat("MainMenuVolume", 0.75f));
        //Debug.Log("MainBGM：" + MusicEffect.instance.titleMusic.volume);
        //Debug.Log("GameVolume：" + PlayerPrefs.GetFloat("GameVolume", 0.75f));
        //Debug.Log("GameBGM：" + MusicEffect.instance.gameMusic.volume);

        // Play the title music
        SettingManager.instance.mainMenuMixer.SetFloat("TitleVolume", MusicEffect.instance.LinearToDb(PlayerPrefs.GetFloat("MainMenuVolume", 0.75f)));
        MusicEffect.instance.PlayTitleMusic(PlayerPrefs.GetFloat("MainMenuVolume", 0.75f));
        MusicEffect.instance.StopGameMusic();

        // Listen for the mouse clicks
        isLastStartButton = false;
        lastStartButton = GameObject.FindWithTag("LastButton").GetComponent<Button>();
        lastStartButton.onClick.AddListener(LastStartGame);

        levelName = PlayerPrefs.GetString("LevelName", "Level1"); // ...

        //showNameText = GameObject.FindWithTag("NameText").GetComponentInChildren<TextMeshProUGUI>();
        //if (showNameText == null)
        //{
        //    Debug.Log("?");
        //}
        //showNameText.enabled = true;
        Debug.Log("vscode");
    }

    IEnumerator InitMainMenu()
    {
        eventSystem.enabled = false;

        FadeUI.instance.FadeOut();
        FadeUI.instance.FadeInText();

        // 禁用鼠标点击事件，防止在没显示完全时点击按钮
        yield return new WaitForSeconds(2.5f);
        FadeUI.instance.FadeOutText();
        FadeUI.instance.FadeIn();

        eventSystem.enabled = true;

        GlobalUI.isFirstTime = false;

        Debug.Log("isFirstTime: " + GlobalUI.isFirstTime);
    }

    // Update is called once per frame
    void Update()
    {
        /* ...
        //if (!quitGame)
        //{
        //    fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, 1.0f * Time.deltaTime));
        //}
        //else
        //{
        //    fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, 1.0f * Time.deltaTime));
        //}
        */
    }


    public void StartGame()
    {
        mainMenu.SetActive(false);
        GlobalUI.instance.isRestart = true;

        MusicEffect.instance.StopTitleMusic();
        StartCoroutine(StartGameCo());
    }

    IEnumerator StartGameCo()
    {
        FadeUI.instance.FadeOut();
        yield return new WaitForSeconds(1f / FadeUI.instance.fadeSpeed + .2f);
        // Play the game music
        if (!isLastStartButton)
        {
            SceneManager.LoadScene("Level1");
        }
        else
        {
            SceneManager.LoadScene(levelName);
        }
    }

    private void LastStartGame()
    {
        isLastStartButton = true;
        GlobalUI.instance.isRestart = false;
    }

    public void OpenSetting()
    {
        //Debug.Log("MainMenuVolume：" + PlayerPrefs.GetFloat("MainMenuVolume", 0.75f));
        //Debug.Log("GameVolume：" + PlayerPrefs.GetFloat("GameVolume", 0.75f));

        MusicEffect.instance.PlayTitleMusic(PlayerPrefs.GetFloat("MainMenuVolume", 0.75f));
        MusicEffect.instance.StopGameMusic();

        mainMenu.SetActive(false);
        settingMenu.SetActive(true);
    }

    public void CloseSetting()
    {
        //Debug.Log("MainMenuVolume：" + PlayerPrefs.GetFloat("MainMenuVolume", 0.75f));
        //Debug.Log("GameVolume：" + PlayerPrefs.GetFloat("GameVolume", 0.75f));

        // Play the title music
        MusicEffect.instance.PlayTitleMusic(PlayerPrefs.GetFloat("MainMenuVolume", 0.75f));
        MusicEffect.instance.StopGameMusic();

        mainMenu.SetActive(true);
        settingMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
