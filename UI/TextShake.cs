using UnityEngine;
using UnityEngine.UI;

public class TextShake : MonoBehaviour
{
    public float intensity = 3f; // The higher the value, the more intense the shake effect
    public float speed = 5f; // The higher the value, the faster the shake effect

    private RectTransform rectTransform;
    private Vector3 originalPos;
    private float seed;

    public void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPos = rectTransform.position;
        /*
         * Random seed to get different shake effect
         * The effct will be abundant if the seed's value is big
         */

        seed = Random.Range(0, 100f);
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        // Get smooth random x and y ,they are between -1 and 1
        float x = Mathf.PerlinNoise(seed, Time.time * speed) * 2 - 1;
        float y = Mathf.PerlinNoise(seed + 1, Time.time * speed) * 2 - 1;
        // Multiply by intensity to change text's position to get the shake effect
        rectTransform.position = originalPos + new Vector3(x, y, 0) * intensity;
    }

    void OnDisable()
    {
        if (rectTransform != null && originalPos != null)
            rectTransform.position = originalPos;
    }
}