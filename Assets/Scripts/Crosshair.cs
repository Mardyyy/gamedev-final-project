using UnityEngine;

public class Crosshair : MonoBehaviour
{
    void OnGUI()
    {
        float size = 20f;
        float posX = (Screen.width - size) / 2;
        float posY = (Screen.height - size) / 2;

        GUI.Label(new Rect(posX, posY, size, size), "+");
    }
}
