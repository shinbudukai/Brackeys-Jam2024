using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class DestroyGO : MonoBehaviour
{
    // Start is called before the first frame update


    private void Update()
    {
     
    }

    void OnMouseOver()
    {

        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("destroyyy");
            Destroy(transform.root.gameObject);
        }


    }

    public void DestroyButton()
    {
        Debug.Log("destroyyy");
        Time.timeScale = 1.0f;
        Destroy(transform.parent.gameObject);
    }
}
