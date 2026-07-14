using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_ObstacleCollision : MonoBehaviour
{
    private Player_Movement_Fisico player_Movement;
    private Rigidbody rb;
    private bool isDead = false;
    public Chunks_Pooling_Script chunkPooling;

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
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && !isDead)
        {
            Die();
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
        if (Horda_Manager.Instance != null)
        {
            Horda_Manager.Instance.RemoveRobot(gameObject);
        }
        else
        {
            Destroy(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TriggerChunk"))
        {
            if (chunkPooling != null)
            {
                chunkPooling.SpawnNewChunk();
            }
            else
            {
                Debug.LogWarning("¡Se tocó un TriggerChunk pero no hay ninguna referencia a Chunks_Pooling_Script!");
            }
        }
    }
}