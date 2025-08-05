using UnityEngine;

public class ADS : MonoBehaviour
{
    [Header("References")]
    public Transform defaultPosition;         // Hip-fire position (local)
    public Transform aimDownSightsPosition;   // ADS position (local)
    public GameObject crosshair;              // UI crosshair
    private PlayerInputHandler inputHandler;


    [Header("Settings")]
    public float aimSpeed = 10f;

    private bool isAiming = false;

    private void Awake()
    {
        inputHandler = FindObjectOfType<PlayerInputHandler>();

    }

    void Update()
    {
        if (inputHandler != null && inputHandler.controls.ContainsKey("aim"))
        {
            isAiming = Input.GetKey(inputHandler.controls["aim"]);
        }


        if (isAiming)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, aimDownSightsPosition.localPosition, Time.deltaTime * aimSpeed);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, aimDownSightsPosition.localRotation, Time.deltaTime * aimSpeed);
            if (crosshair != null) crosshair.SetActive(false);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, defaultPosition.localPosition, Time.deltaTime * aimSpeed);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, defaultPosition.localRotation, Time.deltaTime * aimSpeed);
            if (crosshair != null) crosshair.SetActive(true);
        }
    }
}
