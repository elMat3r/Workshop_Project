using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_ObstacleCollision : MonoBehaviour
{
    private Player_Movement_Fisico player_Movement;
    private Rigidbody rb;
    private bool isDead = false;
    public Chunks_Pooling_Script chunkPooling;
    private LifeManager lifeManager;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player_Movement = GetComponent<Player_Movement_Fisico>();
    }
    private void Start()
    {
        if (chunkPooling == null)
        {
            chunkPooling = FindObjectOfType<Chunks_Pooling_Script>();
        }
        lifeManager = FindFirstObjectByType<LifeManager>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && !isDead)
        {
            Die();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle") && !isDead)
        {
            Die();
            return;
        }
        if (other.CompareTag("TriggerChunk"))
        {
            if (chunkPooling != null)
            {
                chunkPooling.SpawnNewChunk();
            }
        }
    }
    private void Die()
    {
        isDead = true;
        if (player_Movement != null)
        {
            player_Movement.enabled = false;
        }
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        if (lifeManager != null)
        {
            lifeManager.LoseLife();
        }
        else
        {
            Destroy(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    public void ResetRobot()
    {
        isDead = false;
        if (player_Movement != null)
        {
            player_Movement.enabled = true;
        }
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}