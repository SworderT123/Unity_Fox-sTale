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
        // ΪUI�µ� �����Ӷ�����Ӷ��� �е� ��ť����ĵ�� ������¼�
        // �����ҵ����Ҽ�ͷ
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

        // �ҵ���ǰ����Դ
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

    // ʵ�ֽӿڵķ�������������ʱ�����
    // ���ֻ��Ҫ�ж�������ĳ��UIԪ�أ���Ҫ������UIԪ�ص�Raycast Target��������Ϊfalse
    public void OnPointerEnter(PointerEventData eventData)
    {
        //  Debug.Log("Mouse Enter");
        SetArrowVisibility(true);
        audioSource.PlayOneShot(hoverSound);

        // ��ȡ��ťλ��
        RectTransform currentButtonRectTransforn = eventData.pointerCurrentRaycast.gameObject.GetComponent<RectTransform>(); // ��ȡ��ǰ�İ�ť
        Vector2 currentButtonRectPos = currentButtonRectTransforn.anchoredPosition; // ��ȡ��ťê��λ��

        //  Debug.Log("currentButtonRectPos: " + currentButtonRectPos);

        // ��̬��ȡ��ͷλ��
        RectTransform rightArrowRectTransforn = rightArrow.GetComponent<RectTransform>();
        Vector2 rightArrowRectPos = rightArrowRectTransforn.anchoredPosition; // ��ȡ�Ҽ�ͷê��λ��

        rightArrowRectPos.x = -(currentButtonRectTransforn.rect.width + rightArrowRectTransforn.rect.width) / 2 + currentButtonRectPos.x; // ���Ҫ���ϰ�ť��ê��ƫ����
        rightArrowRectPos.y = currentButtonRectTransforn.anchoredPosition.y;
        rightArrowRectTransforn.anchoredPosition = rightArrowRectPos; // �����Ҽ�ͷê��λ��

        // Debug.Log("rightArrowRectPos : " + rightArrowRectPos);

        RectTransform leftArrowRectTransforn = leftArrow.GetComponent<RectTransform>();
        Vector2 leftArrowRectPos = leftArrowRectTransforn.anchoredPosition; // ��ȡ���ͷê��λ��

        leftArrowRectPos.x = (currentButtonRectTransforn.rect.width + rightArrowRectTransforn.rect.width) / 2 + currentButtonRectPos.x;
        leftArrowRectPos.y = rightArrowRectPos.y;
        leftArrowRectTransforn.anchoredPosition = leftArrowRectPos; // �������ͷê��λ��

        // Debug.Log("leftArrowRectPos :" + leftArrowRectPos);


        //Debug.Log("currentButtonRectTransforn.rect.width / 2 : " + currentButtonRectTransforn.rect.width / 2);
        //Debug.Log("rightArrow.GetComponent<RectTransform>().rect.width / 2 : " + rightArrow.GetComponent<RectTransform>().rect.width / 2);
        //Debug.Log("rightArrowPos: " + rightArrow.transform.position);


        //Debug.Log("leftArrowPos: " + leftArrow.transform.position);
    }
    // ������뿪ʱ�����
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
