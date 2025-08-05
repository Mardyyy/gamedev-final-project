using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Turret : MonoBehaviour
{
    [Header("Projectile Settings")]
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float projectileLifetime = 10f;
    public float projectileSpeed = 60f;
    public float shootCooldown = 0.1f;

    [Header("Arc Settings")]
    public int arcResolution = 60; //how many segments in the arc (higher = smoother arc)
    public float arcTimeStep = 0.1f; //time step between each point along the arc (adjust depending on range/speed)

    float shootTimer = 0f;

    private LineRenderer lineRenderer;
    private Vector3 gravity; //The arc uses Physics.gravity, so it behaves like the actual projectile would

    public bool turretEnabled = true;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //spawns at random positions within a range
        //transform.position = transform.position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));

        lineRenderer = GetComponent<LineRenderer>();
        gravity = Physics.gravity;

        //Configure LineRenderer appearance
        lineRenderer.widthMultiplier = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = Color.yellow;
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;

        // Draw the arc preview
        DrawArc();

        if (turretEnabled)
        {
            Debug.Log("Turret Enabled - waiting to shoot");
            if (shootTimer >= shootCooldown)
            {
                Shoot();
                shootTimer = 0f;
            }
        }
        else
        {
            Debug.Log("Turret Disabled - no shooting");
        }
    }

    void Shoot()
    {
        GameObject newProjectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);

        // newProjectile.GetComponent<Rigidbody>().linearVelocity = projectileSpawnPoint.forward * projectileSpeed;
        Rigidbody rb = newProjectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = projectileSpawnPoint.forward * projectileSpeed;
        }

        Destroy(newProjectile, projectileLifetime);
    }

    void DrawArc()
    {
        Vector3 startPos = projectileSpawnPoint.position;
        Vector3 startVel = projectileSpawnPoint.forward * projectileSpeed;

        int pointCount = Mathf.CeilToInt(projectileLifetime / arcTimeStep);
        lineRenderer.positionCount = pointCount;

        for (int i = 0; i < pointCount; i++)
        {
            float t = i * arcTimeStep;
            Vector3 pos = startPos + startVel * t + 0.5f * gravity * t * t;
            lineRenderer.SetPosition(i, pos);
        }
    }

    public void ToggleTurret()
    {
        turretEnabled = !turretEnabled;
        Debug.Log("Turret is now " + (turretEnabled ? "ON" : "OFF"));
    }
}
