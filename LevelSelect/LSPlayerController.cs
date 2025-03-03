using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSPlayerController : MonoBehaviour
{
    public float moveSpeed;
    public MapPoint currentPoint;
    public MapPoint lastPoint; // Save current point

    public bool isShowingText;

    // Start is called before the first frame update
    void Start()
    {
        foreach (MapPoint mp in LSManager.instance.connectedPoints)
        {
            if (PlayerPrefs.GetString("CurrentLevelName", "Level1") == mp.levelName)
            {
                currentPoint = mp;
                transform.position = currentPoint.transform.position;
            }
        }
        lastPoint = currentPoint;
    }

    // Update is called once per frame
    void Update()
    {
        // Move towards the current point
        transform.position = Vector3.MoveTowards(transform.position, currentPoint.transform.position, moveSpeed * Time.deltaTime);
        //    isShowingText = true;

        // Next point will not update if player is not at the current point
        // Avoids player skipping points in every frame's update
        if (!LSManager.instance.isLevelLoad && Vector3.Distance(transform.position, currentPoint.transform.position) < .1f)
        {
            lastPoint = currentPoint;

            if (lastPoint.isLevel && !lastPoint.isLocked)
            {
                LSUIController.instance.ShowInfoPanle();
            }

            if (Input.GetAxisRaw("Horizontal") > .2f)
            {
                LSUIController.instance.levelInfoPanel.SetActive(false);
                if (currentPoint.right != null)
                {
                    SetNextPoint(currentPoint.right);
                }
            }

            if (Input.GetAxisRaw("Horizontal") < -.2f)
            {
                LSUIController.instance.levelInfoPanel.SetActive(false);
                if (currentPoint.left != null)
                {
                    SetNextPoint(currentPoint.left);
                }
            }

            if (Input.GetAxisRaw("Vertical") > .2f)
            {
                LSUIController.instance.levelInfoPanel.SetActive(false);
                if (currentPoint.up != null)
                {
                    SetNextPoint(currentPoint.up);
                }
            }

            if (Input.GetAxisRaw("Vertical") < -.2f)
            {
                LSUIController.instance.levelInfoPanel.SetActive(false);
                if (currentPoint.down != null)
                {
                    SetNextPoint(currentPoint.down);
                }

            }

            if (currentPoint.isLevel && Input.GetButtonDown("Jump"))
            {
                if (!currentPoint.isLocked)
                {
                    LSManager.instance.LoadLevel(currentPoint.levelName);
                    LSManager.instance.isLevelLoad = true;
                }
                else
                {
                    if (!isShowingText)
                    {
                        LSUIController.instance.ShowLockedText();
                        isShowingText = true;
                    }
                }
            }
        }
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Debug.Log("Current Point: " + currentPoint.name);
        //    Debug.Log("Last Point: " + lastPoint.name);
        //}
    }

    public void SetNextPoint(MapPoint nextPoint)
    {
        currentPoint = nextPoint;
    }
}
