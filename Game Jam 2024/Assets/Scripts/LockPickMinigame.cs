using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class LockPickMinigame : MonoBehaviour
{
    float pickLockPos; //for mouse using
    float pickPos;    //for keyboard using
    private bool shaking;  //new method
    private float tension;
    private bool isBreak = false;


    private float idleTimeThreshold = 0.1f; // Set the idle time threshold in seconds
    private Vector3 lastMousePosition;
    private float timeSinceLastMouseMove;


    [SerializeField] private float tensionMultiplicator = 0.5f;

    Vector3 v2Pos;
    private bool isPaused = false;
    float targetPos;
    [SerializeField] float leanency = 0.1f;

    private GameObject bigLock;
    private GameObject door;


    [SerializeField] float lockRerotSpeed = 0.6f;
    [SerializeField] float lockSpeed = 1.2f;

    [HideInInspector] public float pickbreak;


    
    

    public float PickPos
    {
        get { return pickPos; }
        set
        {
            pickPos = value;
            pickPos = Math.Clamp(pickPos, 0f, 1f);

        }
    }

    float lockPos;
    public float LockPos
    {
        get { return  lockPos; }
        set
        {
            lockPos = value;
            lockPos = Mathf.Clamp(lockPos, 0f, MaxRotDis);
        }
    }


    float MaxRotDis
    {
        get
        {
            return 1 - Mathf.Abs(targetPos - pickLockPos) + leanency;  //change v2Pos.x = PickPos if use keyboard
        }
    }


    [SerializeField] private float pickSpeed;

    Animator lockAnimator;


    private void Awake()
    {
        lockAnimator = GetComponent<Animator>();
    }

    void Start()
    {
        bigLock = GameObject.FindGameObjectWithTag("LockBig");

        lastMousePosition = Input.mousePosition;

        InitializedPos();
    }

    // Update is called once per frame
    void Update()
    {
        if(isPaused) return;



        if (bigLock != null)
        {
            bigLock.GetComponent<LockPickMinigame>().targetPos = targetPos;

        }

        if(!isBreak && GameManager.Instance.backButton.activeSelf)
        {
            CheckMouseIdle();
        }

        if (!GameManager.Instance.backButton.activeSelf)
        {
            AudioManager.Instance.StopSound("LockPick2");
            AudioManager.Instance.StopSound("LockPick1");
            
        }



        CylinderRot();
        UpdateAnimation();
        

        if (!Input.GetMouseButton(0))
        {
            
            GetMousePos();
            Pick();
           


        }

        if(Input.GetMouseButtonDown(0))
        {
           
        }

  


        Shaking();




        if (lockPos > 0.97f)
        {
            Unlocked();
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 1);
    }



    private void InitializedPos()
    {
        ResetPick();
        LockPos = 0f;
        targetPos = GameManager.Instance.lockPickTargetPosHolder;
       


    }

    void Shaking()
    {
        // Debug.Log("tension " + tension);
        //  Debug.Log("isBreak " + isBreak);


        //  shaking = MaxRotDis - LockPos < 0.1f;  //true if it's less than 0.1

        // if(MaxRotDis - LockPos < 0.03f && Input.GetMouseButton(0)) shaking = true;
        

        if (MaxRotDis - LockPos < 0.1f && Input.GetMouseButton(0))
        {
            shaking = true;
            AudioManager.Instance.PlaySoundOneShot("LockPickShake");
            AudioManager.Instance.StopSound("LockPick2");
            AudioManager.Instance.StopSound("LockPick1");
            CameraLookAt.Instance.OnShake(0.1f, 0.1f);
           // Debug.Log("isBeingHeld");
        }

        if (MaxRotDis - LockPos >= 0.1f)
        {
            shaking = false;
            AudioManager.Instance.StopSound("LockPickShake");
        }

        if (shaking && Input.GetMouseButton(0))
        {
            tension += Time.deltaTime * tensionMultiplicator;

            if (tension > 1f )
            {
                PickBreak();
            }

        }

    }

    private void PickBreak()
    {
        isBreak = true;
        AudioManager.Instance.PlaySoundOneShot("Break");
        tension = 0f;
        Debug.Log("Pick has been broken");
        lockAnimator.SetTrigger("Break");

        if(GameManager.Instance.pickCount <= 0)
        {
            GameManager.Instance.GameOver();
        }

        else
        {
            GameManager.Instance.pickCount -= 0.5f;
        }
        
       
        // isPaused = true;
    }

    public void CheckPick()
    {
        lockAnimator.SetTrigger("NewPick");
    }


    public void ResetPick()
    {
        //lockAnimator.ResetTrigger("Break");
        //lockAnimator.ResetTrigger("NewPick");
       
        PickPos = 0f;
        isBreak = false;
    }

    private void Unlocked()
    {
        AudioManager.Instance.StopSound("LockPick2");
        AudioManager.Instance.StopSound("LockPick1");

        if (GameManager.Instance.rightDoor)
        {


            lockAnimator.SetBool("Shake", false);       //force the shaking animation to Stop
            AudioManager.Instance.PlaySoundOneShot("Unlock");
            AudioManager.Instance.PlaySoundOneShot("OpenDoor");
            AudioManager.Instance.StopSound("LockPickShake");
            LevelManager.Instance.LoadLevel();
            Debug.Log("The door has been unlocked");

            //Disable 3DWrap on moving
            LevelManager.Instance.Wrap3D.SetActive(false);
            isPaused = true;
        }

        else
        {
            StartCoroutine(GameManager.Instance.WrongDoor());
        }
    }
        

    private void UpdateAnimation()
    {
        lockAnimator.SetFloat("PickPos", pickLockPos);
        lockAnimator.SetFloat("LockOpen", LockPos);
        lockAnimator.SetFloat("TargetPos", targetPos);
        lockAnimator.SetBool("Shake", shaking);
        lockAnimator.SetFloat("AnimMul", GameManager.Instance.speedBoost);
    }

    private void CylinderRot()
    {
        LockPos -= lockRerotSpeed * Time.deltaTime;

        if (Input.GetMouseButton(0) && !isBreak)
        {
            LockPos += lockSpeed * Time.deltaTime * GameManager.Instance.speedBoost;
            AudioManager.Instance.PlaySoundOneShot("Rattle");



        }

        if (Input.GetMouseButtonUp(0))
        {
            AudioManager.Instance.StopSound("Rattle");
        }

        
        LockPos += Mathf.Abs(Input.GetAxisRaw("Vertical")) * Time.deltaTime * lockSpeed;

        



    }

    private void Pick()
    {
        PickPos += Input.GetAxisRaw("Horizontal") * Time.deltaTime * pickSpeed;
    }

    

    private void GetMousePos()
    {


        if(!CameraLookAt.Instance.isDoing)
        {

            //distance from cursor to object in vector 2
            v2Pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
            v2Pos.z = 10f;


            ////Get Value from left to right on the Screen.
            // Vector3 pos = Camera.main.WorldToViewportPoint(v2Pos);

            //Debug.Log(pos);


            ////Lerp the value so and scale it from 0-1                  =>>     Increase the 20f if want the Pick moves faster.
            //  pickLockPos = Mathf.InverseLerp(0f, 10f, pos.x);



            Vector3 tempPos = Camera.main.ScreenToWorldPoint(v2Pos);

            Vector3 pos = Camera.main.WorldToViewportPoint(tempPos);

            pickLockPos = pos.x;

            





            //  v2Pos = v2Pos - new Vector3(transform.position.x, transform.position.y, transform.position.z);

            //  v2Pos = math.clamp(v2Pos, 0f, 1f);

            // creates direction from mouse to current position
            //Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);

        }

    }

    void CheckMouseIdle()
    {
        if (Input.mousePosition != lastMousePosition)
        {
            // Mouse has moved
            lastMousePosition = Input.mousePosition;
            timeSinceLastMouseMove = 0f;
            AudioManager.Instance.PlaySoundOneShot("LockPick2");
            AudioManager.Instance.PlaySoundOneShot("LockPick1");
          
        }
        else
        {
            // Mouse is idle
            timeSinceLastMouseMove += Time.deltaTime;

            if (timeSinceLastMouseMove >= idleTimeThreshold)
            {
                AudioManager.Instance.StopSound("LockPick2");
                AudioManager.Instance.StopSound("LockPick1");
              
            }
        }
    }




}
