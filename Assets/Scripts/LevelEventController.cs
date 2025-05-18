using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEventController : MonoBehaviour
{
    public Transform cameraTransform;
    public Transform barricadeTransform;
    public Transform cameraPlayerTarget;

    public Vector3 cameraBarricadeOffset = new Vector3(0, 2, -5);
    public float cameraMoveSpeed = 2f;
    public float barricadeLowerAmount = 2f;
    public float barricadeLowerSpeed = 1f;

    public AudioSource successSound;
    public move playerMoveScript;

    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;

    public void TriggerBarricadeEvent()
    {
        StartCoroutine(BarricadeSequence());
    }

    IEnumerator BarricadeSequence()
    {
        // Congelar el movimiento del jugador
        playerMoveScript.enabled = false;

        // Guardar posición y rotación actuales de la cámara
        originalCameraPosition = cameraTransform.position;
        originalCameraRotation = cameraTransform.rotation;

        // Mover la cámara hacia la barricada
        Vector3 targetCamPos = barricadeTransform.position + cameraBarricadeOffset;
        yield return MoveCameraTo(targetCamPos);

        // Pequeña pausa para efecto cinematográfico
        yield return new WaitForSeconds(0.5f);

        // Reproducir sonido
        if (successSound != null) successSound.Play();

        // Bajar la barricada
        Vector3 finalPos = barricadeTransform.position - new Vector3(0, barricadeLowerAmount, 0);
        float elapsed = 0f;
        Vector3 start = barricadeTransform.position;

        while (elapsed < 1f)
        {
            barricadeTransform.position = Vector3.Lerp(start, finalPos, elapsed);
            elapsed += Time.deltaTime * barricadeLowerSpeed;
            yield return null;
        }

        barricadeTransform.position = finalPos;

        // Esperar antes de volver la cámara
        yield return new WaitForSeconds(0.5f);

        // Volver cámara a su posición original
        yield return MoveCameraTo(originalCameraPosition, originalCameraRotation);

        // Reactivar control del jugador
        playerMoveScript.enabled = true;
    }

    IEnumerator MoveCameraTo(Vector3 targetPos)
    {
        float distance = Vector3.Distance(cameraTransform.position, targetPos);

        while (distance > 0.1f)
        {
            cameraTransform.position = Vector3.MoveTowards(
                cameraTransform.position,
                targetPos,
                cameraMoveSpeed * Time.deltaTime
            );

            distance = Vector3.Distance(cameraTransform.position, targetPos);
            yield return null;
        }

        cameraTransform.position = targetPos;
    }

    // Sobrecarga para rotación también
    IEnumerator MoveCameraTo(Vector3 targetPos, Quaternion targetRot)
    {
        float distance = Vector3.Distance(cameraTransform.position, targetPos);

        while (distance > 0.1f)
        {
            cameraTransform.position = Vector3.MoveTowards(
                cameraTransform.position,
                targetPos,
                cameraMoveSpeed * Time.deltaTime
            );

            cameraTransform.rotation = Quaternion.Slerp(
                cameraTransform.rotation,
                targetRot,
                Time.deltaTime * cameraMoveSpeed
            );

            distance = Vector3.Distance(cameraTransform.position, targetPos);
            yield return null;
        }

        cameraTransform.position = targetPos;
        cameraTransform.rotation = targetRot;
    }
}