using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pinchoBehaviour : MonoBehaviour
{
    
    void Start()
    {
        
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<move>()?.DieAndRespawn();
        }
    }
}