using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DamegePlayer : MonoBehaviour
{
    private float waitFrame; // Set the wait time after the player is damaged then back
    private bool hasReturned;

    public GameObject obj;

    // Hurted can sprint after a slow time when pressing the space key
    public float sprintSpeed;
    public float sprintTime, sprintCounter;
    public float resumeTime;
    public bool isCanSprint;
    public bool isSprinting;

    private bool _isHurted;
    //public bool isHurted => _isHurted;
    private Color origColor; // The original color of the player
    public Color targetColor;

    private void OnDestroy()
    {
        Time.timeScale = 1;
        PlayerController.instance.currentSpeed = PlayerController.instance.moveSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        obj = this.gameObject;

        resumeTime = 0;
        _isHurted = false;

        sprintSpeed = 20f;
        sprintTime = 0.5f;

        isCanSprint = false;
        sprintCounter = sprintTime;

        origColor = PlayerController.instance.theSR.color;
        targetColor = new Color(0.182079f, 0.9018868f, 0.8877729f);
        // Debug.Log("The Target color: " + targetColor);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Time: " + Time.timeScale);
        if (waitFrame > 0)
        {
            waitFrame -= Time.deltaTime;
        }
        else
        {
            if (!hasReturned)
            {
                if (obj.CompareTag("Spike"))
                {
                    // PlayerController.instance.TransportPlayer();
                }
                hasReturned = true; // The player has returned to the checkpoint
            }
        }

        if (_isHurted)
        {
            if (resumeTime < 1)
            {
                // Debug.Log("ʱ��ָ��ڼ�");
                resumeTime += Time.unscaledDeltaTime;
                Time.timeScale = Mathf.Lerp(0, 1, resumeTime);
                //Debug.Log("Time.timeScale: " + Time.timeScale);
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    isCanSprint = true;
                    // Debug.Log("�㱻ǿ����");
                }
            }
            else
            {
                // Debug.Log("ʱ��ָ����");
                resumeTime = 0;
                Time.timeScale = 1;
                PauseMenu.instance.isResumeTime = true;
                _isHurted = false;

                if (isCanSprint)
                {
                    //Debug.Log("���ϣ�");
                    StartSprint();
                    isCanSprint = false;
                }
            }
        }

        if (isSprinting)
        {
            sprintCounter -= Time.deltaTime;
            // �� Lerp ����������������ɫ֮����в�ֵ�������������ǲ�ֵ�İٷֱȣ�0 ��ʾ��ȫ�ǵ�һ����ɫ��1 ��ʾ��ȫ�ǵڶ�����ɫ
            PlayerController.instance.theSR.color = Color.Lerp(origColor, targetColor, (sprintTime - sprintCounter) / sprintTime);
            if (sprintCounter <= 0)
            {
                sprintCounter = 0;
                isSprinting = false;

                PlayerController.instance.currentSpeed = PlayerController.instance.moveSpeed;
                PlayerController.instance.theSR.color = origColor;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            // GetComponent<PlayerHealthController>().DealDamage();
            // according to the singleton pattern, we can use the instance to access the DealDamage function
            if (!obj.CompareTag("Spike"))
            {
                _isHurted = true;
                PauseMenu.instance.isResumeTime = false;
                Time.timeScale = 0;
                PlayerHealthController.instance.isSpiked = false;
            }
            else
            {
                PlayerHealthController.instance.isSpiked = true;
            }
            PlayerHealthController.instance.DealDamage(1);

            waitFrame = 0.25f;
            hasReturned = false;
        }
    }

    void StartSprint()
    {
        if (!isSprinting)
        {
            //Debug.Log("�𳳣����ڳ��");
            isSprinting = true;
            sprintCounter = sprintTime;
            PlayerController.instance.currentSpeed = sprintSpeed;
            //Debug.Log("��ʱ�ٶȣ�" + PlayerController.instance.theRB.velocity);
        }
    }

}
