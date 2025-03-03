#define Debug
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float moveSpeed; // Horizontal movement speed
    public float currentSpeed;

    public Rigidbody2D theRB;
    public float jumpForce; // Jump speed

    private bool isGrounded = true; // Check if the player is on the ground
    public Transform groundCheckPoint; // Check the position of the ground
    public LayerMask whatIsGround; // Check the layer of the ground

    public int jumpCount;// Can jump count
    private bool wasGrounded = true; // Check whether the player was on the ground during the last frame.

    private Vector2 startPos; // Save the starting position of the player

    private Animator anim;// Create animation

    public SpriteRenderer theSR;// Flip the direction

    public float knockBackLength_Spike, knockBackForce_Spike; // Knockback effect
    private float knockBackFrame_Spike;

    public Vector3 playerForward; // The player faces the direction

    public float knockBackTime_Enemy, knockBackForce_Enemy;
    private float knockBackCounter_Enemy;

    public bool stopInput; // Stop the player's input

    private Transform parent;


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
        gameObject.SetActive(true);

        parent = transform.parent;

        // Get animation component
        anim = GetComponent<Animator>();

        // Get the sprite-renderer component
        theSR = GetComponent<SpriteRenderer>();

        // Before the first frame, initialize wasGrounded
        wasGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, .1f, whatIsGround);
        if (wasGrounded)
        {
            jumpCount = 2;
        }

        playerForward = transform.right; // The player faces the right direction

        startPos = transform.position; // Save the starting position of the player

        moveSpeed = 7.5f;
        currentSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.instance.isPaused || stopInput || PlayerHealthController.instance.isDead)
        {
            // theRB.velocity = Vector2.zero;
            return;
        }

        //Assigns current speed to the role so that allows the player to move left and right
        theRB.velocity = new Vector2(currentSpeed * Input.GetAxis("Horizontal"), theRB.velocity.y);
        // Debug.Log(theRB.velocity);
        // Save the player's last frame's face direction
        Vector3 playerForwarded = playerForward;

        // Update the player's forward direction
        if (theRB.velocity.x != 0)
        {
            playerForward = (theRB.velocity.x > 0 ? transform.right : -transform.right);
        }
        else
        {
            // If the player is not moving, the player's forward direction remains the same
            playerForward = playerForwarded;
        }

        // Debug.DrawRay(transform.position, playerForward * 5f, Color.red);


        // Check if the player is on the ground
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, .1f, whatIsGround);

        // Initialize the jump count
        if (isGrounded && !wasGrounded) // player just landed
        {
            // Debug.Log("Just Landed");
            jumpCount = 2;
#if UNITY_EDITOR
            jumpCount = 100;
#endif
        }
        // Debug.Log("IsGrounded: " + isGrounded);
        wasGrounded = isGrounded; // last frame's player's grounded status

        // Detection of the jump button
        if (Input.GetButtonDown("Jump"))
        {
            // If the player is on the ground, the player can jump
            if (jumpCount != 0)
            {
                jumpCount--;
                // Debug.Log("JumpCount: " + jumpCount);
                theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);

                AudioEffect.instance.PlayAudio(10);
            }
        }

        // Flip the direction of the player
        if (theRB.velocity.x < 0)
        {
            theSR.flipX = true;
        }
        else if (theRB.velocity.x > 0)
        {
            theSR.flipX = false;
        }

        // Assign the value and bool to the animation
        anim.SetFloat("moveSpeed", Mathf.Abs(theRB.velocity.x)); // get the absolute value of the horizontal speed to avoid negative value
        anim.SetBool("isGround", isGrounded);

        #region knockBack
        if (knockBackFrame_Spike > 0)
        {

            theRB.velocity = new Vector2(knockBackForce_Spike * (theSR.flipX ? 1 : -1), knockBackForce_Spike * .6f);
            //Debug.Log("V:" + theRB.velocity);

            knockBackFrame_Spike -= Time.deltaTime;
        }

        if (knockBackCounter_Enemy > 0)
        {
            knockBackCounter_Enemy -= Time.deltaTime;
        }
        #endregion

    }

    public void KnockBack_Spike()
    {
        knockBackFrame_Spike = knockBackLength_Spike;
        anim.SetTrigger("isHurted");
    }

    public void KnockBack_Enemy<T>(T enemy) where T : EnemyManager
    {
        // Debug.Log("KnockBack_Enemy");
        knockBackCounter_Enemy = knockBackTime_Enemy;

        theRB.velocity = new Vector2(knockBackForce_Enemy * (enemy.theRB.velocity.x * theRB.velocity.x == -1 ? -1 : 0), knockBackForce_Enemy * .6f);
    }

    public void TransportPlayer()
    {
        if (PlayerHealthController.instance.currentHealth <= 0)
        {
            return;
        }

        CheckPoint cp = CheckPointController.instance.DeactivateCheckPoints();
        if (cp != null)
        {
            theSR.transform.position = cp.transform.position;
        }
        else
        {
            // If the player is not at the checkpoint, the player will be transported to the starting point
            theSR.transform.position = startPos;
        }
    }

    public void CloseEagle(Vector3 Pos, float dis, FlyEnemy enemy)
    {
        float distance = Vector2.Distance(transform.position, Pos);
        if (distance <= dis)
        {
            enemy.isClose = true;
        }
        else
        {
            enemy.isClose = false;
        }
    }


    // Physical collision detection not using trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Platform")
        {
            // Debug.Log("OnPlatform");
            transform.parent = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Platform")
        {
            // Debug.Log("OffPlatform");
            transform.parent = parent;
        }
    }
}
