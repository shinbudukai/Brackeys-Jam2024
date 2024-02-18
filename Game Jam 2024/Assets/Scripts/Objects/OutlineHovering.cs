using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineHovering : MonoBehaviour
{

    private Material objectNormalMaterial;
    private Renderer objectMaterial;
    [SerializeField] private Material[] objectMaterialArray;

   // [SerializeField] private Material hoverMaterial;
    


    private void Awake()
    {
        objectMaterial = GetComponent<Renderer>();
    }

    void Start()
    {
        // objectMaterialArray[0] = objectMaterial;
        objectMaterial.sharedMaterial = objectMaterialArray[0];
    }

    private void Update()
    {
        if (GameManager.Instance.IsPointerOverUIElement())
        {
            objectMaterial.sharedMaterial = objectMaterialArray[0];
            
          
        }
    }

    // Update is called once per frame
    void OnMouseOver()
    {

        objectMaterial.sharedMaterial = objectMaterialArray[1];
       

    }

    void OnMouseExit()
    {

        objectMaterial.sharedMaterial = objectMaterialArray[0];
    }
}
