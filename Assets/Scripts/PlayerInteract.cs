using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float interactDistance = 3f;
    public Camera playerCamera;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left click or use a key here
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
            {
                TurretButton button = hit.collider.GetComponent<TurretButton>();
                if (button != null)
                {
                    button.PressButton();
                }
            }
        }
    }
}
