using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key : MonoBehaviour
{
    Vector3 screenPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        screenPoint = Input.mousePosition;
        screenPoint.z = 10.0f;
        transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
        if(!GameManager.Instance.keyInUse)
        {
            Destroy(gameObject);
        }
    }
}
