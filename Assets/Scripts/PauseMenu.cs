using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Audio;


public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public Button resumeButton;
    public Button quitButton;
    public Button optionsButton;
    public GameObject optionsMenuUI;
    public Button keyBindsButton;
    public GameObject keyBindsMenuUI;
    public TMPro.TMP_Dropdown resolutionDropdown;
    public static bool isPaused = false;
    public GameObject gameplayHUD;

    private Resolution[] resolutions;

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public AudioMixer audioMixer;
    void Start()
    {
        resumeButton.onClick.AddListener(Resume);
        quitButton.onClick.AddListener(QuitGame);
        optionsButton.onClick.AddListener(OpenOptions);
        keyBindsButton.onClick.AddListener(OpenKeyBinds);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //------------ResolutionOptions
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        //-------------------

        //Audio
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        //--------------------
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else if (!isPaused)
                Pause();
            else if (optionsMenuUI.activeSelf)
                BackFromOptions(); // ESC while in options returns to pause menu
            else if (keyBindsMenuUI.activeSelf)
                BackFromKeyBinds(); // ESC while in keybinds returns to options menu
        }
    }

    public void Resume()
    {
        Debug.Log("Resume clicked");
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        keyBindsMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        if (gameplayHUD != null)
        gameplayHUD.SetActive(true);

        // 🔒 Lock cursor again
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true); // only pause menu shows
        optionsMenuUI.SetActive(false); 
        keyBindsMenuUI.SetActive(false); 
        Time.timeScale = 0f;
        isPaused = true;

        if (gameplayHUD != null)
        gameplayHUD.SetActive(false);

        // Show cursor so player can click UI
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void QuitGame()
    {
        Debug.Log("Quit clicked");
        //Application.Quit();

        DisablePanel(optionsMenuUI);
        //if (deathScreenUI != null) deathScreenUI.SetActive(false);
        
        //if (mainMenuUI != null) mainMenuUI.SetActive(true);

        SceneManager.LoadScene("MainMenu");
    }

    void DisablePanel(GameObject panel)
    {
        if (panel == null) return;

        panel.SetActive(false);

        CanvasGroup cg = panel.GetComponent<CanvasGroup>();
        if (cg != null)
        {
            cg.interactable = false;
            cg.blocksRaycasts = false;
            cg.alpha = 0f;
        }
    }


    public void OpenOptions()
    {
        Debug.Log("Options clicked");
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(true);
    }
    public void BackFromOptions()
    {
        Debug.Log("Back from options");
        optionsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    public void OpenKeyBinds()
    {
        Debug.Log("KeyBinds clicked");
        optionsMenuUI.SetActive(false);
        keyBindsMenuUI.SetActive(true);
    }

    public void BackFromKeyBinds()
    {
        Debug.Log("Back from keybinds");
        keyBindsMenuUI.SetActive(false);
        optionsMenuUI.SetActive(true);
    }

    void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
    }

    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
    }

    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
    }   

}
