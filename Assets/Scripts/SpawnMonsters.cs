using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnMonsters : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject objectToSpawn;
    public int baseObjectsToSpawn = 3;
    public WaveManager waveChecker;

    void Start()
    {
        //SpawnAtRandomPoint(1);
    }

    public void SpawnAtRandomPoint(int waveNumber)
    {
        Debug.Log($"SpawnAtRandomPoint called for wave {waveNumber}");
        if (spawnPoints.Length == 0 || objectToSpawn == null)
        {
            Debug.LogWarning("No spawn points or object assigned!");
            return;
        }

        int objectsToSpawn = baseObjectsToSpawn + (waveNumber - 1) * 2;

        for (int i = 0; i < objectsToSpawn; i++)
        {
            int chosenIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[chosenIndex];

            //helps avoid overlapping the floor or another collider on spawn
            Vector3 safeSpawnPos = spawnPoint.position + Vector3.up * 0.5f;
            GameObject obj = ObjectPool.Instance.GetFromPool(safeSpawnPos, spawnPoint.rotation);

            if (obj != null)
            {
                obj.SetActive(true);

                if (waveChecker != null)
                {
                    waveChecker.AddSpawnedObject(obj);
                    Debug.Log($"Spawned enemy at spawn point {chosenIndex}");
                }
                else
                {
                    Debug.LogWarning("waveChecker is null! Cannot track spawned object.");
                }
            }
            else
            {
                Debug.LogWarning("Spawned object is null!");
            }
        }
    }
}
