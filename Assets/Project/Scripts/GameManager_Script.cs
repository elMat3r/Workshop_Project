using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Paneles de la Interfaz")]
    public GameObject startGamePanel;
    public GameObject retryGamePanel;

    [Header("Textos de Distancia (TMP)")]
    public TextMeshProUGUI textDistanceGame;
    public TextMeshProUGUI textDistanceFinal;

    [Header("Referencia al Jugador")]
    private Transform playerTransform;
    private float initialPosX;
    private int distanciaRecorrida;
    private bool isGameActive = false;

    void Awake()
    {
        Instance = this;

        Time.timeScale = 0f;

        if (startGamePanel != null) startGamePanel.SetActive(true);
        if (retryGamePanel != null) retryGamePanel.SetActive(false);
    }

    void Start()
    {
        // Buscamos al Horda_Manager para obtener el primer robot (el líder)
        Horda_Manager horda = FindObjectOfType<Horda_Manager>();
        if (horda != null && horda.activeRobots.Count > 0)
        {
            playerTransform = horda.activeRobots[0].transform;
            initialPosX = playerTransform.position.x;
        }
    }

    void Update()
    {
        // Solo calculamos distancia si el juego ya arrancó y tenemos al líder vivo
        if (isGameActive && playerTransform != null)
        {
            // Calculamos la diferencia entre la posición actual en X y donde empezó
            float diferenciaX = playerTransform.position.x - initialPosX;

            // Convertimos a un número entero (sin decimales) para que se vea limpio
            distanciaRecorrida = Mathf.Max(0, Mathf.FloorToInt(diferenciaX));

            // Actualizamos el texto de la pantalla en tiempo real
            if (textDistanceGame != null)
            {
                textDistanceGame.text = distanciaRecorrida + "m";
            }
        }
        else if (isGameActive && playerTransform == null)
        {
            // Plan de respaldo: Si el líder original muere, intentamos buscar si queda otro robot vivo en la horda
            Horda_Manager horda = FindObjectOfType<Horda_Manager>();
            if (horda != null && horda.activeRobots.Count > 0 && horda.activeRobots[0] != null)
            {
                playerTransform = horda.activeRobots[0].transform;
            }
        }
    }

    public void IniciarJuego()
    {
        Time.timeScale = 1f;
        startGamePanel.SetActive(false);
        isGameActive = true; // Empezamos a contar la distancia
    }

    public void ActivarGameOver()
    {
        isGameActive = false; // Dejamos de contar
        Time.timeScale = 0f;

        // 1. Le pasamos el número final logrado al texto del panel de reinicio
        if (textDistanceFinal != null)
        {
            textDistanceFinal.text = "Distancia lograda: " + distanciaRecorrida + "m";
        }

        // 2. Ocultamos el marcador flotante para que no se superponga feo
        if (textDistanceGame != null)
        {
            textDistanceGame.gameObject.SetActive(false);
        }

        retryGamePanel.SetActive(true);
    }

    public void ReiniciarJuego()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}




//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class GameManager : MonoBehaviour
//{
//    public static GameManager Instance;

//    [Header("Paneles de la Interfaz")]
//    public GameObject startGamePanel;
//    public GameObject retryGamePanel;

//    void Awake()
//    {
//        Instance = this;
//        Time.timeScale = 0f;
//        if (startGamePanel != null) startGamePanel.SetActive(true);
//        if (retryGamePanel != null) retryGamePanel.SetActive(false);
//    }
//    public void IniciarJuego()
//    {
//        Time.timeScale = 1f;
//        startGamePanel.SetActive(false);
//    }
//    public void ActivarGameOver()
//    {
//        Time.timeScale = 0f;
//        retryGamePanel.SetActive(true);
//    }
//    public void ReiniciarJuego()
//    {
//        Time.timeScale = 1f;
//        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//    }
//}