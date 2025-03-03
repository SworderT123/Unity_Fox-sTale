using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapPoint : MonoBehaviour
{
    public string levelBigName;

    [Header("LevelSetting")]
    public MapPoint up, down, left, right;
    public bool isLevel;
    public string levelName;
    public bool isLocked;

    [Header("TargetSetting")]
    public float targetTime;
    public int targetGems;

    [Header("UISetting")]
    public GameObject gemBadge, timeBadge;

    private void Awake()
    {
        isLocked = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (gemBadge == null || timeBadge == null)
        {
            return;
        }

        if (PlayerPrefs.GetInt(levelName + "GemsCollected", 0) >= targetGems)
        {
            gemBadge.gameObject.SetActive(true);
        }
        else
        {
            gemBadge.gameObject.SetActive(false);
        }

        if (PlayerPrefs.GetFloat(levelName + "BestTime", 0f) <= targetTime)
        {
            timeBadge.gameObject.SetActive(true);
        }
        else
        {
            timeBadge.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
