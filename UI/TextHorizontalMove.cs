using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TextMove : MonoBehaviour
{
    public Text text;
    public float moveSpeed;

    private bool moveRight = true;
    private RectTransform textRectTransform;

    private RectTransform keyPanel;

    void Start()
    {
        text = GetComponent<Text>();
        textRectTransform = text.GetComponent<RectTransform>();

        // ����UI�ĸ�����
        keyPanel = transform.parent.parent.GetComponent<RectTransform>();

        /* UI����������
         * ����UI������λ��ʱ��ʹ�õ���ê���λ�ã���AnchoredPosition
         * ��λ���Ǹ�UI��ê������ڸ������ê���λ��
         * ê��λ��һ�㶼��UI���м�
         * ��ˣ�����UI��λ��ʱ����Ҫ���ǵ��丸����ê��λ�õ�Ӱ��
         * һ����˵���������ê��λ��Ҳ�����м�
         */
        // ���ó�ʼλ�ã�ʹ��������Ļ�м�
        Vector2 anchoredPosition = textRectTransform.anchoredPosition; // ��ȡtextê��λ��
        anchoredPosition.x = 0; // 0 ��ʾ�����������λ�ã�ê��λ�ã�
        textRectTransform.anchoredPosition = anchoredPosition;
    }

    void Update()
    {
        if (textRectTransform == null)
            return;

        // ������λ��
        float delta = moveSpeed * Time.deltaTime;
        Vector2 newPosition = textRectTransform.anchoredPosition;

        if (moveRight)
        {
            newPosition.x += delta;
        }
        else
        {
            newPosition.x -= delta;
        }

        // ���߽�
        float panelWidth = keyPanel.rect.width;
        float textWidth = textRectTransform.rect.width;
        // Debug.Log("panelWidth: " + panelWidth + ", textWidth: " + textWidth);

        float leftBound = -(panelWidth - textWidth) / 2;
        float rightBound = (panelWidth - textWidth) / 2;

        if (newPosition.x >= rightBound)
        {
            newPosition.x = rightBound;
            moveRight = false;
        }
        else if (newPosition.x <= leftBound)
        {
            newPosition.x = leftBound;
            moveRight = true;
        }

        // ����λ��
        textRectTransform.anchoredPosition = newPosition;
    }
}