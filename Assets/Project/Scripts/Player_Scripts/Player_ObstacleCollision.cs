using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_ObstacleCollision : MonoBehaviour
{
    private Player_Movement player_Movement;
    private Rigidbody rb;
    private bool isDead = false;
    public Chunks_Pooling_Script chunkPooling;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Obstacle") && !isDead)
        {
            Die();
        }
    }
    private void Die()
    {
        isDead = true;

        if(player_Movement != null)
        {
            player_Movement.StopMovement();
        }
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Invoke("RestartGame", 2f);
    }
    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TriggerChunk"))
        {
            chunkPooling.SpawnNewChunk();
        }
    }
}
