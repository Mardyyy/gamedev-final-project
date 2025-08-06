using UnityEngine;
using UnityEngine.UI;
using TMPro; // If using TextMeshPro

public class Seeker : MonoBehaviour
{
    public Transform targetTransform;
    public int maxHealth = 5;   // Original max health, editable in Inspector
    public float speed = 2f;
    public float minDistance = 0.9f;

    CharacterController cc;
    public float gravity = -40f;
    private Vector3 currentGravity = Vector3.zero;
    private float spawnTime;

    private int currentHealth;

    AudioManager audioManager;

    //----------Health Bar/Slider
    public GameObject healthBarPrefab;
    private GameObject healthBarInstance;
    private Slider healthSlider;
    private TextMeshProUGUI healthText; // or use Text if you're not using TMP

    public Vector3 healthBarOffset = new Vector3(0, 2f, 0);

    //---------------


    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        cc = GetComponent<CharacterController>();
        currentHealth = maxHealth;

        // if (healthBarInstance != null)
        // {
        //     // Scale the entire health bar
        //     //healthBarInstance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); // Adjust as needed
        // }

    }

    void OnEnable()
    {
        spawnTime = Time.time;
        ApplyDifficultySettings();
        currentHealth = maxHealth;

        // Clean up old health bar if somehow still present
        if (healthBarInstance != null)
        {
            Destroy(healthBarInstance);
            healthBarInstance = null;
        }

        // Spawn new health bar
        if (healthBarPrefab != null)
        {
            healthBarInstance = Instantiate(healthBarPrefab, transform.position + healthBarOffset, Quaternion.identity);

            healthSlider = healthBarInstance.GetComponentInChildren<Slider>();
            healthText = healthBarInstance.GetComponentInChildren<TextMeshProUGUI>();
        }

        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (healthText != null)
        {
            healthText.text = $"{currentHealth} / {maxHealth}";
        }

        UpdateHealthUI();

    }

    private void OnDisable()
    {
        if (healthBarInstance != null)
        {
            Destroy(healthBarInstance);
            healthBarInstance = null;
        }
    }


    void Update()
    {
        if (!IsAlive())
        {
            Debug.Log("Seeker died!");

            if (healthBarInstance != null)
            {
                Destroy(healthBarInstance);
            }

            ObjectPool.Instance.ReturnToPool(gameObject);
            audioManager.PlaySFX(audioManager.enemyDeath);
            return;  // Stop update if dead
        }

        // Follow enemy
        if (healthBarInstance != null)
        {
            // healthBarInstance.transform.position = transform.position + Vector3.up * 2f;
            healthBarInstance.transform.position = transform.position + healthBarOffset;


            // Optionally face camera
            Camera cam = Camera.main;
            if (cam != null)
                healthBarInstance.transform.LookAt(healthBarInstance.transform.position + cam.transform.forward);
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Seeker hit something??");
        // if (other.GetComponent<Creature>() != null)
        // {
        //     other.GetComponent<Creature>().TakeDamage(1);
        // }

        if (other.GetComponent<Creature>() != null)
        {
            int damage = 1;

            if (DifficultyManager.Instance != null)
            {
                switch (DifficultyManager.Instance.currentDifficulty)
                {
                    case DifficultyManager.Difficulty.Easy:
                        damage = 1;
                        break;
                    case DifficultyManager.Difficulty.Normal:
                        damage = 2;
                        break;
                    case DifficultyManager.Difficulty.Hard:
                        damage = 3;
                        break;
                }
            }

            other.GetComponent<Creature>().TakeDamage(damage);
        }

    }

    void ApplyDifficultySettings()
    {
        if (DifficultyManager.Instance == null)
        {
            Debug.LogWarning("DifficultyManager not found!");
            return;
        }

        int round = Mathf.Max(1, WaveManager.CurrentWave);
        var difficulty = DifficultyManager.Instance.currentDifficulty;

        // Easy: enemies have low HP, move slower, do less damage
        // Normal: your current default behavior
        // Hard: more HP, move faster, hit harder
        switch (difficulty)
        {
            case DifficultyManager.Difficulty.Easy:
                maxHealth = 2 + round; // +1 health per wave
                speed = 1.5f;
                break;

            case DifficultyManager.Difficulty.Normal:
                maxHealth = 3 + round * 2; // +2 health per wave
                speed = 2f;
                break;

            case DifficultyManager.Difficulty.Hard:
                maxHealth = 5 + round * 3; // +3 health per wave
                speed = 2.8f;
                break;
        }

        currentHealth = maxHealth;
        
        
    }

    void UpdateHealthUI()
    {
        if (healthSlider != null)
            healthSlider.maxValue = maxHealth;

        if (healthSlider != null)
            healthSlider.value = currentHealth;

        if (healthText != null)
            healthText.text = $"{currentHealth} / {maxHealth}";
    }


    public void SimulateGravity()
    {
        currentGravity.y += gravity * Time.deltaTime;
        cc.Move(currentGravity * Time.deltaTime);

        if (cc.isGrounded && currentGravity.y < 0)
        {
            currentGravity.y = -2f; // keeps you grounded
        }
    }

    public void TakeDamage(int damage)
    {
        if (Time.time - spawnTime < 1f) return; // Ignore damage first second

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            // Optionally trigger death animation or events here
        }
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
        if (healthText != null)
        {
            healthText.text = $"{currentHealth} / {maxHealth}";
        }
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    public void SetHealth(int newHealth)
    {
        currentHealth = newHealth;
    }
}
