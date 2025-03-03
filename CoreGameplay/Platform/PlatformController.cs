using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [Header("PositionPoints")]
    public Transform platform;
    [SerializeField] private GameObject[] posPoints;
    [SerializeField] private GameObject currentPoint;

    // Can't use struct because it's size is fixed
    // To create a double-linked list, we need to use a class
    private class Point
    {
        public GameObject point;

        public Point nextPoint;
        public Point prevPoint;
    }
    private Point head = null;
    private Point tail = null;

    public void AddPoint(GameObject pointy)
    {
        Point newPoint = new Point();
        newPoint.point = pointy;
        newPoint.nextPoint = null;
        newPoint.prevPoint = null;
        if (head == null)
        {
            head = newPoint;
            tail = newPoint;
        }
        else
        {
            Point current = head;
            while (current.nextPoint != null)
            {
                current = current.nextPoint;
            }
            current.nextPoint = newPoint;
            newPoint.prevPoint = current;
            // Update the tail
            tail = newPoint;
        }
    }

    [Header("MovingSetting")]
    public float moveSpeed;
    public float waitTime;
    public bool isForwards = true;
    [SerializeField] private float waitCounter;




    // Start is called before the first frame update
    void Start()
    {
        PlatformWaitMove();

        /* This method will cause the platform to move to all points in the scene from all platforms
        //posPoints = GameObject.FindGameObjectsWithTag("PlatformPoints");        
        */

        // 获取当前平台子物体中的路径点
        List<GameObject> childPoints = new List<GameObject>();
        foreach (Transform child in transform)
        {
            foreach (Transform point in child)
            {
                if (point.CompareTag("PlatformPoints"))
                {
                    childPoints.Add(point.gameObject);
                }
            }
        }
        posPoints = childPoints.ToArray();
        //// Sort the points by their hierarchical order
        Array.Sort(posPoints, (x, y) => x.transform.GetSiblingIndex().CompareTo(y.transform.GetSiblingIndex()));

        // Remove the parent of the points to avoid the platform moving with the points
        for (int i = 0; i < posPoints.Length; i++)
        {
            posPoints[i].transform.parent = null;
        }
        // Create a link
        head = new Point
        {
            point = posPoints[0]
        };

        for (int i = 1; i < posPoints.Length; i++)
        {
            AddPoint(posPoints[i]);
        }

        currentPoint = head?.point;

    }

    // Update is called once per frame
    void Update()
    {
        platform.position = Vector3.MoveTowards(platform.position, currentPoint.transform.position, Time.deltaTime * moveSpeed);
        //Debug.Log("PlatformPos: " + platform.position);
        //Debug.Log("CurrentPoint: " + currentPoint.transform.position);
        // If the platform reaches the current point, wait for a while and move to the next or previous point
        if (platform.position.Equals(currentPoint.transform.position))
        {
            if (waitCounter > 0)
            {
                waitCounter -= Time.deltaTime;
            }
            else
            {
                // Move forwards
                if (isForwards)
                {
                    Point temp = head;
                    if (currentPoint != tail.point)
                    {
                        for (; temp.point != currentPoint;)
                        {
                            temp = temp.nextPoint;
                        }
                        currentPoint = temp.nextPoint.point;
                    }
                    else
                    {
                        isForwards = false;
                        currentPoint = tail.prevPoint.point;
                    }
                }
                // Move backwards
                else
                {
                    Point temp = tail;
                    if (currentPoint != head.point)
                    {
                        for (; temp.point != currentPoint;)
                        {
                            temp = temp.prevPoint;
                        }
                        currentPoint = temp.prevPoint.point;
                    }
                    else
                    {
                        isForwards = true;
                        currentPoint = head.nextPoint.point;
                    }
                }
                PlatformWaitMove();
            }
        }
    }

    void PlatformWaitMove()
    {
        waitCounter = waitTime;
    }

}
