using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{

    public Creature playerCreature;
    public GameObject deathScreenUI;
    private bool deathHandled = false;
    AudioManager audioManager;
    public GameObject gameplayHUD;


    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1f; // Ensure game is running
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerCreature.IsAlive() && !deathHandled)
        {
            HandleDeath();

        }

    }

    void HandleDeath()
    {
        deathHandled = true;
        Debug.Log("Game Over!");
        audioManager.PlayRoundSFX(audioManager.playerDeath);

        Time.timeScale = 0f; // Pause the game
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (gameplayHUD != null)
        {
            gameplayHUD.SetActive(false);
        }


        if (deathScreenUI != null)
        {
            deathScreenUI.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Death Screen UI not assigned in GameManager.");
        }
    }

    public void RestartGame()
    {
        WaveManager.CurrentWave = 1; //Reset the static wave value
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;

        //destroy persistent objects that shouldn't carry over
        GameObject music = GameObject.Find("MainMenuMusic");
        if (music != null) Destroy(music);

        //if (settingsPanel != null) settingsPanel.SetActive(false);
        if (deathScreenUI != null) deathScreenUI.SetActive(false);
        
        //if (mainMenuUI != null) mainMenuUI.SetActive(true);

        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    

    
    
}
