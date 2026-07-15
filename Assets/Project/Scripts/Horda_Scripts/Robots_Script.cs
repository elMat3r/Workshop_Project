using System.Collections;
using UnityEngine;

public class Robots_Script : MonoBehaviour
{
    public Player_Movement_Fisico proceduralMovement;

    [Header("Físicas")]
    public float jumpForce;
    public float gravityMultiplier;
    private bool isGrounded;

    [Header("Dash / Agacharse")]
    public float dashDuration;
    private bool isDashing = false;

    // === NUEVAS VARIABLES PARA EL COLLIDER ===
    private CapsuleCollider capsuleCollider; // O BoxCollider, usa el que tengas puesto
    private float originalColliderHeight;
    private Vector3 originalColliderCenter;

    private Rigidbody rb;
    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        if (capsuleCollider != null)
        {
            originalColliderHeight = capsuleCollider.height;
            originalColliderCenter = capsuleCollider.center;
        }
        else
        {
            Debug.LogError("¡El robot necesita un CapsuleCollider para agacharse físicamente!");
        }

        if (proceduralMovement != null)
        {
            proceduralMovement.enabled = false;
        }
    }
    public void StartRunning()
    {
        if (proceduralMovement != null)
        {
            proceduralMovement.enabled = true; // Empieza la física de correr
        }

        if (anim != null)
        {
            anim.SetBool("gameStarted", true); // Pasa de Idle a Run
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

            if (anim != null)
            {
                anim.ResetTrigger("Dash");
                anim.SetTrigger("Jump");
                anim.SetBool("isGrounded", false);
            }
        }
    }

    public void Dash()
    {
        // Forzamos el descenso si estamos en el aire
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

        // 1. Activamos la animación visual de agacharse en Mixamo
        if (anim != null)
        {
            anim.SetTrigger("Dash");
        }

        // === AQUÍ ESTÁ EL CAMBIO CLAVE: NO TOCAMOS EL TRANSFORM.SCALE ===
        // En su lugar, encogemos el Collider físicamente a la mitad

        if (capsuleCollider != null)
        {
            // Hacemos el collider más bajito (ej: 50% de su altura original)
            capsuleCollider.height = originalColliderHeight * 0.5f;

            // Ajustamos el centro del collider para que no flote (lo bajamos la mitad de lo que encogimos)
            capsuleCollider.center = originalColliderCenter + new Vector3(0, -(originalColliderHeight * 0.25f), 0);
        }

        // Esperamos la duración del dash
        yield return new WaitForSeconds(dashDuration);

        // === RESTAURAMOS EL COLLIDER FÍSICO ===
        if (capsuleCollider != null)
        {
            capsuleCollider.height = originalColliderHeight;
            capsuleCollider.center = originalColliderCenter;
        }

        isDashing = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;

            if (anim != null)
            {
                anim.SetBool("isGrounded", true);
            }
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (Horda_Manager.Instance != null)
            {
                Horda_Manager.Instance.RemoveRobot(gameObject);
            }
            else
            {
                Horda_Manager managerEnEscena = FindObjectOfType<Horda_Manager>();
                if (managerEnEscena != null)
                {
                    managerEnEscena.RemoveRobot(gameObject);
                }
                else
                {
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