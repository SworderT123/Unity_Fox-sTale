using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

using UnityEngine;
using UnityEngine.Audio;

public class SettingManager : MonoBehaviour
{
    public static SettingManager instance;

    [Header("Music")]
    public AudioMixer mainMenuMixer, gameMixer; // Manage the volume of the main menu and the game
    public float mainMenuVolume, gameVolume; // Store the volume of the main menu and the game
    [Header("Sound")]
    public AudioMixer soundMixer; // Manage the volume of the sound effects
    public float soundVolume; // Store the volume of the sound effects

    [Header("Graphics")]
    private int targetFPS = 60;
    private Resolution selectedRes; // Store the selected resolution

    //public float fadeDuration = 1f;
    //private Coroutine fadeCoroutine;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(transform.root.gameObject); // Don't destroy the root object when loading a new scene
            LoadSettings();
        }
        else
        {
            // Can't destory it because of game scene's Setting script need it 
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        mainMenuVolume = PlayerPrefs.GetFloat("MainMenuVolume", 0.75f); // Save the volume of the main menu
    }

    // Update is called once per frame
    void Update()
    {
        // mainMenuVolume = Mathf.MoveTowards(mainMenuVolume, targetVolume, .5f * Time.deltaTime); invalid
    }


    //public void TargerVolume(float volume)
    //{
    //    targetVolume = volume;
    //}

    public void LoadSettings()
    {
        // Load the Audio settings
        mainMenuVolume = PlayerPrefs.GetFloat("MainMenuVolume", 0.75f); // Save the volume of the main menu
        gameVolume = PlayerPrefs.GetFloat("GameVolume", 0.75f); // Save the volume of the game

        soundVolume = PlayerPrefs.GetFloat("SoundVolume", 0.75f); // Save the volume of the sound effects

        // Load the Graphics settings
        targetFPS = PlayerPrefs.GetInt("TargetFPS", 60); // Save the target FPS
        selectedRes = Screen.currentResolution;

        // Apply the settings
        ApplyAllSettings();
        // Save the settings
        SetSettings();
    }

    public void ChangeMainMenuVolume(float volume)
    {
        mainMenuVolume = volume;
        // Debug.Log("Main Menu Volume: " + mainMenuVolume);
        // Upadate the Volume of the AudioMixer
        mainMenuMixer.SetFloat("TitleVolume", MusicEffect.instance.LinearToDb(mainMenuVolume)); // Achive linear changes in audio
        // Debug.Log("Main Menu Volume: " + mainMenuVolume);
    }

    public void ChangeGameVolume(float volume)
    {
        gameVolume = volume;
        // Debug.Log("Game Volume: " + gameVolume);
        // Upadate the Volume of the AudioMixer
        gameMixer.SetFloat("GameVolume", MusicEffect.instance.LinearToDb(gameVolume));
        // Debug.Log("Game Volume: " + gameVolume);
    }

    public void ChangeSoundVolume(float volume)
    {
        soundVolume = volume;

        soundMixer.SetFloat("SoundVolume", MusicEffect.instance.LinearToDb(soundVolume));
        // Debug.Log("Sound Volume: " + soundVolume);
    }

    public void ChangeFPS(int fps)
    {
        targetFPS = fps;
        // Debug.Log("Target FPS: " + targetFPS);
        Application.targetFrameRate = targetFPS;
        // Debug.Log("Application.targetFrameRate: " + Application.targetFrameRate);
    }

    public void ChangeResolution(Resolution resolution)
    {
        selectedRes = resolution;
        // Set the resolution
        Screen.SetResolution(selectedRes.width, selectedRes.height, Screen.fullScreen);
    }

    public void ApplyAllSettings()
    {
        //Apply audio settings
        ChangeMainMenuVolume(mainMenuVolume);
        ChangeGameVolume(gameVolume);

        ChangeSoundVolume(soundVolume);

        // Apply graphics settings
        ChangeFPS(targetFPS);
        ChangeResolution(selectedRes);
    }

    public void SetSettings()
    {
        // Save the Audio settings
        PlayerPrefs.SetFloat("MainMenuVolume", mainMenuVolume);
        PlayerPrefs.Save();
        PlayerPrefs.SetFloat("GameVolume", gameVolume);
        PlayerPrefs.Save();
        PlayerPrefs.SetFloat("SoundVolume", soundVolume);
        PlayerPrefs.Save();
        // Save the Graphics settings
        PlayerPrefs.SetInt("TargetFPS", targetFPS);
        PlayerPrefs.Save();
    }

    /* Abondoned...
    // Fade the volume
    //public void UpdateVolume(float volume)
    //{
    //    mainMenuVolume = Mathf.MoveTowards(mainMenuVolume, volume, .1f * Time.deltaTime);

    //}

    //public void ChangeVolume(float targetVolume)
    //{
    //    if (fadeCoroutine != null)
    //        StopCoroutine(fadeCoroutine);

    //    fadeCoroutine = StartCoroutine(VolumeFade(targetVolume));
    //}

    //IEnumerator VolumeFade(float targetVolume)
    //{
    //    float crruentVolume;
    //    mainMenuMixer.GetFloat("TitleVolume", out crruentVolume);
    //    float startVolume = DbToLinear(crruentVolume);

    //    for (float t = 0; t < fadeDuration; t += Time.deltaTime)
    //    {
    //        float volumeDb = LinearToDb(Mathf.Lerp(startVolume, targetVolume, t / fadeDuration));

    //        mainMenuMixer.SetFloat("TitleVolume", volumeDb);
    //        yield return null;

    //    }

    //    float finalDb = LinearToDb(targetVolume);
    //    mainMenuMixer.SetFloat("TitleVolume", finalDb);
    //}

    //float DbToLinear(float db)
    //{
    //    return Mathf.Pow(10.0f, db / 20.0f);
    //}

    //float LinearToDb(float linear)
    //{
    //    return (linear <= 0.0001f ? -80f : 20.0f * Mathf.Log10(linear));
    //}
    */
}
