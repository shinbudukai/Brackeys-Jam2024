using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update

    // Door===========================================
    private GameObject[] doors;
    [SerializeField] int thisDoorValue;
    int randomPickedDoor;
    [SerializeField] float transitionTime = 2f;
    private Animator trans;

    public int randomBox;
    

    //Background=========================================
    public GameObject Wrap3D;

    public static LevelManager Instance { get; private set; }
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


    void Start()
    {
        GameManager.Instance.rightDoor = false;









        trans = GetComponentInChildren<Animator>();
        pickRandomDoor();
        FindObjectOnLoad();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && GameManager.Instance.gameOver)
        {
            ResetGame();


        }

    }

    private void pickRandomDoor()
    {
        doors = GameObject.FindGameObjectsWithTag("Door");
        randomPickedDoor = Random.Range(0, doors.Length);
        doors[randomPickedDoor].GetComponent<doorBehavior>().thisDoorValue = 1;
        
        for(int i = 0; i < doors.Length; i++)
        {
            doors[i].GetComponent<ObjectSelected>().targetPos = UnityEngine.Random.value; ;
          
        }

    }

    public void LoadLevel()
    {
        StartCoroutine(WaitForAnim(SceneManager.GetActiveScene().buildIndex + 1));  //add 1 is loading the next level in order
        randomBox = Random.Range(0, 2);
        Debug.Log("Box: " + randomBox);


                if (randomBox == 0)
                {
                    GameManager.Instance.box.SetActive(false);
                }

                if (randomBox == 1)
                {
                    Debug.Log("BoxAppear");
                    GameManager.Instance.box.SetActive(true);
                 }


        

        GameManager.Instance.jumpScareScene.SetActive(false);

      


    }

    private IEnumerator WaitForAnim(int levelIndex)
    {
        trans.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);

        if(levelIndex == 12)
        {
            GameManager.Instance.endGame.SetActive(true);
            AudioManager.Instance.StopAllAudio();
            AudioManager.Instance.PlaySoundOneShot("EndGame");

            GameObject ui = GameObject.FindGameObjectWithTag("UI");
            if(ui != null)
            {
                ui.SetActive(false);
            }

            GameManager.Instance.isEndGame = true;





            yield return null;

        }





        SceneManager.LoadScene(levelIndex);

        GameManager.Instance.backButton.SetActive(false);
        GameManager.Instance.timerOnMove = 0f;
        
        // GameManager.Instance.FindObjectOnLoad();



        //Reset Audio
        AudioManager.Instance.StopSound("Rattle");
        AudioManager.Instance.StopSound("LockPick1");
        AudioManager.Instance.StopSound("LockPick2");
        AudioManager.Instance.StopSound("LockPickShake");




    }


    public void FindObjectOnLoad()
    {
        //Enable 3DWrap on moving
        Wrap3D = GameObject.FindGameObjectWithTag("3DWrap");
        if (Wrap3D != null)
        {
            Wrap3D.SetActive(false);
            
        }
    }


    private void ResetGame()
    {
        //UIInstance ui = FindObjectOfType<UIInstance>();

        //if (ui != null)
        //{
        //    Destroy(ui);

        //}

        GameObject[] gameObject = GameObject.FindGameObjectsWithTag("UI");

        if (gameObject != null)
        {
            for (int i = 0; i < gameObject.Length; i++)
            {
                Debug.Log("Deleted");
                Destroy(gameObject[i]);

            }

        }


        GameManager gameManager = FindObjectOfType<GameManager>();

        if (gameManager != null)
        {
            Debug.Log("destroy gmanaer");
            Destroy(gameManager);
        }

        SceneManager.LoadScene(1);
        AudioManager.Instance.PlaySoundOneShot("Breath");
        AudioManager.Instance.PlaySoundOneShot("Theme");

        Time.timeScale = 1f;
        GameManager.Instance.gameOver = false;
        UnityEngine.Cursor.lockState = CursorLockMode.None;







    }

}