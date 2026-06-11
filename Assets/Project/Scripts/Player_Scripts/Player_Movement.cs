using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public float playerSpeed;
    private Rigidbody rb;
    private bool isDead = false;
    public AnimationCurve progressiveSpeed = AnimationCurve.Linear(0f, 5f, 60f, 20f);
    public bool loopSpeed = false;
    private float timeElapsed;
    private float currentSpeed;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        timeElapsed = 0f;
        if (playerSpeed <= 0.1f)
        {
            playerSpeed = 5f;
        }
    }
    private void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }

        float time = timeElapsed;

        if(loopSpeed)
        {
            float maxTime = progressiveSpeed.keys[progressiveSpeed.length - 1].time;
            time = timeElapsed % maxTime;
        }

        currentSpeed = progressiveSpeed.Evaluate(time) * playerSpeed;

        rb.linearVelocity = new Vector3(currentSpeed, rb.linearVelocity.y, rb.linearVelocity.z);

    }
    private void Update()
    {
        if (isDead)
        {
            return;
        }

        timeElapsed += Time.deltaTime;
    }
    public void StopMovement()
    {
        isDead = true;
        rb.linearVelocity = Vector3.zero;
    }

    //// DENTRO DE TU SCRIPT DE MOVIMIENTO INDIVIDUAL

    //private Rigidbody rb;
    //private Horda_Manager hordaManager;

    //void Start()
    //{
    //    rb = GetComponent<Rigidbody>();

    //    // Buscamos al manager en la escena una sola vez al nacer
    //    hordaManager = FindObjectOfType<Horda_Manager>();
    //}

    //void FixedUpdate()
    //{
    //    if (hordaManager != null)
    //    {
    //        // Leemos la velocidad global del juego directamente del manager
    //        float velocidadSincronizada = hordaManager.VelocidadActualDelJuego;

    //        // Aplicamos la velocidad directamente al Rigidbody
    //        // Mantenemos las físicas originales en Y (saltos) y Z (por si acaso)
    //        rb.linearVelocity = new Vector3(velocidadSincronizada, rb.linearVelocity.y, rb.linearVelocity.z);
    //    }
    //}
}
