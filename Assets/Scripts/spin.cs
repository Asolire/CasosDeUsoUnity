using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class spin : MonoBehaviour
{
    [Header("Rotación")]
    public Vector3 velocidadRotacion = new Vector3(0f, 20f, 0f); // grados por segundo

    [Header("Flotación")]
    public float amplitud = 0.5f;         // Cuánto sube y baja
    public float frecuencia = 1f;         // Velocidad del ciclo de flotación

    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.position;
    }

    void Update()
    {
        // Rotación constante
        transform.Rotate(velocidadRotacion * Time.deltaTime);

        // Movimiento vertical suave usando una función seno
        float desplazamientoY = Mathf.Sin(Time.time * frecuencia * Mathf.PI * 2f) * amplitud;
        transform.position = posicionInicial + new Vector3(0f, desplazamientoY, 0f);
    }
}
