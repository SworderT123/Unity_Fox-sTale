using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("MouseReference")]
    [SerializeField] private GameObject leftArrow;
    [SerializeField] private GameObject rightArrow;

    [Header("MouseAudio")]
    [SerializeField] public AudioClip hoverSound;
    [SerializeField] public AudioClip clickSound;

    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // 为UI下的 所有子对象的子对象 中的 按钮组件的点击 增添该事件
        // 并且找到左右箭头
        foreach (Transform child in transform)
        {
            if (child.CompareTag("LeftArrow"))
            {
                leftArrow = child.gameObject;
            }
            else if (child.CompareTag("RightArrow"))
            {
                rightArrow = child.gameObject;
            }

            foreach (Transform child2 in child)
            {
                Button button = child2.GetComponent<Button>();
                if (button != null)
                {
                    child2.GetComponent<Button>().onClick.AddListener(OnButtonClick);
                }
            }
        }

        SetArrowVisibility(false);

        // 找到当前播放源
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in audioSources)
        {
            if (audio.isPlaying)
            {
                audioSource = audio;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetArrowVisibility(bool visible)
    {
        if (leftArrow != null)
        {
            leftArrow.SetActive(visible);
        }
        if (rightArrow != null)
        {
            rightArrow.SetActive(visible);
        }
    }

    // 实现接口的方法，当鼠标进入时会调用
    // 如果只需要判断鼠标进入某个UI元素，需要把其他UI元素的Raycast Target属性设置为false
    public void OnPointerEnter(PointerEventData eventData)
    {
        //  Debug.Log("Mouse Enter");
        SetArrowVisibility(true);
        audioSource.PlayOneShot(hoverSound);

        // 获取按钮位置
        RectTransform currentButtonRectTransforn = eventData.pointerCurrentRaycast.gameObject.GetComponent<RectTransform>(); // 获取当前的按钮
        Vector2 currentButtonRectPos = currentButtonRectTransforn.anchoredPosition; // 获取按钮锚点位置

        //  Debug.Log("currentButtonRectPos: " + currentButtonRectPos);

        // 动态获取箭头位置
        RectTransform rightArrowRectTransforn = rightArrow.GetComponent<RectTransform>();
        Vector2 rightArrowRectPos = rightArrowRectTransforn.anchoredPosition; // 获取右箭头锚点位置

        rightArrowRectPos.x = -(currentButtonRectTransforn.rect.width + rightArrowRectTransforn.rect.width) / 2 + currentButtonRectPos.x; // 最后要加上按钮的锚点偏移量
        rightArrowRectPos.y = currentButtonRectTransforn.anchoredPosition.y;
        rightArrowRectTransforn.anchoredPosition = rightArrowRectPos; // 更新右箭头锚点位置

        // Debug.Log("rightArrowRectPos : " + rightArrowRectPos);

        RectTransform leftArrowRectTransforn = leftArrow.GetComponent<RectTransform>();
        Vector2 leftArrowRectPos = leftArrowRectTransforn.anchoredPosition; // 获取左箭头锚点位置

        leftArrowRectPos.x = (currentButtonRectTransforn.rect.width + rightArrowRectTransforn.rect.width) / 2 + currentButtonRectPos.x;
        leftArrowRectPos.y = rightArrowRectPos.y;
        leftArrowRectTransforn.anchoredPosition = leftArrowRectPos; // 更新左箭头锚点位置

        // Debug.Log("leftArrowRectPos :" + leftArrowRectPos);


        //Debug.Log("currentButtonRectTransforn.rect.width / 2 : " + currentButtonRectTransforn.rect.width / 2);
        //Debug.Log("rightArrow.GetComponent<RectTransform>().rect.width / 2 : " + rightArrow.GetComponent<RectTransform>().rect.width / 2);
        //Debug.Log("rightArrowPos: " + rightArrow.transform.position);


        //Debug.Log("leftArrowPos: " + leftArrow.transform.position);
    }
    // 当鼠标离开时会调用
    public void OnPointerExit(PointerEventData eventData)
    {
        // Debug.Log("Mouse Exit");
        SetArrowVisibility(false);
    }

    public void OnButtonClick()
    {
        // Debug.Log("Button Clicked");
        audioSource.PlayOneShot(clickSound);

        SetArrowVisibility(false);
    }
}
