using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : MonoBehaviour
{
    public static CheckPointController instance;

    private CheckPoint[] checkPoints;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        checkPoints = FindObjectsOfType<CheckPoint>();
        // After each reopening the project , array are random , so sort the checkpoints by x position
        Array.Sort(checkPoints, (x, y) => x.transform.position.x.CompareTo(y.transform.position.x));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public CheckPoint DeactivateCheckPoints()
    {
        int i = 0;
        for (int j = 1; j < checkPoints.Length; j++)
        {
            if (checkPoints[j].theSR.sprite == checkPoints[j].cpOn)
            {
                if (checkPoints[j - 1].theSR.sprite == checkPoints[j - 1].cpOff)
                {
                    return checkPoints[j];
                }
                else
                {
                    checkPoints[j - 1].ResetCheckPoint();
                    i = j;
                }
            }
        }

        if (checkPoints[i].theSR.sprite == checkPoints[i].cpOff)
        {
            return null;
        }

        return checkPoints[i];
    }
}
