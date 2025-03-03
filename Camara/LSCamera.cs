using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSCamera : MonoBehaviour
{
    public Transform target;
    public Vector2 minPos, maxPos;

    // Start is called before the first frame update
    void Start()
    {

    }

    // LateUpdate is called befor once per frame finished
    void LateUpdate()
    {
        // Clamp the camera's position to the range
        float posX = Mathf.Clamp(target.position.x, minPos.x, maxPos.x);
        float posY = Mathf.Clamp(target.position.y, minPos.y, maxPos.y);

        transform.position = new Vector3(posX, posY, transform.position.z);
    }
}
