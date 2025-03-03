using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class EventSystemManager : MonoBehaviour
{
    public static EventSystemManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject.transform.root);
            // The OnSceneLoaded is called immediately each time after the scene is loaded.
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            // Keep crruent UI
            // Destroy(gameObject);
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

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (FindObjectOfType<EventSystem>() == null)
        {
            CreateEventSystem();
        }
    }

    void CreateEventSystem()
    {
        GameObject obj = new GameObject("EventSystem");
        obj.AddComponent<EventSystem>();
        obj.AddComponent<StandaloneInputModule>();

        // Bind EventSystem to the UICanvas of the current scene
        GameObject uiCanvas = GameObject.FindGameObjectWithTag("UICanvas");
        if (uiCanvas != null)
        {
            obj.transform.SetParent(uiCanvas.transform);
        }
    }
}
