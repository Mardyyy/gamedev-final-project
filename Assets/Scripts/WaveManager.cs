using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class WaveManager : MonoBehaviour
{
    private List<GameObject> spawnedObjects = new List<GameObject>();
    private bool waveOverTriggered = false;
    public SpawnMonsters spawnMonsters;

    private int waveNumber = 0;
    public TMPro.TextMeshProUGUI waveText;

    AudioManager audioManager;

    private DifficultyManager.Difficulty difficulty;
    public static int CurrentWave { get; private set; }



    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        

    }

    public void AddSpawnedObject(GameObject obj)
    {
        if (obj != null)
        {
            Debug.Log($"Tracking spawned enemy: {obj.name}");
            spawnedObjects.Add(obj);
        }
        else
        {
            Debug.LogWarning("Attempted to track a null enemy!");
        }
    }

    void Start()
    {
        if (DifficultyManager.Instance != null)
        {
            difficulty = DifficultyManager.Instance.currentDifficulty;
        }
        else
        {
            Debug.LogError("DifficultyManager.Instance is null!");
        }

        Debug.Log("Current Difficulty: " + difficulty);

        if (spawnMonsters == null)
            Debug.LogWarning("spawnMonsters not assigned in WaveManager!");
        else
            spawnMonsters.SpawnAtRandomPoint(1); // Start first wave

        // Display initial wave number
        if (waveText != null)
            waveText.text = "Wave 1"; // Start display at wave 1 since first wave spawns as wave 1
        waveNumber = 1;
        CurrentWave = waveNumber;

    }

    void Update()
    {
        if (waveOverTriggered || spawnedObjects.Count == 0) return;

        bool allEnemiesInactive = true;

        foreach (var obj in spawnedObjects)
        {
            if (obj != null && obj.activeInHierarchy)
            {
                allEnemiesInactive = false;
                break;
            }
        }

        if (allEnemiesInactive)
        {
            Debug.Log("Triggering WaveOverCoroutine...");
            waveOverTriggered = true;
            StartCoroutine(WaveOverCoroutine());
        }
    }

    IEnumerator WaveOverCoroutine()
    {
        yield return new WaitForSeconds(0.5f); // small delay to avoid race conditions

        waveNumber++;
        CurrentWave = waveNumber;

        // Update wave number display
        if (waveText != null)
        {
            waveText.text = "Wave " + waveNumber;
        }

        Debug.Log($"Wave {waveNumber} complete. All enemies destroyed. Next wave in 3 seconds...");
        audioManager.PlayRoundSFX(audioManager.roundEnd);
        yield return new WaitForSeconds(7f);

        
        spawnedObjects.Clear();

        // int enemiesToSpawn = 3 + (waveNumber - 1) * 2;
        int enemiesToSpawn;
        switch (difficulty)
        {
            case DifficultyManager.Difficulty.Easy:
                enemiesToSpawn = 2 + (waveNumber - 1) * 1; // slower ramp up
                break;
            case DifficultyManager.Difficulty.Normal:
                enemiesToSpawn = 3 + (waveNumber - 1) * 2; // current behavior
                break;
            case DifficultyManager.Difficulty.Hard:
                enemiesToSpawn = 5 + (waveNumber - 1) * 3; // faster ramp up
                break;
            default:
                enemiesToSpawn = 3 + (waveNumber - 1) * 2;
                break;
        }


        if (ObjectPool.Instance != null && ObjectPool.Instance.prefab != null)
        {
            if (enemiesToSpawn > ObjectPool.Instance.poolSize)
            {
                int extraNeeded = enemiesToSpawn - ObjectPool.Instance.poolSize;
                ObjectPool.Instance.ExpandPool(extraNeeded);
            }
        }
        else
        {
            Debug.LogWarning("ObjectPool.Instance or prefab is null. Cannot expand pool.");
        }

        
        audioManager.PlayRoundSFX(audioManager.roundStart);
        yield return new WaitForSeconds(3f);

        spawnMonsters.SpawnAtRandomPoint(waveNumber);

        waveOverTriggered = false;
    }
}
