using UnityEngine;

public class Player_Movement_Fisico : MonoBehaviour
{
    private Rigidbody rb;
    private Horda_Manager hordaManager;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Buscamos al manager en la escena al nacer
        hordaManager = FindObjectOfType<Horda_Manager>();

        if (hordaManager == null)
        {
            Debug.LogError("¡No se encontró el Horda_Manager en la escena! El player no sabrá a qué velocidad correr.");
        }
    }

    void FixedUpdate()
    {
        if (hordaManager != null && rb != null)
        {
            // 1. Leemos la velocidad actual y global del juego desde el manager
            float velocidadSincronizada = hordaManager.VelocidadActualDelJuego;

            // 2. Aplicamos la velocidad directamente al Rigidbody en el eje X
            // Mantenemos la velocidad actual en Y (para que los saltos/gravedad funcionen perfecto)
            // y en Z (por si tu juego tiene algo de profundidad)
            rb.linearVelocity = new Vector3(velocidadSincronizada, rb.linearVelocity.y, rb.linearVelocity.z);
        }
    }
}