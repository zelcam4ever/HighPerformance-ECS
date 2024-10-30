using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.tag == "Arrow")
        {
            other.transform.parent.GetComponent<Rigidbody>().isKinematic = true;
            other.transform.parent.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

    }
}
