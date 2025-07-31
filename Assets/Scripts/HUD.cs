using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    public Creature player; // Reference to your Creature script
    public TextMeshProUGUI healthText;

    void Update()
    {
        healthHUD();
    }

    void healthHUD()
    {
        if (player != null && healthText != null)
        {
            healthText.text = "Health: " + player.GetHealth().ToString();
        }
        if (player.GetHealth() <= 30)
            healthText.color = Color.red;
        else
            healthText.color = Color.white;
    }
}
