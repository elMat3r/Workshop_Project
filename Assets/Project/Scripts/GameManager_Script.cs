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
        Horda_Manager horda = FindObjectOfType<Horda_Manager>();
        if (horda != null && horda.activeRobots.Count > 0)
        {
            playerTransform = horda.activeRobots[0].transform;
            initialPosX = playerTransform.position.x;
        }
    }
    void Update()
    {
        if (isGameActive && playerTransform != null)
        {
            float diferenciaX = playerTransform.position.x - initialPosX;
            distanciaRecorrida = Mathf.Max(0, Mathf.FloorToInt(diferenciaX));
            if (textDistanceGame != null)
            {
                textDistanceGame.text = distanciaRecorrida + "m";
            }
        }
        else if (isGameActive && playerTransform == null)
        {
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
        isGameActive = true;
    }
    public void ActivarGameOver()
    {
        isGameActive = false;
        Time.timeScale = 0f;
        if (textDistanceFinal != null)
        {
            textDistanceFinal.text = "Distancia lograda: " + distanciaRecorrida + "m";
        }
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