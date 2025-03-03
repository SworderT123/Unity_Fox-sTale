using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceController : MonoBehaviour
{
    public float bounceForce;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerController.instance.theRB.velocity = new Vector2(PlayerController.instance.theRB.velocity.x, -bounceForce * PlayerController.instance.theRB.velocity.y);
            PlayerController.instance.jumpCount = 2;
            if (Mathf.Abs(PlayerController.instance.theRB.velocity.y) > 1)
            {
                anim.SetTrigger("isJump");
            }
        }
    }
}
