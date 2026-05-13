using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public float playerSpeed;
    private Rigidbody rb;
    private bool isDead = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if(!isDead)
        {
            rb.linearVelocity = new Vector3(playerSpeed, rb.linearVelocity.y, rb.linearVelocity.z);
        }
    }
    public void StopMovement()
    {
        isDead = true;
        rb.linearVelocity = Vector3.zero;
    }
}
