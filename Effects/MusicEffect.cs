using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicEffect : MonoBehaviour
{
    public static MusicEffect instance;

    public AudioSource[] musicSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject.transform.root);
            // Load the volume settings faster than the Start function
            musicSource[5].volume = PlayerPrefs.GetFloat("MainMenuVolume", 0.75f);
            musicSource[4].volume = PlayerPrefs.GetFloat("GameVolume", 0.75f);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayTitleMusic(float volume)
    {
        if (!musicSource[5].isPlaying)
        {
            //Debug.Log("musicSourceVolume£º" + volume);
            musicSource[5].volume = volume;
            musicSource[5].Play();
            musicSource[5].loop = true;
        }
        else
        {
            musicSource[5].volume = volume;
        }
    }

    public void PlayGameMusic(float volume)
    {
        if (!musicSource[4].isPlaying)
        {
            musicSource[4].volume = volume;
            // Debug.Log("musicSourceVolume£º" + volume);
            musicSource[4].Play();
            musicSource[4].loop = true;
        }
        else
        {
            musicSource[4].volume = volume;
        }
    }

    public void PlayLevelSelectMusic(float volume)
    {
        if (!musicSource[2].isPlaying)
        {
            musicSource[2].volume = volume;
            musicSource[2].Play();
            musicSource[2].loop = true;
        }
        else
        {
            musicSource[2].volume = volume;
        }
    }

    // Stop the title music
    public void StopTitleMusic()
    {
        if (musicSource[5].isPlaying)
        {
            musicSource[5].Stop();
        }
    }

    public void StopGameMusic()
    {
        if (musicSource[4].isPlaying)
        {
            musicSource[4].Stop();
        }
    }

    public void StopLevelSelectMusic()
    {
        if (musicSource[2].isPlaying)
        {
            musicSource[2].Stop();
        }
    }

    public void StopAllMusic()
    {
        for (int i = 0; i < musicSource.Length; i++)
        {
            if (musicSource[i].isPlaying)
            {
                musicSource[i].Stop();
            }
        }
    }

    /// <summary>
    /// Convert the volume from linear to decibels
    /// </summary>
    /// <param name="volume"></param>
    /// <returns></returns>
    public float LinearToDb(float volume)
    {
        return (volume < 0.00001f ? -80f : 20 * Mathf.Log10(volume));
    }

}
