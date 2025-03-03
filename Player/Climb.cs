using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climb : MonoBehaviour
{
    /*
     * ��anim�У������ʹ��.play()����������ʹ��.SetBool����/ .Setfloat()����
     * �ͻᵼ�¶���һֱ����ѭ�����޷���������
     * �ȽϺõ�������ɾ��������������������ǽ��ץǽ�Ķ�����ֱ���ڴ����е���.play()����
     * ������������������Ҫ�Ķ����ܳ�ȥ���Ͳ��ᵼ�¶���ѭ��
     * ��Ҳ��֪��Ϊʲô����������������Ч��
     * ������...
     */
    private Animator anim;

    public LayerMask whatIsLadder;
    public Vector3 offset;

    public bool isRange;
    public bool isClimb;
    public float climbSpeed;

    public enum ClimbState
    {
        Grab,
        Climb,
        Slide,
        Jump,
        None
    }
    public ClimbState cs;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        cs = ClimbState.None;
        isClimb = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckLadder();
        if (isRange && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)))
        {
            isClimb = true;
        }
        else if (!isRange)
        {
            isClimb = false;
        }

        if (isClimb)
        {
            if (Input.GetKey(KeyCode.W))
            {
                cs = ClimbState.Climb;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                cs = ClimbState.Slide;
            }
            else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
            {
                // Debug.Log("Grab");
                cs = ClimbState.Grab;
                // isClimb = false;
            }
        }
        else
        {
            cs = ClimbState.None;
        }

        switch (cs)
        {
            case ClimbState.None:
                PlayerController.instance.theRB.gravityScale = 5f;
                break;
            case ClimbState.Grab:
                Grab();
                break;
            case ClimbState.Climb:
                ClimbUp();
                break;
            case ClimbState.Slide:
                ClimbDown();
                break;
        }

        anim.SetBool("isClimb", isClimb);
        anim.SetFloat("climbSpeed", Mathf.Abs(PlayerController.instance.theRB.velocity.y));

    }

    private void CheckLadder()
    {
        if (Physics2D.OverlapCircle(transform.position - offset, 0.1f, whatIsLadder))
        {
            isRange = true;
        }
        else if (Physics2D.OverlapCircle(transform.position + offset, 0.1f, whatIsLadder))
        {
            isRange = true;
        }
        else
        {
            isRange = false;
        }
    }

    private void Grab()
    {
        PlayerController.instance.theRB.gravityScale = 0; // Set the gravity to 0
        PlayerController.instance.theRB.velocity = Vector2.zero; // Set the velocity to 0
        anim.Play("Player_climb_idle");
    }

    private void ClimbUp()
    {
        PlayerController.instance.theRB.gravityScale = 0; // Set the gravity to 0
        PlayerController.instance.theRB.velocity = new Vector2(0, climbSpeed); // Set the velocity to 0
        anim.Play("Player_clamb");
    }

    private void ClimbDown()
    {
        PlayerController.instance.theRB.gravityScale = 0; // Set the gravity to 0
        PlayerController.instance.theRB.velocity = new Vector2(0, -climbSpeed); // Set the velocity to 0
        anim.Play("Player_clamb");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position - offset, 0.1f);
        Gizmos.DrawWireSphere(transform.position + offset, 0.1f);
    }
}
