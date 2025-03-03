using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FPSDisplay : MonoBehaviour
{
    // ��Ļ�ϵ� Text ����������ʾ FPS
    public Text fpsLabel;

    // ���ڴ洢��� 100 ֡��ʱ���
    private readonly List<float> frameTimestamps = new List<float>(100);
    private float fps;

    private void Start()
    {

    }

    private void Update()
    {
        // ��ӵ�ǰ֡��ʱ���
        frameTimestamps.Add(Time.time);

        // ���ʱ������� 100 �����Ƴ���ɵ�
        if (frameTimestamps.Count > 100)
        {
            frameTimestamps.RemoveAt(0);
        }

        // ���� FPS
        if (frameTimestamps.Count >= 2)
        {
            float deltaTime = frameTimestamps[frameTimestamps.Count - 1] - frameTimestamps[0];
            fps = (1.0f / deltaTime) * frameTimestamps.Count;
        }

        // ������ʾ
        fpsLabel.text = $"FPS: {Mathf.Round(fps)}";
    }
}