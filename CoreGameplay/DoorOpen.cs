using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public GameObject collectible;
    public bool isCollected;

    private void OnDisable()
    {
        if (!isCollected)
        {
            isCollected = true;

            collectible.gameObject.SetActive(true);
            Instantiate(collectible, transform.position + new Vector3(0, 1f, 0), transform.rotation);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        collectible.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
