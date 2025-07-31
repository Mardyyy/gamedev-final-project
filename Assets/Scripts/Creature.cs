using UnityEngine;

public class Creature : MonoBehaviour
{

    public int health = 1;
    public float speed = 10f;
    public float oxygen = 100f;

    public GameObject ObjectToSpawn;

    CharacterController cc;

    public float gravity = -40f;
    Vector3 currentGravity = Vector3.zero;
    public Camera fpsCam;

    public Transform facingTransform;
    public Transform bodyTransform;
    public float rotationSpeed = 10f;
    public float jumpHeight = 5f;
    private bool isJumping = false;

    [Header("Crouch Settings")]
    public float crouchHeight = 1.0f;
    public float standingHeight = 2.0f;
    public float crouchSpeed = 4f;
    private bool isCrouching = false;



    void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SimulateGravity();
        FaceTransform();

        
    }

    void FaceTransform()
    {
        //bodyTransform.rotation = Quaternion.LookRotation(facingTransform.position - transform.position);

        //------------
        // Vector3 targetPos = facingTransform.position;
        // targetPos.y = bodyTransform.position.y; // flatten Y

        // Vector3 direction = targetPos - bodyTransform.position;
        // if (direction.sqrMagnitude > 0.001f) // avoid zero-direction
        // {
        //     Quaternion lookRotation = Quaternion.LookRotation(direction);
        //     bodyTransform.rotation = Quaternion.Lerp(bodyTransform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        // }
        //--------------

        if (facingTransform == null || bodyTransform == null)
            return;

        // Use fpsCam's forward direction flattened to the XZ plane
        Vector3 forward = fpsCam.transform.forward;
        forward.y = 0f;

        if (forward.sqrMagnitude > 0.01f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(forward);
            bodyTransform.rotation = Quaternion.Lerp(bodyTransform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
        //-------
    }


    //Should be called every frame, moves the creature down
    public void SimulateGravity()
    {
        if (cc.isGrounded && currentGravity.y < 0)
        {
            currentGravity.y = -2f; // small downward force to keep grounded
            isJumping = false;
        }
        else
        {
            currentGravity.y += gravity * Time.deltaTime;
        }
    }

    public void SpawnObject()
    {
        Instantiate(ObjectToSpawn, transform.position, Quaternion.identity);
    }

    public void Move(Vector3 direction)
    {
        Vector3 horizontal = Vector3.zero;

        // Only apply horizontal movement if there's input
        if (direction != Vector3.zero)
        {
            horizontal = direction.normalized * speed;

            // (Optional) Rotate body toward movement direction
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            bodyTransform.rotation = Quaternion.Lerp(bodyTransform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        // ✅ Always apply vertical gravity
        Vector3 moveVector = new Vector3(horizontal.x, currentGravity.y, horizontal.z);
        cc.Move(moveVector * Time.deltaTime);
    }
    public void Jump()
    {
        if (cc.isGrounded && !isJumping)
        {
            currentGravity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isJumping = true;
        }
    }
    public void SetCrouch(bool crouch)
    {
        float heightDifference = standingHeight - crouchHeight;

        if (crouch && !isCrouching)
        {
            cc.height = crouchHeight;

            // Move center UP by half the difference to keep feet on ground
            cc.center = new Vector3(0, heightDifference / 2f, 0);

            speed = crouchSpeed;
            isCrouching = true;
        }
        else if (!crouch && isCrouching)
        {
            if (CanStandUp())
            {
                cc.height = standingHeight;

                // Reset center back to zero because pivot is centered
                cc.center = Vector3.zero;

                speed = 10f;
                isCrouching = false;
            }
        }
    }

    //Prevents standing up into low ceilings or obstacles:
    private bool CanStandUp()
    {
        float checkDistance = standingHeight - crouchHeight;
        // Start just above the current height
        Vector3 rayStart = transform.position + Vector3.up * (crouchHeight + 0.05f);

        // Shoot upward to see if there's space for standing
        return !Physics.SphereCast(rayStart, cc.radius - 0.05f, Vector3.up, out _, checkDistance);
    }


    //if using toggle crouch
    public bool IsCrouching()
    {
        return isCrouching;
    }




    public void SetHealth(int newHealth)
    {
        health = newHealth;
    }
    public int GetHealth()
    {
        return health;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
        }
    }
    public bool IsAlive()
    {
        return health > 0;
    }

    
}

