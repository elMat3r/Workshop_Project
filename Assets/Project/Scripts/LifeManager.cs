using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LifeManager : MonoBehaviour
{
    [Header("Configuración de Vidas")]
    public int maxLives = 3;
    private int currentLives;
    [Header("Referencias UI: Paneles")]
    public GameObject retryPanel;
    public GameObject adPanel;
    [Header("Referencias UI: Anuncio")]
    public TMP_Text countdownText;
    public Button claimButton;
    [Header("Personalización del Anuncio")]
    [Tooltip("El componente Image del panel de anuncios que cambiará de fondo.")]
    public Image adBackgroundImage;
    [Tooltip("Arrastra aquí todos los Sprites de anuncios diferentes que quieras mostrar.")]
    public Sprite[] adImages;
    [Header("Referencias del Juego")]
    public Player_Movement_Fisico playerMovement;
    void Start()
    {
        currentLives = PlayerPrefs.GetInt("CurrentLives", maxLives);
        Time.timeScale = 1f;
        if (retryPanel != null) retryPanel.SetActive(false);
        if (adPanel != null) adPanel.SetActive(false);
        if (claimButton != null) claimButton.gameObject.SetActive(false);
    }
    public void LoseLife()
    {
        currentLives--;
        PlayerPrefs.SetInt("CurrentLives", currentLives);
        PlayerPrefs.Save();
        Time.timeScale = 0f;
        if (playerMovement != null) playerMovement.enabled = false;
        if (currentLives > 0)
        {
            if (retryPanel != null) retryPanel.SetActive(true);
            if (adPanel != null) adPanel.SetActive(false);
        }
        else
        {
            if (retryPanel != null) retryPanel.SetActive(false);
            StartFakeAdFlow();
        }
    }
    public void RetryLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void StartFakeAdFlow()
    {
        if (adImages != null && adImages.Length > 0 && adBackgroundImage != null)
        {
            int randomIndex = Random.Range(0, adImages.Length);
            adBackgroundImage.sprite = adImages[randomIndex];
        }
        if (adPanel != null) adPanel.SetActive(true);
        if (claimButton != null) claimButton.gameObject.SetActive(false);
        StartCoroutine(AdCountdownRoutine());
    }
    IEnumerator AdCountdownRoutine()
    {
        float totalDuration = 60f;
        float timeElapsed = 0f;
        while (timeElapsed < totalDuration)
        {
            float timeLeft = totalDuration - timeElapsed;
            if (countdownText != null)
                countdownText.text = $"El anuncio terminará en: {(int)timeLeft}s";
            if (timeElapsed >= 30f && claimButton != null && !claimButton.gameObject.activeSelf)
            {
                claimButton.gameObject.SetActive(true);
                claimButton.GetComponentInChildren<TMP_Text>().text = "¡Reclamar 3 Vidas!";
            }
            yield return new WaitForSecondsRealtime(1f);
            timeElapsed++;
        }
        if (countdownText != null) countdownText.text = "¡Anuncio finalizado!";
        if (claimButton != null) claimButton.gameObject.SetActive(true);
    }
    public void RecoverLivesAndResume()
    {
        StopAllCoroutines();
        PlayerPrefs.SetInt("CurrentLives", maxLives);
        PlayerPrefs.Save();
        if (adPanel != null) adPanel.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    [ContextMenu("Resetear Vidas Manualmente")]
    public void ResetVidasInspector()
    {
        PlayerPrefs.SetInt("CurrentLives", 3);
        PlayerPrefs.Save();
        currentLives = 3;
    }
}