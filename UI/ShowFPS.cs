using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FPSDisplay : MonoBehaviour
{
    // 屏幕上的 Text 对象，用于显示 FPS
    public Text fpsLabel;

    // 用于存储最近 100 帧的时间戳
    private readonly List<float> frameTimestamps = new List<float>(100);
    private float fps;

    private void Start()
    {

    }

    private void Update()
    {
        // 添加当前帧的时间戳
        frameTimestamps.Add(Time.time);

        // 如果时间戳超过 100 个，移除最旧的
        if (frameTimestamps.Count > 100)
        {
            frameTimestamps.RemoveAt(0);
        }

        // 计算 FPS
        if (frameTimestamps.Count >= 2)
        {
            float deltaTime = frameTimestamps[frameTimestamps.Count - 1] - frameTimestamps[0];
            fps = (1.0f / deltaTime) * frameTimestamps.Count;
        }

        // 更新显示
        fpsLabel.text = $"FPS: {Mathf.Round(fps)}";
    }
}