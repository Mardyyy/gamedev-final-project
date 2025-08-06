using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance;

    public enum Difficulty { Easy, Normal, Hard }
    public Difficulty currentDifficulty = Difficulty.Normal;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep it across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetDifficulty(int difficultyIndex)
    {
        currentDifficulty = (Difficulty)difficultyIndex;
        Debug.Log("Difficulty set to: " + currentDifficulty);
    }
}
