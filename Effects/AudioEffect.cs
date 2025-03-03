using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEffect : MonoBehaviour
{
    public static AudioEffect instance;

    public AudioSource[] audioSource;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(transform.root.gameObject);
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

    public void PlayAudio(int indexToPlay)
    {
        // If the audio is playing, stop it
        audioSource[indexToPlay].Stop();

        // Adjust the pitch of the audio
        audioSource[indexToPlay].pitch = Random.Range(0.9f, 1.1f);

        // Adjust the volume of the audio
        audioSource[indexToPlay].volume = PlayerPrefs.GetFloat("SoundVolume", 0.75f);

        audioSource[indexToPlay].Play();
    }

}
