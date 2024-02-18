using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class box : MonoBehaviour
{

    [SerializeField] private GameObject[] items;
    private bool isOpened = false;
    private float rangeX;
    private float rangeY;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        rangeX = Random.Range(-12f, 12f);
        rangeY = Random.Range(-5f, -3f);
        Vector2 randomPos = new Vector2(rangeX, rangeY);
        transform.position = randomPos;

        isOpened = false;
    }

    private void OnDisable()
    {
        isOpened = true;
    }

    private void OnMouseOver()
    {
        int randomItem = Random.Range(0, items.Length);

        if (Input.GetMouseButtonDown(0) && !GameManager.Instance.IsPointerOverUIElement() && !isOpened)
        {
            AudioManager.Instance.PlaySoundOneShot("ClickItem");
            items[randomItem].gameObject.SetActive(true);
            Debug.Log("Get " + items[randomItem].name);
            isOpened = true;


            switch (items[randomItem].tag)
            {
                case "Mag":
                    StartCoroutine(GameManager.Instance.PopupAnimMag());
                    GameManager.Instance.magCount++;
                    items[randomItem].GetComponentInChildren<TextMeshProUGUI>().text = ("x " + GameManager.Instance.magCount);
                    if(GameManager.Instance.firstMag)
                    {
                        Debug.Log("first time " + items[randomItem].name);
                        StartCoroutine(GameManager.Instance.PopupAnimMag1st());
                        GameManager.Instance.firstMag = false;
                    }
                    

                    break;

                case "UV":
                    StartCoroutine(GameManager.Instance.PopupAnimUV());
                    GameManager.Instance.uvCount++;
                    items[randomItem].GetComponentInChildren<TextMeshProUGUI>().text = ("x " + GameManager.Instance.uvCount);
                    if (GameManager.Instance.firstUV)
                    {
                        Debug.Log("first time " + items[randomItem].name);
                        StartCoroutine(GameManager.Instance.PopupAnimUV1st());
                        GameManager.Instance.firstUV = false;
                    }
                  

                    break;

                case "Adre":
                    StartCoroutine(GameManager.Instance.PopupAnimAdre());
                    GameManager.Instance.adreCount++;
                    items[randomItem].GetComponentInChildren<TextMeshProUGUI>().text = ("x " + GameManager.Instance.adreCount);
                    if (GameManager.Instance.firstAdre)
                    {
                        Debug.Log("first time " + items[randomItem].name);
                        StartCoroutine(GameManager.Instance.PopupAnimAdre1st());
                        GameManager.Instance.firstAdre = false;
                    }

                    
                    break;

                case "LockPick":
                    StartCoroutine(GameManager.Instance.PopupAnimPick());
                    GameManager.Instance.pickCount++;
                    items[randomItem].GetComponentInChildren<TextMeshProUGUI>().text = ("x " + GameManager.Instance.pickCount);
                    if (GameManager.Instance.pickCount == 1)
                    {
                        Debug.Log("first time " + items[randomItem].name);
                    }
                    break;


                case "Key":
                    StartCoroutine(GameManager.Instance.PopupAnimKey());
                    GameManager.Instance.keyCount++;
                    items[randomItem].GetComponentInChildren<TextMeshProUGUI>().text = ("x " + GameManager.Instance.keyCount);
                    if (GameManager.Instance.firstKey)
                    {
                        Debug.Log("first time " + items[randomItem].name);
                        StartCoroutine(GameManager.Instance.PopupAnimKey1st());
                        GameManager.Instance.firstKey = false;
                    }
                    break;

                case "Oxy":
                    GameManager.Instance.oxyGainAmount = 120f;
                    StartCoroutine(GameManager.Instance.PopupAnimOxy());
                    GameManager.Instance.keyCount++;
                    if (GameManager.Instance.firstOxy)
                    {
                        Debug.Log("first time " + items[randomItem].name);
                        StartCoroutine(GameManager.Instance.PopupAnimOxy1st());
                        GameManager.Instance.firstOxy = false;
                    }
                    break;

                default:
                    break;

                
            }

            StartCoroutine(DeactiveOnTime());

            
            // items[randomItem].GetComponentInChildren<TextMeshProUGUI>().text = ("x " +  randomItem).ToString();

        }




    }


    private IEnumerator DeactiveOnTime()
    {
        yield return new WaitForSeconds(6);
        this.gameObject.SetActive(false);
        yield return null;
    }




}
