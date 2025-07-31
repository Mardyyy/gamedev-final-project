using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float projectileLifetime = 20f;
    public float projectileSpeed = 10f;
    public float shootCooldown = 1f;
    float shootTimer = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        //spawns at random positions within a range
        //transform.position = transform.position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootCooldown)
        {
            Shoot();
            shootTimer = 0f;
        }
    }

    void Shoot()
    {
        GameObject newProjectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);

        newProjectile.GetComponent<Rigidbody>().linearVelocity = projectileSpawnPoint.forward * projectileSpeed;

        Destroy(newProjectile, projectileLifetime);
    }
}
