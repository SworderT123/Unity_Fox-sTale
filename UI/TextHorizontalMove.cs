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

        // 设置UI的父对象
        keyPanel = transform.parent.parent.GetComponent<RectTransform>();

        /* UI的坐标设置
         * 设置UI的坐标位置时，使用的是锚点的位置，即AnchoredPosition
         * 该位置是该UI的锚点相对于父对象的锚点的位置
         * 锚点位置一般都在UI的中间
         * 因此，设置UI的位置时，需要考虑到其父对象锚点位置的影响
         * 一般来说，父对象的锚点位置也是在中间
         */
        // 设置初始位置，使文字在屏幕中间
        Vector2 anchoredPosition = textRectTransform.anchoredPosition; // 获取text锚点位置
        anchoredPosition.x = 0; // 0 表示父对象的中心位置（锚点位置）
        textRectTransform.anchoredPosition = anchoredPosition;
    }

    void Update()
    {
        if (textRectTransform == null)
            return;

        // 计算新位置
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

        // 检查边界
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

        // 更新位置
        textRectTransform.anchoredPosition = newPosition;
    }
}