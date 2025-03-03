using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    [Tooltip("At least there are two points that one is the starting position and one is the end position")]
    public GameObject[] triggerPos;
    public Transform parent;
    private int currentPos = 0;
    private Vector3 startPos;
    public float triggerDistance;

    public Transform obj;
    public float downSpeed;
    public float upSpeed;

    public bool isClose;

    public float waitTime, waitCounter;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent;
        //triggerPos = GameObject.FindGameObjectsWithTag("SmasherPoints");

        List<GameObject> temp = new List<GameObject>();
        foreach (Transform childPoint in parent.transform)
        {
            if (childPoint.CompareTag("SmasherPoints"))
            {
                temp.Add(childPoint.gameObject);
            }
        }
        triggerPos = temp.ToArray();
        //Debug.Log("triggerPos.Length: " + triggerPos.Length);
        Array.Sort(triggerPos, (x, y) => x.transform.GetSiblingIndex().CompareTo(y.transform.GetSiblingIndex()));

        for (int i = 0; i < triggerPos.Length; i++)
        {
            triggerPos[i].transform.parent = null;
        }

        startPos = triggerPos[0].transform.position;

        waitCounter = waitTime;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 1; i < triggerPos.Length; i++)
        {
            if (Vector3.Distance(PlayerController.instance.transform.position, triggerPos[i].transform.position) <= triggerDistance)
            {
                if (obj.transform.position.Equals(startPos))
                {
                    isClose = true;
                    currentPos = i;
                    break;
                }
            }
        }

        if (isClose)
        {
            obj.position = Vector3.MoveTowards(obj.position, triggerPos[currentPos].transform.position, downSpeed * Time.deltaTime);

            if (obj.position == triggerPos[currentPos].transform.position)
            {
                if (waitCounter > 0)
                {
                    waitCounter -= Time.deltaTime;
                }
                else
                {
                    waitCounter = waitTime;
                    isClose = false;
                }
            }
        }
        else
        {
            obj.position = Vector3.MoveTowards(obj.position, startPos, upSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("KillPlayer");
            PlayerHealthController.instance.DealDamage(GlobalUI.instance.maxHP);
        }
        else if (other.tag == "BossTank")
        {
            other.GetComponent<TankController>().health -= 3;
        }
    }
}
