using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LSUIController : MonoBehaviour
{
    public static LSUIController instance;

    public Text confirmText;
    public LSPlayerController player;

    public Text lockedText;

    public GameObject levelInfoPanel;
    public Text levelNameText, gemsCollectedText, gemTargetText, bestTimeTest, targetTimeText;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        confirmText.gameObject.SetActive(false);
        lockedText.gameObject.SetActive(false);

        FadeUI.instance.FadeIn();

        ShowConfirmText();
    }

    // Update is called once per frame
    void Update()
    {
        /* ...
        //if (player.currentPoint.isLevel && Vector3.Distance(player.transform.position, player.currentPoint.transform.position) < .1f)
        //{
        //    confirmText.gameObject.SetActive(true);
        //    confirmText.gameObject.transform.position = Camera.main.WorldToScreenPoint(player.transform.position + new Vector3(.7f, .5f, 0));
        //}
        //else
        //{
        //    confirmText.gameObject.SetActive(false);
        //}
        */
    }

    public void ShowConfirmText()
    {
        confirmText.gameObject.SetActive(true);
        lockedText.gameObject.SetActive(false);

        StartCoroutine(HideConfirmText());
    }

    IEnumerator HideConfirmText()
    {
        FadeUI.instance.GetText(confirmText);
        yield return new WaitForSeconds(3f);
        FadeUI.instance.FadeOutText();
        yield return new WaitForSeconds(1 / FadeUI.instance.fadeTextSpeed + .2f);
        confirmText.gameObject.SetActive(false);
    }

    public void ShowLockedText()
    {
        // Resume text's alpha to 1
        lockedText.color = new Color(lockedText.color.r, lockedText.color.g, lockedText.color.b, 1);
        lockedText.gameObject.SetActive(true);

        RectTransform rtLockedText = lockedText.GetComponent<RectTransform>();
        rtLockedText.position = Camera.main.WorldToScreenPoint(player.lastPoint.transform.position + new Vector3(0, .5f, 0));


        /* transform.position is relative coordinate of the themselves parent object
        //lockedText.gameObject.transform.position = player.lastPoint.transform.position;
        //Debug.Log("LockedText Position:" + lockedText.gameObject.transform.position);
        //Debug.Log("LastPoint Position:" + player.lastPoint.transform.position);*/




        StartCoroutine(HideLockedText());
    }

    IEnumerator HideLockedText()
    {
        FadeUI.instance.GetText(lockedText);

        yield return new WaitForSeconds(.3f);
        FadeUI.instance.FadeOutText();
        yield return new WaitForSeconds(1 / FadeUI.instance.fadeTextSpeed + .2f);

        lockedText.gameObject.SetActive(false);
        player.isShowingText = false;
    }

    public void ShowInfoPanle()
    {
        // LevelInfoPanel
        levelInfoPanel.SetActive(true);
        levelNameText.text = player.lastPoint.levelBigName;

        // GemsUI
        int gemsCollected = PlayerPrefs.GetInt(player.lastPoint.levelName + "GemsCollected", 0);

        gemsCollectedText.text = "Found: " + gemsCollected;
        gemTargetText.text = "Target: " + player.lastPoint.targetGems;

        // TimeUI
        float bestTime = PlayerPrefs.GetFloat(player.lastPoint.levelName + "BestTime", 0f);
        Debug.Log("BestTime: " + bestTime);

        bestTimeTest.text = "Best: " + ((bestTime == 0f) ? "---" : bestTime.ToString("F1")) + "s";
        targetTimeText.text = "Target: " + player.lastPoint.targetTime + "s";

    }

}