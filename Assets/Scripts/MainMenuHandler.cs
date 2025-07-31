using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
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
