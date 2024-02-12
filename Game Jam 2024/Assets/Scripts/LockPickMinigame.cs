using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class LockPickMinigame : MonoBehaviour
{
    float pickPos;
    Vector2 v2Pos;
    private bool isPaused = false;
    float targetPos;
    [SerializeField] float leanency = 0.1f;
    

    [SerializeField] float lockRerotSpeed = 0.6f;
    [SerializeField] float lockSpeed = 1.2f;
    

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
            return 1 - Mathf.Abs(targetPos - v2Pos.x) + leanency;  //change v2Pos.x = PickPos if use keyboard
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
        InitializedPos();
    }

    // Update is called once per frame
    void Update()
    {
        if(isPaused) return;

        Debug.Log(targetPos);
        Debug.Log("lock Pos " + LockPos);
        
        CylinderRot();
        UpdateAnimation();
        

        if (!Input.GetMouseButton(0))
        {
            GetMousePos();
            Pick();


        }


        if (lockPos > 0.97f)
        {
            Unlocked();
        }
    }



    private void InitializedPos()
    {
        LockPos = 0f;
        PickPos = 0f;
        targetPos = UnityEngine.Random.value;
    }

    private void Unlocked()
    {
        isPaused = true;
        Debug.Log("The door has been unlocked");
    }

    private void UpdateAnimation()
    {
        lockAnimator.SetFloat("PickPos", v2Pos.x);
        lockAnimator.SetFloat("LockOpen", LockPos);
    }

    private void CylinderRot()
    {
        LockPos -= lockRerotSpeed * Time.deltaTime;

        if (Input.GetMouseButton(0))
        {
            LockPos += lockSpeed * Time.deltaTime;

            
        }

        LockPos += Mathf.Abs(Input.GetAxisRaw("Vertical")) * Time.deltaTime * lockSpeed;

        



    }

    private void Pick()
    {
        PickPos += Input.GetAxisRaw("Horizontal") * Time.deltaTime * pickSpeed;
    }

    private void GetMousePos()
    {
        //distance from cursor to object in vector 2
        v2Pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        v2Pos = Camera.main.ScreenToWorldPoint(v2Pos);
        v2Pos = v2Pos - new Vector2(transform.position.x, transform.position.y);
        v2Pos = math.clamp(v2Pos, 0f, 1f);


        

    //    Debug.Log(v2Pos.x);



    }
}
