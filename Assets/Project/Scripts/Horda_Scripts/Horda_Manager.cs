using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Horda_Manager : MonoBehaviour
{
    public static Horda_Manager Instance;
    public GameObject robotPrefab;
    public int minRobotsToDestroyObstacle = 3;
    public List<GameObject> activeRobots = new List<GameObject>();
    [Header("Control de Velocidad Global")]
    public float velocidadInicial = 6f;
    public float velocidadMaxima = 18f;
    public float aceleracionPorSegundo = 0.1f;
    public float VelocidadActualDelJuego { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        VelocidadActualDelJuego = velocidadInicial;
    }
    private void Start()
    {
        GameObject initialRobot = GameObject.FindGameObjectWithTag("Player");
        if (initialRobot != null)
        {
            activeRobots.Add(initialRobot);
        }
    }
    void Update()
    {
        if (activeRobots.Count > 0)
        {
            VelocidadActualDelJuego += aceleracionPorSegundo * Time.deltaTime;
            VelocidadActualDelJuego = Mathf.Clamp(VelocidadActualDelJuego, velocidadInicial, velocidadMaxima);
        }
        if (activeRobots.Count > 0 && activeRobots[0] != null)
        {
            transform.position = new Vector3(activeRobots[0].transform.position.x, transform.position.y, transform.position.z);
        }
    }
    public void AddRobots(Vector3 spawnPosition)
    {
        Vector3 spawnPos = spawnPosition - new Vector3(1.2f, 0f, 0f);
        GameObject newRobot = Instantiate(robotPrefab, spawnPos, Quaternion.identity);
        activeRobots.Add(newRobot);

        if (activeRobots.Count > 1 && activeRobots[0] != null)
        {
            Rigidbody leaderRb = activeRobots[0].GetComponent<Rigidbody>();
            Rigidbody cloneRb = newRobot.GetComponent<Rigidbody>();
            if (leaderRb != null && cloneRb != null)
            {
                cloneRb.linearVelocity = new Vector3(leaderRb.linearVelocity.x, cloneRb.linearVelocity.y, cloneRb.linearVelocity.z);
            }
        }
    }
    public void RemoveRobot(GameObject robotRemoved)
    {
        if (activeRobots.Contains(robotRemoved))
        {
            activeRobots.Remove(robotRemoved);
            Destroy(robotRemoved);
        }
        if (activeRobots.Count == 0)
        {
            GameManager.Instance.ActivarGameOver();
        }
    }
    public void CommandJump()
    {
        for (int i = activeRobots.Count - 1; i >= 0; i--)
        {
            if (activeRobots[i] == null)
            {
                activeRobots.RemoveAt(i);
                continue;
            }
            activeRobots[i].GetComponent<Robots_Script>()?.Jump();
        }
    }
    public void CommandDash()
    {
        for (int i = activeRobots.Count - 1; i >= 0; i--)
        {
            if (activeRobots[i] == null)
            {
                activeRobots.RemoveAt(i);
                continue;
            }
            activeRobots[i].GetComponent<Robots_Script>()?.Dash();
        }
    }
    private void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}