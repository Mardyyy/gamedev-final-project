using UnityEngine;

public class MainMenuMusicPlayer : MonoBehaviour
{
    public AudioSource audioSource;

    void Start()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        audioSource.volume = 0.5f; // Half volume
    }
}
