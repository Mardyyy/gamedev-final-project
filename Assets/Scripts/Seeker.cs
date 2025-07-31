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

    void Start()
    {
        cc = GetComponent<CharacterController>();
        currentHealth = maxHealth;
    }

    void OnEnable()
    {
        spawnTime = Time.time;
        currentHealth = maxHealth;  // Reset health on respawn
    }

    void Update()
    {
        if (!IsAlive())
        {
            Debug.Log("Seeker died!");
            ObjectPool.Instance.ReturnToPool(gameObject);
            return;  // Stop update if dead
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Seeker hit something??");
        if (other.GetComponent<Creature>() != null)
        {
            other.GetComponent<Creature>().TakeDamage(1);
        }
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
