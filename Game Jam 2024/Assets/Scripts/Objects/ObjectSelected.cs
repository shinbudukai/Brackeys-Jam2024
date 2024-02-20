using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ObjectSelected : MonoBehaviour
{
    [SerializeField] GameObject doorLock;
    [SerializeField] GameObject bigDoorLock;
    public bool moveOver = false;
    
    private GameObject theLock;
    private GameObject theBigLock;

    public float targetPos;
    private bool isChecked = false;

   



    // Start is called before the first frame update
    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()
    {
        

        //Debug.Log("door behavior: " + gameObject.GetComponent<doorBehavior>().thisDoorValue);
    }

    void FixedUpdate()
    {
        MoveToSelectedObject();


    }

    private void OnMouseOver()
    {


        if (Input.GetMouseButtonDown(0) && !GameManager.Instance.IsPointerOverUIElement())
        {
            
            GameManager.Instance.doorValueHolder = gameObject.GetComponent<doorBehavior>().thisDoorValue;

            if (GameManager.Instance.keyInUse && gameObject.GetComponent<doorBehavior>().thisDoorValue == 1)
            {
                AudioManager.Instance.PlaySoundOneShot("Key");
                GameManager.Instance.UpdateCount(ref GameManager.Instance.keyCount, "Key");
                LevelManager.Instance.LoadLevel();
                GameManager.Instance.keyInUse = false;
                Cursor.visible = true;
                return;
            }

            if (GameManager.Instance.keyInUse && gameObject.GetComponent<doorBehavior>().thisDoorValue == 0)
            {
                Debug.Log("scare");
                GameManager.Instance.keyInUse = false;
                Cursor.visible = true;
                StartCoroutine(GameManager.Instance.WrongDoor());

                return;
            }


            CameraLookAt.Instance.LookAtObject(gameObject.transform);
            GameManager.Instance.hitBack = false;
            moveOver = true;
        }
    }

    void MoveToSelectedObject()
    {
        //Check 3DWrap

       

        if (moveOver && !GameManager.Instance.keyInUse)
        {

            
            Camera.main.eventMask = 0;

            GameManager.Instance.LockPickTarPosHolder(this.targetPos);
            //Check door=============
            CheckDoor();

            //Enable 3DWrap on moving
            LevelManager.Instance.Wrap3D.SetActive(true);



            float distance = Vector3.Distance(Camera.main.transform.position, transform.position);
          
            if (distance < 10f)
            {
                
                isChecked = false;
                moveOver = false;

                ////Reset the camera Angle before instanting Lock
                //ResetCamAng();
                AudioManager.Instance.PlaySoundOneShot("GetDoor");

                InstantiateTheLock();

                
                PostProcessingEffect.instance.GetFocus();
                
                GameManager.Instance.backButton.SetActive(true);
                LevelManager.Instance.Wrap3D.SetActive(false);


                return;

            }
            GameManager.Instance.timerOnMove += Time.deltaTime;
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, transform.position, GameManager.Instance.timerOnMove / (CameraLookAt.Instance.timeToMove/ GameManager.Instance.speedBoost));
            
            

        }



        
    }



    //spawn The Lock
    void InstantiateTheLock()
    {

        CameraLookAt.Instance.IsDoingST();
        if (theLock == null)
        {
            CameraLookAt.Instance.isDoing = false;
            
            theLock = Instantiate(doorLock, transform.position, Camera.main.transform.rotation);
            theBigLock = Instantiate(bigDoorLock, transform.position, Camera.main.transform.rotation);

        }
    }


    void CheckDoor()
    {
        if(!isChecked)
        {
            if (gameObject.GetComponent<doorBehavior>().thisDoorValue == 1)
            {
                Debug.Log("Right door");
                GameManager.Instance.rightDoor = true;
            }


            else
            {
                if (!gameObject.GetComponent<doorBehavior>().isScared && gameObject.GetComponent<doorBehavior>().thisDoorValue == 0)
                {
                    Debug.Log("Wrong door");
                    GameManager.Instance.rightDoor = false;

                    Debug.Log("How many function");
                    StartCoroutine(GameManager.Instance.JumpScare(gameObject.GetComponent<doorBehavior>()));






                }

                //if (gameObject.GetComponent<doorBehavior>().isScared && !GameManager.Instance.doneJumpScare)
                //{
                //    gameObject.GetComponent<doorBehavior>().isScared = false;

                //}



            }

            isChecked = true;

            Debug.Log("Checked Door");
          

        }

       
        
        
    }

 
}
