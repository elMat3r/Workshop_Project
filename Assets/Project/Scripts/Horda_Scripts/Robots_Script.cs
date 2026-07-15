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
    private CapsuleCollider capsuleCollider;
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
        if (proceduralMovement != null)
        {
            proceduralMovement.enabled = false;
        }
    }
    public void StartRunning()
    {
        if (proceduralMovement != null)
        {
            proceduralMovement.enabled = true;
        }
        if (anim != null)
        {
            anim.SetBool("gameStarted", true);
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
        if (anim != null)
        {
            anim.SetTrigger("Dash");
        }
        if (capsuleCollider != null)
        {
            capsuleCollider.height = originalColliderHeight * 0.5f;
            capsuleCollider.center = originalColliderCenter + new Vector3(0, -(originalColliderHeight * 0.25f), 0);
        }
        yield return new WaitForSeconds(dashDuration);
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