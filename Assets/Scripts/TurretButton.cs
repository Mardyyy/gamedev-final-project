using UnityEngine;

public class TurretButton : MonoBehaviour
{
    public Turret turret;
    private AudioSource audioSource;
    private Renderer buttonRenderer;

    // Colors for ON and OFF states
    public Color colorOn = Color.green;
    public Color colorOff = Color.red;



    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        buttonRenderer = GetComponent<Renderer>();

        // Set initial color based on turret state
        UpdateButtonColor();
    }

    public void PressButton()
    {
        if (turret != null)
        {
            turret.ToggleTurret();
            Debug.Log("Button pressed, toggling turret");

            if (audioSource != null)
                audioSource.Play();

            UpdateButtonColor();
        }
        else
        {
            Debug.LogWarning("Turret not assigned in TurretButton!");
        }
    }

    void UpdateButtonColor()
    {
        if (buttonRenderer != null && turret != null)
        {
            buttonRenderer.material.color = turret.turretEnabled ? colorOn : colorOff;
        }
    }

}
