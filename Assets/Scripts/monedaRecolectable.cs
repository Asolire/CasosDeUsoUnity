using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monedaRecolectable : MonoBehaviour
{
    public AudioClip collectSound;
    public float bounceHeight = 2f;
    public float shrinkDuration = 0.5f;
    public float bounceDuration = 0.3f;

    private AudioSource audioSource;
    private bool isCollected = false;

    private Vector3 originalPosition;
    private Vector3 originalScale;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // Si no hay uno ya en el objeto, lo agregamos
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        originalPosition = transform.position;
        originalScale = transform.localScale;
    }
    
    private void OnEnable()
    {
        // Al activarse, restaurar estado si había sido recolectada
        if (isCollected)
        {
            transform.position = originalPosition;
            transform.localScale = originalScale;
            isCollected = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCollected) return;

        if (other.CompareTag("Player"))
        {
            isCollected = true;
            StartCoroutine(CollectEffect());

            // Notificar al contador global (lo veremos luego)
            CoinCounter.Instance.CollectCoin();
        }
    }

    private System.Collections.IEnumerator CollectEffect()
    {
        // Reproducir sonido
        if (collectSound != null)
        {
            audioSource.PlayOneShot(collectSound);
        }

        Vector3 startPos = transform.position;
        Vector3 peakPos = startPos + Vector3.up * bounceHeight;
        Vector3 endPos = startPos;

        float elapsed = 0f;

        // Rebote hacia arriba
        while (elapsed < bounceDuration)
        {
            transform.position = Vector3.Lerp(startPos, peakPos, elapsed / bounceDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Caida hacia abajo
        elapsed = 0f;
        while (elapsed < bounceDuration)
        {
            transform.position = Vector3.Lerp(peakPos, endPos, elapsed / bounceDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Encogimiento
        elapsed = 0f;
        Vector3 originalScale = transform.localScale;
        while (elapsed < shrinkDuration)
        {
            float scale = Mathf.Lerp(1f, 0f, elapsed / shrinkDuration);
            transform.localScale = originalScale * scale;
            elapsed += Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
