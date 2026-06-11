using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Horda_Manager : MonoBehaviour
{
    public static Horda_Manager Instance;

    public GameObject robotPrefab;
    public int minRobotsToDestroyObstacle = 3;

    public List<GameObject> activeRobots = new List<GameObject>();

    private void Start()
    {
        GameObject initialRobot = GameObject.FindGameObjectWithTag("Player");
        if(initialRobot != null)
        {
            activeRobots.Add(initialRobot);
        }
    }
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
    }
    public void AddRobots(Vector3 spawnPosition)
    {
        //Vector3 spawnPos = spawnPosition - new Vector3 (1.5f, 0, 0);
        //GameObject newRobot = Instantiate(robotPrefab, spawnPos, Quaternion.identity);
        //activeRobots.Add (newRobot);

        Vector3 spawnPos = spawnPosition - new Vector3(1.2f, 0f, 0f);
        GameObject newRobot = Instantiate(robotPrefab, spawnPos, Quaternion.identity);
        activeRobots.Add(newRobot);
        if (activeRobots.Count > 1 && activeRobots[0] != null)
        {
            Player_Movement leaderMovement = activeRobots[0].GetComponent<Player_Movement>();
            Player_Movement cloneMovement = newRobot.GetComponent<Player_Movement>();

            if (leaderMovement != null && cloneMovement != null)
            {
                cloneMovement.playerSpeed = leaderMovement.playerSpeed;
            }
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
        if(activeRobots.Count == 0)
        {
            GameOver();
        } 
    }
    public void CommandJump()
    {
        //foreach (GameObject robot in activeRobots)
        //{
        //    robot.GetComponent<Robots_Script>()?.Jump();
        //}
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
        //foreach (GameObject robot in activeRobots)
        //{
        //    robot.GetComponent<Robots_Script>()?.Dash();
        //}
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
    void Update()
    {
        if (activeRobots.Count > 0 && activeRobots[0] != null)
        {
            transform.position = new Vector3(activeRobots[0].transform.position.x, transform.position.y, transform.position.z);
        }
    }
    //// DENTRO DE HORDA_MANAGER.CS

    //[Header("Configuración de Velocidad Global")]
    //public float velocidadInicial = 5f;
    //public float velocidadMaxima = 15f;
    //public float aceleracionPorSegundo = 0.05f; // Cuánto aumenta la velocidad cada segundo

    //// Esta es la variable pública que TODO el mundo va a leer
    //public float VelocidadActual del Juego { get; private set; }

    //void Awake()
    //{
    //    // Inicializamos la velocidad al arrancar
    //    VelocidadActualDelJuego = velocidadInicial;
    //}

    //void Update()
    //{
    //    // Si la horda no ha muerto, aumentamos la velocidad gradualmente con el tiempo
    //    if (activeRobots.Count > 0)
    //    {
    //        VelocidadActualDelJuego += aceleracionPorSegundo * Time.deltaTime;

    //        // Ponemos un tope para que el juego no se vuelva imposible matemáticamente
    //        VelocidadActualDelJuego = Mathf.Clamp(VelocidadActualDelJuego, velocidadInicial, velocidadMaxima);
    //    }
    //}
}