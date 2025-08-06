using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    public TMP_Dropdown difficultyDropdown;
    public Button playButton;

    void Awake()
    {
        Debug.Log("MainMenuHandler Awake");
        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Debug.Log("Start called on MainMenuHandler");

        if (playButton == null)
        {
            GameObject buttonObj = GameObject.Find("PlayButton");
            if (buttonObj != null)
            {
                playButton = buttonObj.GetComponent<Button>();
                Debug.Log("Found PlayButton at runtime.");
            }
            else
            {
                Debug.LogError("❌ Couldn't find PlayButton GameObject in scene!");
            }
        }

        if (playButton != null)
        {
            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(PlayGame);
            Debug.Log("✅ Play button listener added.");
        }

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //set default selection
        difficultyDropdown.value = (int)DifficultyManager.Instance.currentDifficulty;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Manual PlayGame() trigger with 'P' key.");
            PlayGame();
        }
    }

    public void OnDifficultyChanged(int index)
    {
        DifficultyManager.Instance.SetDifficulty(index);
    }

    public void PlayGame()
    {
        Debug.Log("Play button clicked"); // <== Confirm this shows up in console
        WaveManager.CurrentWave = 1; // 🔧 Reset the static wave value
        Time.timeScale = 1f;
        GameObject music = GameObject.Find("MainMenuMusic");
        if (music != null)
        {
            Destroy(music);
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene("GamePlay");
    }

    private void OnEnable()
    {
        Debug.Log("MainMenuHandler Enabled");

        // Clear and re-assign Play button click listener
        if (playButton != null)
        {
            playButton.onClick.RemoveAllListeners(); // Prevent duplicates
            playButton.onClick.AddListener(PlayGame);
        }

        // Update dropdown if needed
        if (difficultyDropdown != null)
            difficultyDropdown.value = (int)DifficultyManager.Instance.currentDifficulty;
        }
}
