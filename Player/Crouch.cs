using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : MonoBehaviour
{
    public bool isCrouch;
    public bool isCanJump;

    public Animator anim;

    public new Collider2D collider;
    public float playerCrouchHeight;

    // Start is called before the first frame update
    void Start()
    {

        anim = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
        playerCrouchHeight = 1.101729f;

        isCrouch = false;
        isCanJump = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("isFirstCrouch: " + isFirstCrouch);
        /* Calculate the player's height 
        //if (collider is CapsuleCollider2D)
        //{
        //    CapsuleCollider2D capsuleCollider = (CapsuleCollider2D)collider;
        //    playerHeight = capsuleCollider.size.y * transform.localScale.y;
        //}
        //Debug.Log("playerHeight: " + playerHeight);
        */

        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCrouch();

            isCanJump = !isCrouch;

            if (!isCanJump)
            {
                PlayerController.instance.jumpCount = 0;
            }
            else
            {
                PlayerController.instance.jumpCount = 2;
            }
        }

        anim.SetBool("isCrouch", isCrouch);
    }


    private void ToggleCrouch()
    {
        if (isCrouch)
        {
            if (!ExistObstacleOverHead())
            {
                isCrouch = false;
            }
        }
        else
        {
            isCrouch = true;
        }
    }

    private bool ExistObstacleOverHead()
    {
        Vector3 headPosition = transform.position + new Vector3(0, playerCrouchHeight * 0.8f, 0);
        Vector2 headCheckSize = new Vector2(0.6f, 0.3f);

        Collider2D collider = Physics2D.OverlapBox(headPosition, headCheckSize, 0, LayerMask.GetMask("Obstacle"));
        return collider != null;
    }
}
