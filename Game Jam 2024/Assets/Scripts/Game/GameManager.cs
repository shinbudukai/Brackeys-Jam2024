using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    // MagGlass===========================================
    [SerializeField] GameObject MagGlass;
    private GameObject MagGlassIns;


    // UV===========================================
    [SerializeField] GameObject UV;
    private GameObject UVIns;


    // Thelock===========================================
    private GameObject theLockClone;
    public float lockPickTargetPosHolder;

    [Header("Boost Group")] 
    // syringe===========================================
    public float speedBoost;
    [SerializeField] float boostDuration = 10f;
    private bool boostUsed = false;
    private GameObject[] boostFXs;


    // key===========================================
    [SerializeField] private GameObject theKey;
    private GameObject keyIns;
    public bool keyInUse = false;

    




    //LockPickItem=================================================
    [SerializeField] TextMeshProUGUI pickTextCount;
    public float pickCount;


    //Deal with UI===========================================
    int UILayer;
    private Vector3 oldCamPos;
    private Quaternion oldCamRot;




    //Usage===========================================
    private bool canUseItem = false;



    //GameProcess==========================================
    private bool isOver = false;
    public bool firstMag = true;
    public bool firstUV = true;
    public bool firstAdre = true;
    public bool firstKey = true;
    public bool firstOxy = true;
    [SerializeField] public GameObject backButton;
    public float timerOnMove = 0;
    [SerializeField] GameObject cantUseBoard;
    [SerializeField] GameObject gameOverBoard;
    [SerializeField] GameObject wrongDoorBoard;
    [SerializeField] GameObject consumeBoard;
    [SerializeField] public GameObject endGame;
    [SerializeField] GameObject exitBoard;

    private bool wrongDoorBoardPop = false;
    private bool firstTimeConsume = true;
    public bool isEndGame = false;

    [HideInInspector]
    public int doorValueHolder;

    public bool hitBack = false;



    public bool gameOver = false;

    [SerializeField] Slider oxySlider;

    [HideInInspector]
    public float oxyAmount;
    public float oxyGainAmount =  0;
    public bool rightDoor = false;


    public float openPoint = 0.05f;



    //Items Initialize============================================
    public int magCount = 0;
    public int uvCount = 0;
    public int adreCount = 0;
    public int keyCount = 0;



    //Items Anim===============================================
    [SerializeField] private GameObject MagPopupAnim;
    [SerializeField] private GameObject Mag1stPopupAnim;

    [SerializeField] private GameObject UVPopupAnim;
    [SerializeField] private GameObject UV1stPopupAnim;

    [SerializeField] private GameObject AdrePopupAnim;
    [SerializeField] private GameObject Adre1stPopupAnim;

    [SerializeField] private GameObject KeyPopupAnim;
    [SerializeField] private GameObject Key1stPopupAnim;

    [SerializeField] private GameObject OxyPopupAnim;
    [SerializeField] private GameObject Oxy1stPopupAnim;

    [SerializeField] private GameObject PickPopupAnim;

    [SerializeField] public GameObject box;



    //Jumpscare======================================
    [SerializeField] public GameObject jumpScareScene;
    public bool doneJumpScare = false;
  


    int countTest = 5;



    public static GameManager Instance { get; private set; }
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


        oldCamPos = Camera.main.transform.position;
        oldCamRot = Camera.main.transform.rotation;







    }


    void Start()
    {
        //GameObject[] gameObject = GameObject.FindGameObjectsWithTag("GameOverBoard");

        //if (gameObject != null)
        //{
        //    for(int i = 0; i < gameObject.Length; i++)
        //    {
        //        Debug.Log("Deleted");
        //        Destroy(gameObject[i]);

        //    }

        //}
        openPoint = 0.05f;

        AudioManager.Instance.StopAllAudio();
        AudioManager.Instance.PlaySoundOneShot("Breath");
        AudioManager.Instance.PlaySoundOneShot("Theme");

        isEndGame = false;


        UILayer = LayerMask.NameToLayer("UI");
        oxyAmount = 300f;
        oxySlider.value = oxyAmount;
        oxyGainAmount = 0;

        //InitPick();
        firstMag = true;
        firstUV = true;
        firstAdre = true;
        firstKey = true;
        backButton.SetActive(false);

        


}

    // Update is called once per frame
    void Update()
    {
        
        
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ExitPopUp();
        }
        // IsPointerOverUIElement();

        //reset camera smoothly
        if(hitBack)
        {

            oldCamPos = LevelManager.Instance.camPos;
           

            Quaternion resetRotation = Quaternion.Euler(Vector3.zero);
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, resetRotation, Time.deltaTime * 5f);
            Camera.main.transform.position = oldCamPos;
        }


       

      
        if(isEndGame)
        {
            if(Input.GetMouseButtonDown(0))
            {
                Application.Quit();
            }
           
        }

        oxyAmount -= Time.deltaTime;
        oxySlider.value = oxyAmount + oxyGainAmount;
        if(oxyAmount + oxyGainAmount >= oxySlider.maxValue)
        {
            oxySlider.value = oxySlider.maxValue;
        }

        if(oxySlider.value <= 0)
        {
            GameOver();
        }
        

     //   Debug.Log(oxySlider.value);

        if (Input.GetMouseButtonDown(0))
        {
            MagGlassIns = GameObject.Find("magGlass(Clone)");
            

            if (MagGlassIns != null)
            {
                CameraLookAt.Instance.isDoing = false;
                Cursor.visible = true;
                Destroy(MagGlassIns);
            }
   
            

            UVIns = GameObject.Find("UVlight(Clone)");

            if (UVIns != null)
            {
                CameraLookAt.Instance.isDoing = false;
                Cursor.visible = true;
                Destroy(UVIns);
            }

        }


        //Boost using ===============================

        if (boostUsed)
        {
            boostDuration -= Time.deltaTime;
            Debug.Log("Boost " + boostDuration);

            if (boostDuration <= 0)
            {
                boostUsed = false;
                ToggleBoostFX();
                speedBoost = 1f;
               


            }

        }


        //Pick Count=======================================
        pickTextCount.text = ("x " + pickCount);
        



    }


    public void LockPickTarPosHolder(float targetPos)
    {
        lockPickTargetPosHolder = targetPos;
    }


    public void ResetCamPos()
    {
        CameraLookAt.Instance.isDoing = false;
        Camera.main.eventMask = ~0;
        Camera.main.transform.position = oldCamPos;
        Camera.main.transform.localRotation = oldCamRot;
        Camera.main.transform.eulerAngles = new Vector3(0, 0, 0);
        PostProcessingEffect.instance.GetUnfocus();
        backButton.SetActive(false);
        hitBack = true;
        timerOnMove = 0f;


        GameObject lockSmall = GameObject.FindGameObjectWithTag("LockSmall");
        GameObject lockBig = GameObject.FindGameObjectWithTag("LockBig");
        GameObject[] wrongDoorBoard = GameObject.FindGameObjectsWithTag("WrongDoorBoard");
        for(int i = 0; i < wrongDoorBoard.Length; i++)
        {
            Destroy(wrongDoorBoard[i]);
        }
        


        if (lockSmall != null && lockBig != null)
        {
            Destroy(lockSmall);
            Destroy(lockBig);

        }


        //Reset Audio
        AudioManager.Instance.StopSound("Rattle");
        AudioManager.Instance.StopSound("LockPick1");
        AudioManager.Instance.StopSound("LockPick2");
        AudioManager.Instance.StopSound("LockPickShake");

       

    }




    private void InitPick()
    {
        pickCount = 5;
    }




    public void SelectedMag()
    {
        
        theLockClone = GameObject.Find("The Lock(Clone)");

        if (MagGlassIns == null && theLockClone != null)
        {

            UpdateCount(ref magCount, "Mag");
            //Add this line and the bottom line to make the prevent animation button work
            canUseItem = true;

            Cursor.visible = false;
            MagGlassIns = Instantiate(MagGlass, transform.position, Camera.main.transform.rotation);
            
            CameraLookAt.Instance.isDoing = true;
        }

        else
        {
            
            Debug.Log("Can't use Mag here");
            StartCoroutine(CantUse());

            GameObject temp = GameObject.FindGameObjectWithTag("Mag");
            StartCoroutine(FixHighlightedAnimNoDisable(temp));

            //this one line
            DisableOnHover("MagGlassItem");



        }
    }



    public void SelectedKey()
    {
        keyInUse = false;
        theLockClone = GameObject.Find("The Lock(Clone)");

        if (keyIns == null)
        {
            keyInUse = true;
            UpdateCount(ref keyCount, "Key");
            //Add this line and the bottom line to make the prevent animation button work
            canUseItem = true;

            Cursor.visible = false;
            keyIns = Instantiate(theKey, transform.position, Camera.main.transform.rotation);

            CameraLookAt.Instance.isDoing = true;

            if (theLockClone != null && rightDoor)
            {
                AudioManager.Instance.PlaySoundOneShot("Key");
                LevelManager.Instance.LoadLevel();
                Cursor.visible = true;
                keyInUse = false;
            }

            if (theLockClone != null && !rightDoor)
            {
                Debug.Log("scare");
                Destroy(keyIns);
                Cursor.visible = true;
                keyInUse = false;
                StartCoroutine(WrongDoor());
            }

        }


    }


    public void SelectedUV()
    {
        

        theLockClone = GameObject.Find("The Lock(Clone)");



        

        if (UVIns == null && theLockClone == null)
        {

            UpdateCount(ref uvCount, "UV");
            //Add this line and the bottom line to make the prevent animation button work
            canUseItem = true;

            Cursor.visible = false;
            AudioManager.Instance.PlaySoundOneShot("UVon");
            UVIns = Instantiate(UV, transform.position, Camera.main.transform.rotation);         

            CameraLookAt.Instance.isDoing = true;
        }

        else
        {
            Debug.Log("Can't use UV here");
            StartCoroutine(CantUse());
            GameObject temp = GameObject.FindGameObjectWithTag("UV");
            StartCoroutine(FixHighlightedAnimNoDisable(temp));
            DisableOnHover("UVFlashlightItem");
        }
    }



    public void SelectedBoost()
    {
        if(!boostUsed)
        {
            UpdateCount(ref adreCount, "Adre");
            StartCoroutine(PostProcessingEffect.instance.Boosting());
            CameraLookAt.Instance.OnShakePos(0.5f, 0.5f);
            boostUsed = true;
            AudioManager.Instance.PlaySoundOneShot("HeartBeat");
            AudioManager.Instance.PlaySoundOneShot("Inject");
            ToggleBoostFX();
            boostDuration = 10f;
            speedBoost = 1.5f;

            

            
        }

        else
        {
            Debug.Log("Is Boosted!");
        }
        


    }

    public void UpdateCount(ref int itemsCount, string itemsName)
    {

        GameObject temp = GameObject.FindGameObjectWithTag(itemsName);


        if (itemsCount <= 1)
        {
            

            

            itemsCount--;


            //Fix Button Highlighted Error
            StartCoroutine(FixHighlightedAnim(temp));
            
            return;
        }

        itemsCount--;




        Debug.Log(itemsName + " " + itemsCount);

        
        if (temp != null)
        {
            temp.GetComponentInChildren<TextMeshProUGUI>().text = ("x " + itemsCount);
        }
    }


    private IEnumerator FixHighlightedAnimNoDisable(GameObject GO)
    {
        GO.GetComponent<Button>().enabled = false;
        GO.GetComponent<Animator>().SetTrigger("Normal");
      
        yield return new WaitForSeconds(0.01f);
        GO.GetComponent<Button>().enabled = true;



        yield return null;

    }


    private IEnumerator FixHighlightedAnim(GameObject GO)
    {
        GO.GetComponent<Button>().enabled = false;
        GO.GetComponent<Animator>().SetTrigger("Normal");
        yield return new WaitForSeconds(0.01f);
        GO.SetActive(false);
        GO.GetComponent<Button>().enabled = true;
        yield return null;

    }


    public void GameOver()
    {
        
        Debug.Log("You will be stucked here forever!");
        Cursor.lockState = CursorLockMode.Locked;
        GameObject parent = GameObject.Find("UI");
        GameObject go = Instantiate(gameOverBoard, parent.transform.position, Quaternion.identity);
        go.transform.SetParent(parent.transform);
        backButton.SetActive(false);
        gameOver = true;
        AudioManager.Instance.StopAllAudio();
        Time.timeScale = 0;

        UIInstance ui = FindObjectOfType<UIInstance>();

        if (ui != null)
        {
            Destroy(ui);
        }

        


    }

    public IEnumerator WrongDoor()
    {
        wrongDoorBoardPop = false;
        if(!wrongDoorBoardPop)
        {
            Debug.Log("You will be stucked here forever!");
            GameObject parent = GameObject.Find("UI");
            GameObject go = Instantiate(wrongDoorBoard, parent.transform.position, Quaternion.identity);
            go.transform.SetParent(parent.transform);
            yield return new WaitForSeconds(2f);
            Destroy(go);
            GameObject wrongDoorGO = GameObject.Find("WrongDoorBoard(Clone)");
            if(wrongDoorGO != null)
            {
                Destroy(wrongDoorGO);
            }
            wrongDoorBoardPop = true;

        }
        
        yield break;

    }




    private void ToggleBoostFX()
    {
        //boostFXs[0].GetComponent<Animator>().SetBool("boostUsed", boostUsed);
        //boostFXs[1].GetComponent<Animator>().SetBool("boostUsed", boostUsed);


        ////For the Destroyed one
        //if (boostFXs[2] != null)
        //{
        //    boostFXs[2].GetComponent<Animator>().SetBool("boostUsed", boostUsed);

        //}



        //boostFXs[3].GetComponent<Animator>().SetBool("boostUsed", boostUsed);

        boostFXs = GameObject.FindGameObjectsWithTag("FXBoost");

        for (int i = 0; i < boostFXs.Length;  i++)
        {
            if(boostFXs != null)
            {
                boostFXs[i].GetComponent<Animator>().SetBool("boostUsed", boostUsed);
            }
            
        }

        
    }





    void RayCast()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Debug.DrawRay(Camera.main.transform.position, mousePos - Camera.main.transform.position, Color.blue);
    }


    void DisableOnHover(string gameObjectName)
    {
        GameObject tempGO = GameObject.Find(gameObjectName);
        tempGO.GetComponent<Animator>().ResetTrigger("Selected");
        tempGO.GetComponent<Animator>().SetTrigger("Normal");
        
        Debug.Log(gameObjectName);
    }






    //Check if UI is being hovered

    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }



    //Returns 'true' if we touched or hovering on Unity UI element.
    public bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == UILayer)
              //  Debug.Log("Over UI");



                return true;
        }

      //  Debug.Log("Not over UI");
        return false;
    }


    //Gets all event system raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }



    public IEnumerator PopupAnimMag()
    {
        
        GameObject parent = GameObject.Find("UI");
        GameObject go = Instantiate(MagPopupAnim, parent.transform.position, Quaternion.identity);     
        go.transform.SetParent(parent.transform);
        yield return new WaitForSeconds(2f);
        Destroy(go);
        yield break;
    }
    public IEnumerator PopupAnimMag1st()
    {

        GameObject parent = GameObject.Find("UI");
        GameObject go = Instantiate(Mag1stPopupAnim, parent.transform.position, Quaternion.identity);
        go.transform.SetParent(parent.transform);
        yield return new WaitForSeconds(4.3f);
        Destroy(go);
        yield break;
    }

    public IEnumerator PopupAnimUV()
    {
       
        GameObject parent = GameObject.Find("UI");
        GameObject go = Instantiate(UVPopupAnim, parent.transform.position, Quaternion.identity);
        go.transform.SetParent(parent.transform);
        yield return new WaitForSeconds(2f);
        Destroy(go);
        yield break;
    }
    public IEnumerator PopupAnimUV1st()
    {

        GameObject parent = GameObject.Find("UI");
        GameObject go = Instantiate(UV1stPopupAnim, parent.transform.position, Quaternion.identity);
        go.transform.SetParent(parent.transform);
        yield return new WaitForSeconds(4.3f);
        Destroy(go);
        yield break;
    }



    public IEnumerator PopupAnimAdre()
    {
       
        GameObject parent = GameObject.Find("UI");
        GameObject go = Instantiate(AdrePopupAnim, parent.transform.position, Quaternion.identity);
        go.transform.SetParent(parent.transform);
        yield return new WaitForSeconds(2f);
        Destroy(go);
        yield break;
    }

    public IEnumerator PopupAnimAdre1st()
    {

        GameObject parent = GameObject.Find("UI");
        GameObject go = Instantiate(Adre1stPopupAnim, parent.transform.position, Quaternion.identity);
        go.transform.SetParent(parent.transform);
        yield return new WaitForSeconds(4.3f);
        Destroy(go);
        yield break;
    }




    public IEnumerator PopupAnimPick()
    {

        GameObject parent = GameObject.Find("UI");
        GameObject go = Instantiate(PickPopupAnim, parent.transform.position, Quaternion.identity);
        go.transform.SetParent(parent.transform);
        yield return new WaitForSeconds(2f);
        Destroy(go);
        yield break;
    }


    public IEnumerator PopupAnimKey1st()
    {

        GameObject parent = GameObject.Find("UI");
        GameObject go = Instantiate(Key1stPopupAnim, parent.transform.position, Quaternion.identity);
        go.transform.SetParent(parent.transform);
        yield return new WaitForSeconds(4.3f);
        Destroy(go);
        yield break;
    }




    public IEnumerator PopupAnimKey()
    {

        GameObject parent = GameObject.Find("UI");
        GameObject go = Instantiate(KeyPopupAnim, parent.transform.position, Quaternion.identity);
        go.transform.SetParent(parent.transform);
        yield return new WaitForSeconds(2f);
        Destroy(go);
        yield break;
    }


    public IEnumerator PopupAnimOxy1st()
    {

        GameObject parent = GameObject.Find("UI");
        GameObject go = Instantiate(Oxy1stPopupAnim, parent.transform.position, Quaternion.identity);
        go.transform.SetParent(parent.transform);
        yield return new WaitForSeconds(4.3f);
        Destroy(go);
        yield break;
    }

    public IEnumerator ConsumeOxy1st()
    {

        GameObject parent = GameObject.Find("UI");
        GameObject go = Instantiate(consumeBoard, parent.transform.position, Quaternion.identity);
        go.transform.SetParent(parent.transform);
        yield return new WaitForSeconds(4.3f);
        Destroy(go);
        firstTimeConsume = false;
        yield break;
    }




    public IEnumerator PopupAnimOxy()
    {

        GameObject parent = GameObject.Find("UI");
        GameObject go = Instantiate(OxyPopupAnim, parent.transform.position, Quaternion.identity);
        go.transform.SetParent(parent.transform);
        yield return new WaitForSeconds(2f);
        Destroy(go);
        yield break;
    }












    private IEnumerator CantUse()
    {

        GameObject parent = GameObject.Find("UI");
        GameObject go = Instantiate(cantUseBoard, parent.transform.position, Quaternion.identity);
        go.transform.SetParent(parent.transform);
        yield return new WaitForSeconds(2f);
        Destroy(go);
        yield break;
    }

    private void ExitPopUp()
    {

        GameObject parent = GameObject.Find("UI");
        GameObject go = Instantiate(exitBoard, parent.transform.position, Quaternion.identity);
        go.transform.SetParent(parent.transform);
        Time.timeScale = 0;
        
    }


    public IEnumerator JumpScare(doorBehavior thisDoor)
    {
        int triggerJumpScare = Random.Range(0, 2);

        Debug.Log(triggerJumpScare);
        if (triggerJumpScare == 1)
        {
            // doneJumpScare = false;
            float randomTime = Random.Range(0, 20f);
            yield return new WaitForSeconds(randomTime);
            // CameraLookAt.Instance.OnShakePos(1f, 1f);
            if (backButton.activeSelf && doorValueHolder == 0)
            {
                Debug.Log("this door " + thisDoor.thisDoorValue);

                AudioManager.Instance.StopSound("JumpScare");
                AudioManager.Instance.StopSound("HeartBeat");
                AudioManager.Instance.StopSound("HardBreath");

                jumpScareScene.SetActive(true);
                AudioManager.Instance.PlaySoundOneShot("JumpScare");
                AudioManager.Instance.PlaySoundOneShot("HeartBeat");
                AudioManager.Instance.PlaySoundOneShot("HardBreath");
                AudioManager.Instance.StopSound("Breath");
                StartCoroutine(ReactivateSound());
                yield return new WaitForSeconds(2f);
                jumpScareScene.SetActive(false);
                yield return thisDoor.isScared = true;
                oxyGainAmount = -20f;

                if (firstTimeConsume)
                {
                    StartCoroutine(ConsumeOxy1st());
                }


            }
        }

        else
        {
            yield return null;
        }

       
        
        yield break;
    }


    private IEnumerator ReactivateSound()
    {
        yield return new WaitForSeconds(19f);
        AudioManager.Instance.StopSound("HardBreath");
        AudioManager.Instance.PlaySoundOneShot("Breath");
        yield break;
    }


  








}
