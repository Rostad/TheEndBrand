using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CameraTrigger : MonoBehaviour
{

    private BoxCollider col;
    public Camera NextCamera;
    public Camera PreviousCamera;


    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider>();
        col.isTrigger = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NextCamera.gameObject.SetActive(true);
            PreviousCamera.gameObject.SetActive(false);
            
        }
    }
}
