using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public Transform target; // Get target's position

    // set clamp range
    public float minY, maxY;

    public Transform farBG, middleBG; // Get the background's position

    /* Discard the following code because the background will move to the target's y position
    private float lastXPos; // Record the last x position of the target */
    private Vector2 lastPos; // Record the last position of the target

    public bool stopFollow; // Stop the camera following the target

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.parent = null; // Set the camera's parent to null

        //Initialize the last x position of the camera
        //lastXPos = transform.position.x;
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (stopFollow)
        {
            return;
        }

        /* Discard the following code because the camera will move to the target's y position
        // Move the camera to the target's position and keep y,z unchanged
        transform.position = new Vector3(target.position.x, transform.position.y, transform.position.z); */
        // Clamp the camera's position
        transform.position = new Vector3(target.position.x, Mathf.Clamp(target.position.y, minY, maxY), transform.position.z);


        // Calculate the x-distance the camera moved
        //float amountToMoveX = transform.position.x - lastXPos;
        // Calculate the x and y distance the camera moved
        Vector2 amountToMove = new Vector2(transform.position.x - lastPos.x, transform.position.y - lastPos.y);

        // Move the background
        farBG.position += new Vector3(amountToMove.x, amountToMove.y, 0f);
        middleBG.position += new Vector3(amountToMove.x, amountToMove.y, 0f) * .5f;

        // Update the last frame's x position of the camera
        //lastXPos = transform.position.x;
        lastPos = transform.position;
    }
}
