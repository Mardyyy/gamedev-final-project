using UnityEngine;

public class GameplayMusicPlayer : MonoBehaviour
{
    public AudioSource musicSource;

    void Start()
    {
        if (musicSource != null && !musicSource.isPlaying)
        {
            musicSource.Play();
        }
        musicSource.volume = 0.5f; // Half volume
    }
}
