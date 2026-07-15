using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Jump : MonoBehaviour
{
    public float jumpForce;
    public float gravityPlayer;
    public float fastFall;
    private Rigidbody rb;
    private bool isGrounded;
    public float dashDuration;
    private Vector3 originalScale;
    private bool isDashing = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalScale = transform.localScale;
    }
    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded && !isDashing)
        {
            JumpPerform();
        }
    }
    public void JumpPerform()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(Vector3.up *  jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }
    public void OnDash(InputValue value)
    {
        if (value.isPressed)
        {
            if (!isGrounded)
            {
                rb.AddForce(Vector3.down * fastFall, ForceMode.Impulse);
            }
            if (!isDashing)
            {
                StartCoroutine(DashRoutine());
            }
        }
    }
    private IEnumerator DashRoutine()
    {
        isDashing = true;
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
    }
    private void FixedUpdate()
    {
        if(rb.linearVelocity.y < 0)
        {
            rb.AddForce(Vector3.down * gravityPlayer, ForceMode.Acceleration);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
