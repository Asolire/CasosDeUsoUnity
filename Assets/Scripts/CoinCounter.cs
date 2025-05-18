using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinCounter : MonoBehaviour
{
    public static CoinCounter Instance;

    public int totalCoins = 3;
    public TextMeshProUGUI coinText;

    private int currentCoins;

    public LevelEventController levelEventController;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            gameObject.SetActive(false);
    }

    void Start()
    {
        currentCoins = totalCoins;
        UpdateUI();
    }

    public void CollectCoin()
    {
        currentCoins--;
        UpdateUI();

        if (currentCoins <= 0)
        {
            Debug.Log("¡Todas las monedas recolectadas!");

            if (levelEventController != null)
                levelEventController.TriggerBarricadeEvent();
        }
    }

    void UpdateUI()
    {
        if (coinText != null)
        {
            coinText.text = "Monedas restantes: " + currentCoins;
        }
    }

    public void RestaurarContador()
    {
        currentCoins = 3;
        UpdateUI();
    }
}
