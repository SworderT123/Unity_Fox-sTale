using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseSetting : MonoBehaviour
{
    public static PauseSetting instance;

    [Header("UI Controller")]
    public Slider soundEffectSlider, gameSlider; // Next use prefabs to replace so that the settings can be extended
    public AudioMixer soundMixer, gameMixer;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        soundEffectSlider.value = PlayerPrefs.GetFloat("SoundVolume", 0.75f);
        gameSlider.value = PlayerPrefs.GetFloat("GameVolume", 0.75f);

        //Debug.Log("SoundVolume£º" + PlayerPrefs.GetFloat("SoundVolume", 0.75f));
        //Debug.Log("GameVolume£º" + PlayerPrefs.GetFloat("GameVolume", 0.75f));

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnCancleClick();
        }
    }

    public void OnApplyClick()
    {
        // Apply the settings
        SettingManager.instance.ChangeSoundVolume(soundEffectSlider.value);
        SettingManager.instance.ChangeGameVolume(gameSlider.value);

        PlayerPrefs.SetFloat("SoundVolume", soundEffectSlider.value);
        PlayerPrefs.Save();
        PlayerPrefs.SetFloat("GameVolume", gameSlider.value);
        PlayerPrefs.Save();

        Debug.Log("SoundVolume£º" + PlayerPrefs.GetFloat("SoundVolume", 0.75f));
        Debug.Log("GameVolume£º" + PlayerPrefs.GetFloat("GameVolume", 0.75f));

        PauseMenu.instance.CloseSetting();
    }

    public void OnCancleClick()
    {
        soundEffectSlider.value = PlayerPrefs.GetFloat("SoundVolume", 0.75f);
        gameSlider.value = PlayerPrefs.GetFloat("GameVolume", 0.75f);

        SettingManager.instance.gameMixer.SetFloat("GameVolume", Mathf.Log10(gameSlider.value) * 20);
        SettingManager.instance.soundMixer.SetFloat("SoundVolume", Mathf.Log10(soundEffectSlider.value) * 20);

        PlayerPrefs.SetFloat("SoundVolume", soundEffectSlider.value);
        PlayerPrefs.SetFloat("GameVolume", gameSlider.value);

        //Debug.Log("SoundVolume£º" + PlayerPrefs.GetFloat("SoundVolume", 0.75f));
        //Debug.Log("GameVolume£º" + PlayerPrefs.GetFloat("GameVolume", 0.75f));

        //Debug.Log("SoundVolumeSlider£º" + soundEffectSlider.value);
        //Debug.Log("GameVolumeSlider£º" + gameSlider.value);

        PauseMenu.instance.CloseSetting();
    }

    public void OnGameVolumeChange()
    {
        SettingManager.instance.ChangeGameVolume(gameSlider.value);
        // Debug.Log("GameMixerMaxVolume: " + SettingManager.instance.gameMixer.GetFloat("GameVolume", out float gameVolume));
        //Debug.Log("GameVolume£º" + PlayerPrefs.GetFloat("GameVolume", 0.75f));
        //Debug.Log("GameVolumeSlider£º" + gameSlider.value);
    }
}
