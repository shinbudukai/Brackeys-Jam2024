using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using UnityEditor;

public class CameraLookAt : MonoBehaviour
{
    

    [HideInInspector] public bool isDoing = false;
    [SerializeField] public float timeToMove;
    private LayerMask everythingMask = ~0;  //Everything in Bitmask
    private LayerMask nothingMask = 0;    //Nothing in Bitmask
    public static CameraLookAt Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }


        

    }

   

    private void Start()
    {   
    
        

        Camera.main.eventMask = everythingMask;
    }




    public void LookAtObject(Transform target)
    {

       
        transform.LookAt(target);
    }


    public void IsDoingST()
    {
        isDoing = true;
        Camera.main.eventMask = nothingMask;

    }



    //Camera Shaking
    public void OnShake(float duration, float strenght)
    {
        //transform.DOShakePosition(duration, strenght);
        transform.DOShakeRotation(duration, strenght);

    }


    public void OnShakePos(float duration, float strenght)
    {
        transform.DOShakePosition(duration, strenght);
        transform.DOShakeRotation(duration, strenght);

    }





}
