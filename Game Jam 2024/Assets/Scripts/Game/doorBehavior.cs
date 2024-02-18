using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class doorBehavior : MonoBehaviour
{
    
    [SerializeField] public int thisDoorValue;
    
    [SerializeField] GameObject[] fingerPrints;
    private bool fingerCreated = false;
    public bool isScared = false;

    Vector3 randomArea;

    // Start is called before the first frame update

    private void Awake()
    {
        //  int doorRandomValue = Random.Range(0, 2);


    }

    void Start()
    {
        //doorWidth = (gameObject.GetComponent<Collider2D>().bounds.center.x + gameObject.GetComponent<Collider2D>().bounds.extents.x);

        // leftEdge = new Vector2(transform.position.x - (doorWidth / 2), transform.position.y);

        //foreach (var door in FindObjectsOfType(typeof(GameObject)) as GameObject[])
        //{


        //    if (door.tag == ("Door"))
        //    {

        //        if (door.GetComponent<doorBehavior>().doorRandomValue == 1)
        //        {

        //            thisDoorValue  = 0;
        //        }

        //        else
        //        {
        //            thisDoorValue = 1;
        //        }



        //        Debug.Log(door.GetComponent<doorBehavior>().thisDoorValue);



        //    }


        //}

        //for(int i = 0; i < doors.Length; i++)
        //{
        //    if (doors[i].GetComponent<doorBehavior>().doorRandomValue == 1)
        //    {
        //        thisDoorValue = 0; 


        //    }




        //}

        float randomRangeX = Random.Range(-2, 2f);
        float randomRangeY = Random.Range(0, 4.5f);
        randomArea = new Vector3(randomRangeX, randomRangeY, 0);

    }

    // Update is called once per frame
    void Update()
    {

        CreateFingerPrint();
    }

    void CreateFingerPrint()
    {
        if (thisDoorValue == 1)
        {
            if (!fingerCreated)
            {
                Instantiate(fingerPrints[Random.Range(0, fingerPrints.Length)], transform.position + randomArea, Quaternion.identity);
                fingerCreated = true;
            }

          

        }
    }
}
