using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{

    public Creature playerCreature;
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!playerCreature.IsAlive())
        {
            Debug.Log("Game Over!");
            audioManager.PlaySFX(audioManager.playerDeath);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 1f; // Ensure time resumes
            SceneManager.LoadScene("MainMenu"); //Reload Specific scene
            // SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Reloads current scene
        }

    }
    

    
    
}
