using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit something??");
        if (other.CompareTag("Terrain"))
        {
            // Debug.Log("hit Terrain!");
            // transform.localScale = new Vector3(2, 2, 2);
            // GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            Destroy(gameObject);
        }
        else if (other.GetComponent<Creature>() != null)
        {
            other.GetComponent<Creature>().TakeDamage(1);
        }
        else if (other.GetComponent<Seeker>() != null)
        {
            other.GetComponent<Seeker>().TakeDamage(1);
        }
    }
}
