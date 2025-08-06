using UnityEngine;

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

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        cc = GetComponent<CharacterController>();
        currentHealth = maxHealth;
    }

    void OnEnable()
    {
        spawnTime = Time.time;
        ApplyDifficultySettings();
        // currentHealth = maxHealth;  // Reset health on respawn
    }

    void Update()
    {
        if (!IsAlive())
        {
            Debug.Log("Seeker died!");
            ObjectPool.Instance.ReturnToPool(gameObject);
            audioManager.PlaySFX(audioManager.enemyDeath);
            return;  // Stop update if dead
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

        var difficulty = DifficultyManager.Instance.currentDifficulty;

        // Easy: enemies have low HP, move slower, do less damage
        // Normal: your current default behavior
        // Hard: more HP, move faster, hit harder
        switch (difficulty)
        {
            case DifficultyManager.Difficulty.Easy:
                maxHealth = 3;
                speed = 1.5f;
                break;

            case DifficultyManager.Difficulty.Normal:
                maxHealth = 5;
                speed = 2f;
                break;

            case DifficultyManager.Difficulty.Hard:
                maxHealth = 8;
                speed = 2.8f;
                break;
        }

        currentHealth = maxHealth;
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
