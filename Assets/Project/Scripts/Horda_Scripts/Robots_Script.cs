using System.Collections;
using UnityEngine;

public class Robots_Script : MonoBehaviour
{
    public Player_Movement proceduralMovement;

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
        if (proceduralMovement == null)
        {
            proceduralMovement = GetComponent<Player_Movement>();
        }
        if (proceduralMovement != null)
        {
            proceduralMovement.enabled = true;
        }
    }
    private void FixedUpdate()
    {
        if(rb.linearVelocity.y < 0f)
        {
            rb.AddForce(Vector3.down * gravityMultiplier, ForceMode.Acceleration);
        }
    }
    public void Jump()
    {
        if(isGrounded && !isDashing)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }
    public void Dash()
    {
        if(!isGrounded)
        {
            rb.AddForce(Vector3.down * 20f, ForceMode.Impulse);
        }
        if(!isDashing)
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
            Horda_Manager.Instance.RemoveRobot(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Robots"))
        {
            Horda_Manager.Instance.AddRobots(other.transform.position);
            Destroy(other.gameObject);
        }
    }
}
