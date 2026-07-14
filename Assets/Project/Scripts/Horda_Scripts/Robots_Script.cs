using System.Collections;
using UnityEngine;

public class Robots_Script : MonoBehaviour
{
    public Player_Movement_Fisico proceduralMovement;

    public float jumpForce;
    public float gravityMultiplier;
    private bool isGrounded;

    public float dashDuration;
    private Vector3 originalScale;
    private bool isDashing = false;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        originalScale = transform.localScale;

        // CORREGIDO: Ahora busca el componente físico correcto en vez del viejo
        if (proceduralMovement == null)
        {
            proceduralMovement = GetComponent<Player_Movement_Fisico>();
        }

        if (proceduralMovement != null)
        {
            proceduralMovement.enabled = true;
        }
    }

    private void FixedUpdate()
    {
        if (rb.linearVelocity.y < 0f)
        {
            rb.AddForce(Vector3.down * gravityMultiplier, ForceMode.Acceleration);
        }
    }

    public void Jump()
    {
        if (isGrounded && !isDashing)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    public void Dash()
    {
        if (!isGrounded)
        {
            rb.AddForce(Vector3.down * 20f, ForceMode.Impulse);
        }
        if (!isDashing)
        {
            StartCoroutine(DashRoutine());
        }
    }

    private IEnumerator DashRoutine()
    {
        isDashing = true;
        transform.localScale = new Vector3(originalScale.x, originalScale.y * 0.5f, originalScale.z);
        yield return new WaitForSeconds(dashDuration);
        transform.localScale = originalScale;
        isDashing = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            // 1. Intentamos usar la Instancia estática si existe
            if (Horda_Manager.Instance != null)
            {
                Horda_Manager.Instance.RemoveRobot(gameObject);
            }
            else
            {
                // 2. PLAN DE RESPALDO: Si Instance es null, buscamos el manager activamente en la escena
                Horda_Manager managerEnEscena = FindObjectOfType<Horda_Manager>();

                if (managerEnEscena != null)
                {
                    managerEnEscena.RemoveRobot(gameObject);
                }
                else
                {
                    // 3. EMERGENCIA: Si de plano no existe el manager en la escena, 
                    // destruimos el robot de todos modos para que no sea inmortal.
                    Debug.LogError("¡No se encontró ningún Horda_Manager en la escena!");
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Robots"))
        {
            if (Horda_Manager.Instance != null)
            {
                Horda_Manager.Instance.AddRobots(other.transform.position);
            }
            Destroy(other.gameObject);
        }
    }
}