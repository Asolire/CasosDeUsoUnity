using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerVisual : MonoBehaviour
{
    public float rotationSpeed = 10f;
    public float squashAmount = 0.2f;
    public float squashSpeed = 5f;

    private Vector3 originalScale;
    private move moveScript;

    void Start()
    {
        originalScale = transform.localScale;

        // Busca el Move script en el objeto padre (PlayerRoot)
        moveScript = GetComponentInParent<move>();

        if (moveScript == null)
        {
            Debug.LogError("PlayerVisual no puede encontrar el script Move en el objeto padre.");
        }
    }

    void Update()
    {
        if (moveScript == null) return;

        Vector3 moveDir = moveScript.currentMoveDirection;

        // Rotar suavemente hacia la dirección de movimiento
        if (moveDir.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(-moveDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        // Squash & Stretch según la velocidad vertical
        Vector3 targetScale = originalScale;

        if (moveScript.verticalVelocity > 0.1f)
        {
            // Estirado al subir
            targetScale = new Vector3(originalScale.x - squashAmount, originalScale.y + squashAmount, originalScale.z - squashAmount);
        }
        else if (moveScript.verticalVelocity < -0.1f)
        {
            // Aplastado al bajar
            targetScale = new Vector3(originalScale.x + squashAmount, originalScale.y - squashAmount, originalScale.z + squashAmount);
        }

        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * squashSpeed);
    }
}