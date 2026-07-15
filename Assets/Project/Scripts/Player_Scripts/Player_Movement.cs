using UnityEngine;

public class Player_Movement_Fisico : MonoBehaviour
{
    private Rigidbody rb;
    private Horda_Manager hordaManager;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        hordaManager = FindObjectOfType<Horda_Manager>();
    }
    void FixedUpdate()
    {
        if (hordaManager != null && rb != null)
        {
            float velocidadSincronizada = hordaManager.VelocidadActualDelJuego;
            rb.linearVelocity = new Vector3(velocidadSincronizada, rb.linearVelocity.y, rb.linearVelocity.z);
        }
    }
}