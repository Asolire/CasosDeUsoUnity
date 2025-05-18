using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReiniciarJuego : MonoBehaviour
{
    public GameObject pantallaReinicio;
    public Transform respawnPoint;
    public GameObject jugador;
    public GameObject barricadaSalida;
    public AudioSource musica;
    public CoinCounter contadorMonedas;
    public GameObject[] monedas; // array con todas las monedas

    void Start()
    {
        pantallaReinicio.SetActive(false);
    }
    public void Reiniciar()
    {

        
        // 1. Ocultar la UI
        pantallaReinicio.SetActive(false);

        // 2. Reposicionar al jugador
        jugador.transform.position = respawnPoint.position;

        // 3. Resetear la barricada
        barricadaSalida.transform.position = new Vector3(
            barricadaSalida.transform.position.x,
            0.834f, // posición Y original arriba
            barricadaSalida.transform.position.z
        );

        // 4. Reiniciar música
        musica.Stop();
        musica.Play();

        // 5. Resetear contador de monedas
        contadorMonedas.RestaurarContador();

        // 6. Reactivar monedas
        foreach (var moneda in monedas)
        {
            moneda.SetActive(true);
        }
    }
}