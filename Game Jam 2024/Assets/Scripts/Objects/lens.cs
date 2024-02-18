using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lens : MonoBehaviour
{
    private GameObject smallLock, bigLock;

    void Start()
    {
        smallLock = GameObject.FindGameObjectWithTag("LockSmall");
        bigLock = GameObject.FindGameObjectWithTag("LockBig");

    }

    // Update is called once per frame
    void Update()
    {
        if (smallLock != null && bigLock != null)
        {
            bigLock.transform.position = smallLock.transform.position * 2 - transform.position;
        }
        
    }
}
