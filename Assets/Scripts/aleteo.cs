using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aleteo : MonoBehaviour
{
    public float flapAngle = 30f;       // Grados hacia arriba y abajo
    public float flapSpeed = 2f;        // Velocidad del aleteo
    public Vector3 rotationAxis = Vector3.right; // Eje de giro

    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        // Oscilación
        float angle = Mathf.Sin(Time.time * flapSpeed) * flapAngle;
        Quaternion flapRotation = Quaternion.AngleAxis(angle, rotationAxis);
        transform.localRotation = initialRotation * flapRotation;
    }
}
