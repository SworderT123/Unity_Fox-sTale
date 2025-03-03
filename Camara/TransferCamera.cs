using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferCamera : MonoBehaviour
{
    private Camera mainCamera;
    private CameraController camaeraController;

    private Transform origObj;
    public Transform targetObj;

    public float origSize;
    public float targetSize;

    private Vector3 velocity = Vector3.zero;
    public float smoothToTime;
    public float smoothBackTime;

    private bool isFirst;

    public float stayTimer;
    private float stayCounter;

    // Provide state to the camera
    public enum E_CameraState
    {
        Inactive,
        MovingToTarget,
        Stay,
        Returning
    }

    public E_CameraState currentState = E_CameraState.Inactive;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        camaeraController = mainCamera.GetComponent<CameraController>();

        origObj = camaeraController.target;
        origSize = mainCamera.GetComponent<Camera>().orthographicSize;

        isFirst = true;
    }

    // Update is called once per frame
    void Update()
    {
        /* ...
        if (isRange)
        {
            // Control the camera move smoothly
            Vector3 targetPos = new Vector3(
                targetObj.position.x,
                targetObj.position.y,
                mainCamera.transform.position.z
                );

            mainCamera.transform.position = Vector3.SmoothDamp(
                mainCamera.transform.position,
                targetPos,
                ref velocity,
                smoothTime
                );

            if (mainCamera.transform.position == targetPos)
            {
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                }
                else
                {
                    timer = 0;

                    velocity = Vector3.zero;

                    camaeraController.enabled = true;
                    mainCamera.transform.position = Vector3.SmoothDamp(
                        mainCamera.transform.position,
                        origObj.position,
                        ref velocity,
                        smoothTime
                        );

                    PlayerController.instance.stopInput = false;
                }
            }
        }
        */

        switch (currentState)
        {
            case E_CameraState.MovingToTarget:
                MovingToTarget();
                break;
            case E_CameraState.Stay:
                CameraStay();
                break;
            case E_CameraState.Returning:
                CameraReturn();
                break;
        }

    }

    private void MovingToTarget()
    {
        Vector3 targetPos = new Vector3(
            targetObj.position.x,
            targetObj.position.y,
            mainCamera.transform.position.z
            );

        mainCamera.transform.position = Vector3.SmoothDamp(
            mainCamera.transform.position,
            targetPos,
            ref velocity,
            smoothToTime
            );

        if (Vector3.Distance(targetPos, mainCamera.transform.position) < 0.1f)
        {
            stayCounter = stayTimer;
            currentState = E_CameraState.Stay;
        }
    }

    private void CameraStay()
    {
        if (stayCounter > 0)
        {
            stayCounter -= Time.deltaTime;
        }
        else
        {
            stayCounter = 0;
            velocity = Vector3.zero;
            currentState = E_CameraState.Returning;
        }
    }

    private void CameraReturn()
    {
        Vector3 targetPos = new Vector3(
            origObj.transform.position.x,
            origObj.transform.position.y,
            mainCamera.transform.position.z
            );

        mainCamera.transform.position = Vector3.SmoothDamp(
            mainCamera.transform.position,
            targetPos,
            ref velocity,
            smoothBackTime
            );

        if (Vector3.Distance(targetPos, mainCamera.transform.position) < 0.1f)
        {
            EndTransfer();
        }
    }

    private void StartTransfer()
    {
        mainCamera.GetComponent<Camera>().orthographicSize = targetSize;

        PlayerController.instance.stopInput = true;
        PlayerController.instance.theRB.velocity = Vector2.zero;

        currentState = E_CameraState.MovingToTarget;

        camaeraController.enabled = false;

    }

    private void EndTransfer()
    {
        mainCamera.GetComponent<Camera>().orthographicSize = origSize;

        PlayerController.instance.stopInput = false;

        currentState = E_CameraState.Inactive;

        camaeraController.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isFirst)
        {
            isFirst = false;
            StartTransfer();
        }
    }
}
