using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuHandler : MonoBehaviour
{
    public TMP_Dropdown difficultyDropdown;

    private void Start()
    {
        //set default selection
        difficultyDropdown.value = (int)DifficultyManager.Instance.currentDifficulty;
    }

    public void OnDifficultyChanged(int index)
    {
        DifficultyManager.Instance.SetDifficulty(index);
    }

    public void PlayGame()
    {
        Debug.Log("PlayGame!");
        GameObject music = GameObject.Find("MainMenuMusic");
        if (music != null)
        {
            Destroy(music);
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene("GamePlay");
    }
}
