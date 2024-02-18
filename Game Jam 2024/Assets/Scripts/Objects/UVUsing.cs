using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UVUsing : MonoBehaviour
{
    Vector3 screenPoint;
    private Vector3 velocity = Vector3.zero;
    [SerializeField] private float smoothTime = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        screenPoint = Input.mousePosition;
        screenPoint.z = 10.0f;
       // smoothly make the flashlight moving along with the cursor
        transform.position = Vector3.SmoothDamp(transform.position, Camera.main.ScreenToWorldPoint(screenPoint), ref velocity, smoothTime);
    }
}
