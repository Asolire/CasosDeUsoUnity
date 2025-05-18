using UnityEngine;
using System.Collections;

public class SalidaTrigger : MonoBehaviour
{
    public GameObject pantallaReinicio;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pantallaReinicio.SetActive(true);
        }
    }
}