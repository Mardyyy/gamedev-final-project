using UnityEngine;
using TMPro;


public class GunSystem : MonoBehaviour
{
    //Gun stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;


    //bools 
    bool shooting, readyToShoot, reloading;


    //Reference
    public Transform gunPivot;       // The part of the gun that rotates
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;
    private PlayerInputHandler inputHandler;


    // Sound
    public AudioSource audioSource;
    public AudioClip shootingSound;
    public AudioClip reloadSound;




    //Graphics
    public GameObject muzzleFlash, bulletHoleGraphic;
    //public CamShake camShake;
    public float camShakeMagnitude, camShakeDuration;
    public TextMeshProUGUI text;


    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
        inputHandler = FindObjectOfType<PlayerInputHandler>();

    }
    private void Update()
    {
        MyInput();


        //SetText
        text.SetText(bulletsLeft + " / " + magazineSize);

        // Rotate the gun to match camera pitch + yaw
        gunPivot.rotation = Quaternion.Lerp(
            gunPivot.rotation,
            fpsCam.transform.rotation,
            Time.deltaTime * 10f
        );

        // if (inputHandler != null && Input.GetKey(inputHandler.controls["aim"]))
        // {
        //     // Optional: set aiming animation / zoom / crosshair
        // }
    }
    private void MyInput()
    {
        if (inputHandler == null) return;

        KeyCode shootKey = inputHandler.controls["shoot"];
        KeyCode reloadKey = inputHandler.controls["reload"];

        if (allowButtonHold) shooting = Input.GetKey(shootKey);
        else shooting = Input.GetKeyDown(shootKey);

        if (Input.GetKeyDown(reloadKey) && bulletsLeft < magazineSize && !reloading)
            Reload();

        // Shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }
    }
    private void Shoot()
    {
        Debug.Log("Shooting");
        readyToShoot = false;

        // 🔊 Play shooting sound
        if (shootingSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootingSound);
        }

        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);


        //Calculate Direction with Spread
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);


        //RayCast
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            Debug.Log(rayHit.collider.name);


            if (rayHit.collider.CompareTag("Enemy"))
                rayHit.collider.GetComponent<Seeker>().TakeDamage(damage);
        }


        //ShakeCamera
        //camShake.Shake(camShakeDuration, camShakeMagnitude);


        //Graphics
        Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));
        Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);


        bulletsLeft--;
        bulletsShot--;


        Invoke("ResetShot", timeBetweenShooting);


        if(bulletsShot > 0 && bulletsLeft > 0)
        Invoke("Shoot", timeBetweenShots);
    }
    private void ResetShot()
    {
        readyToShoot = true;
    }
    private void Reload()
    {
        reloading = true;

        // Play reload sound
        if (reloadSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(reloadSound);
        }

        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}

