using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingController : MonoBehaviour
{

    [Header("UI Controller")]
    public Slider mainMenuVolumeSlider, gameVolumeSlider; // Manage the volume of the main menu and the game
    public Dropdown resolutionDropdown, fpsDropDown; // Manage the resolution of the game

    private Resolution currentResolution;
    private int currentFPS;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the components
        /*
         * A change in the value here triggers both functions that OnMainMenuVolumeChanged and OnGameVolumeChanged,
         * so I need to trigger the game music change first and then the title music change
         * 
         * Or
         * 
         * // Temporarily remove event listening,
         * after change the value,
         * re-add the event listening
        */
        // Remove the event listener
        mainMenuVolumeSlider.onValueChanged.RemoveListener(OnMainMenuVolumeChanged);
        gameVolumeSlider.onValueChanged.RemoveListener(OnGameVolumeChanged);

        // Change the value of the sliders
        mainMenuVolumeSlider.value = PlayerPrefs.GetFloat("MainMenuVolume", 0.75f);
        gameVolumeSlider.value = PlayerPrefs.GetFloat("GameVolume", 0.75f);

        // SettingManager.instance.ChangeVolume(mainMenuVolumeSlider.value);

        // Re-add the event listener
        mainMenuVolumeSlider.onValueChanged.AddListener(OnMainMenuVolumeChanged);
        gameVolumeSlider.onValueChanged.AddListener(OnGameVolumeChanged);

        // Load the resolution
        resolutionDropdown.ClearOptions(); // Clear the options
        foreach (var item in Screen.resolutions)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData($"{item.width}x{item.height}"));
        }
        // Initialize the resolution dropdown
        currentResolution = Screen.currentResolution;
        resolutionDropdown.value = GetCurrentResolutionIndex(currentResolution);
        fpsDropDown.value = GetCurrentFPSIndex();

        // Load the target FPS
        fpsDropDown.ClearOptions(); // Clear the options
        fpsDropDown.options.Add(new Dropdown.OptionData("30"));
        fpsDropDown.options.Add(new Dropdown.OptionData("60"));
        fpsDropDown.options.Add(new Dropdown.OptionData("90"));
        fpsDropDown.options.Add(new Dropdown.OptionData("Unlimited"));
        // Initialize the FPS dropdown
        int currentFpsIndex = GetCurrentFPSIndex();
        fpsDropDown.value = currentFpsIndex;
        fpsDropDown.RefreshShownValue(); // Refresh the displayed value


    }

    // Update is called once per frame
    void Update()
    {
        // SettingManager.instance.UpdateVolume(mainMenuVolumeSlider.value); falied
    }

    public void OnApplyClick()
    {
        // Get current resolution
        currentResolution = Screen.resolutions[resolutionDropdown.value];
        // Apply the resolution
        SettingManager.instance.ChangeResolution(currentResolution);

        // Get the target FPS
        currentFPS = fpsDropDown.value switch
        {
            0 => 30, // first option
            1 => 60,
            2 => 90,
            3 => -1, // the -1 that means unlimited
            _ => 60 // default value
        };
        // Apply the target FPS
        SettingManager.instance.ChangeFPS(currentFPS);

        // Apply audio settings
        SettingManager.instance.ChangeMainMenuVolume(mainMenuVolumeSlider.value);
        SettingManager.instance.ChangeGameVolume(gameVolumeSlider.value);

        // Save the settings
        SettingManager.instance.SetSettings();

        // Debug.Log("GameMixerMaxVolume: " + SettingManager.instance.gameMixer.GetFloat("GameVolume", out float gameVolume));

        // Close the setting menu
        MainMenu.instance.CloseSetting();
    }

    public void OnCancelClick()
    {
        //Debug.Log("MainMenuVolume£º" + PlayerPrefs.GetFloat("MainMenuVolume", 0.75f));
        //Debug.Log("GameVolume£º" + PlayerPrefs.GetFloat("GameVolume", 0.75f));

        // Restore to the last setting
        SettingManager.instance.LoadSettings();

        //Debug.Log("After Restore MainMenuVolume£º" + PlayerPrefs.GetFloat("MainMenuVolume", 0.75f));
        //Debug.Log("After Restore GameVolume£º" + PlayerPrefs.GetFloat("GameVolume", 0.75f));

        // Restore slider
        mainMenuVolumeSlider.value = PlayerPrefs.GetFloat("MainMenuVolume", 0.75f);
        gameVolumeSlider.value = PlayerPrefs.GetFloat("GameVolume", 0.75f);
        // Restore dropdown
        fpsDropDown.value = GetCurrentFPSIndex();
        resolutionDropdown.value = GetCurrentResolutionIndex(currentResolution);

        MainMenu.instance.CloseSetting();
    }

    public void OnMainMenuVolumeChanged(float a)
    {
        // Stop the game BGM
        MusicEffect.instance.musicSource[4].Stop();

        // Change and play the main menu BGM
        SettingManager.instance.ChangeMainMenuVolume(mainMenuVolumeSlider.value);

        MusicEffect.instance.PlayTitleMusic(mainMenuVolumeSlider.value);

        //Debug.Log("MainMenuSliderVolume: " + mainMenuVolumeSlider.value);
        //Debug.Log("MainMenuVolume" + SettingManager.instance.mainMenuVolume);
        // SettingManager.instance.ChangeVolume(a);
    }

    public void OnGameVolumeChanged(float a)
    {
        // Stop the main menu BGM
        MusicEffect.instance.StopTitleMusic();
        // Change and play the game BGM
        SettingManager.instance.ChangeGameVolume(gameVolumeSlider.value);
        // Cause bad effect
        //MusicEffect.instance.gameMusic.volume = gameVolumeSlider.value;
        //MusicEffect.instance.gameMusic.Play();
        MusicEffect.instance.PlayGameMusic(gameVolumeSlider.value);

        //Debug.Log("GameSliderVolume: " + gameVolumeSlider.value);
        //Debug.Log("GameVolume" + SettingManager.instance.gameVolume);
    }

    public int GetCurrentResolutionIndex(Resolution res)
    {
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].width == res.width && Screen.resolutions[i].height == res.height)
            {
                return i;
            }
        }
        return 0;
    }

    public int GetCurrentFPSIndex()
    {
        return currentFPS switch
        {
            30 => 0,
            60 => 1,
            90 => 2,
            -1 => 3,
            _ => 1
        };
    }


}
