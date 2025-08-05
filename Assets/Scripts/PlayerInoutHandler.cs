using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
public class PlayerInputHandler : MonoBehaviour
{
    public Creature playerCreature;

    public List<Creature> creatures;

    public Transform cameraTransform;

    public Dictionary<string, KeyCode> controls = new Dictionary<string, KeyCode>();
    public Button[] buttons;

    [HideInInspector]
    public bool listening = false;
    [HideInInspector]
    public string control_name = "";

    private void Awake()
    {
        controls.Add("foward", KeyCode.W);
        controls.Add("backward", KeyCode.S);
        controls.Add("left", KeyCode.A);
        controls.Add("right", KeyCode.D);
        controls.Add("jump", KeyCode.Space);
        controls.Add("crouch", KeyCode.LeftControl);
        controls.Add("shoot", KeyCode.Mouse0);
        controls.Add("aim", KeyCode.Mouse1);
        controls.Add("reload", KeyCode.R);

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        // Initialize button texts with current keybinds
        UpdateAllButtonTexts();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = Vector3.zero;

        // if (Input.GetKey(KeyCode.A)) movement += Vector3.left;
        // if (Input.GetKey(KeyCode.D)) movement += Vector3.right;
        // if (Input.GetKey(KeyCode.W)) movement += Vector3.forward;
        // if (Input.GetKey(KeyCode.S)) movement += Vector3.back;
        if (Input.GetKey(controls["left"])) movement += Vector3.left;
        if (Input.GetKey(controls["right"])) movement += Vector3.right;
        if (Input.GetKey(controls["foward"])) movement += Vector3.forward;
        if (Input.GetKey(controls["backward"])) movement += Vector3.back;

        Vector3 cameraMoveForward = cameraTransform.forward * movement.z;
        Vector3 cameraMoveRight = cameraTransform.right * movement.x;
        Vector3 cameraAdjustedMovement = cameraMoveForward + cameraMoveRight;

        cameraAdjustedMovement.y = 0;

        // ✅ Call jump input BEFORE movement
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     Debug.Log("Jump!");
        //     playerCreature.Jump();
        // }
        if (Input.GetKeyDown(controls["jump"]))
        {
            Debug.Log("Jump!");
            playerCreature.Jump();
        }

        // Hold to crouch
        // playerCreature.SetCrouch(Input.GetKey(KeyCode.LeftControl));
        playerCreature.SetCrouch(Input.GetKey(controls["crouch"]));

        //toggle crouch
        // if (Input.GetKeyDown(KeyCode.LeftControl))
        // {
        //     playerCreature.SetCrouch(!playerCreature.IsCrouching());
        // }

        playerCreature.Move(cameraAdjustedMovement);

        // if (Input.GetKeyDown(KeyCode.Q))
        // {
        //     playerCreature.SpawnObject();
        // }

        // for (int i = 0; i < creatures.Count; i++)
        // {
        //     creatures[i].Move(movement);
        // }
        foreach (var creature in creatures)
        {
            creature.Move(movement);
        }

        //--------------- Keybinds listening
        // if (listening)
        // {
        //     ChangeControls(control_name);
        // }
        //------------
        //listen for any key when waiting for input
        if (listening)
        {
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    controls[control_name] = key;
                    listening = false;

                    // Update only the button corresponding to the changed control
                    foreach (Button btn in buttons)
                    {
                        if (btn.name == control_name + "Button")
                        {
                            var tmpText = btn.GetComponentInChildren<TextMeshProUGUI>();
                            if (tmpText != null)
                                tmpText.text = key.ToString();
                            else
                                Debug.LogWarning("TextMeshProUGUI component not found on button " + btn.name);
                        }
                    }

                    Debug.Log($"Bound '{control_name}' to {key}");
                    break; // Stop checking keys after one is found
                }
            }
        }
        //---------------
    }

    // public void ChangeControls(string control)
    // {
    //     listening = true;
    //     control_name = control;
    //     foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
    //     {
    //         if (Input.GetKeyDown(key))
    //         {
    //             controls[control] = key;
    //             listening = false;

    //             // Updated for TextMeshPro
    //             foreach (Button btn in buttons)
    //             {
    //                 if (btn.name == control + "Button")
    //                 {
    //                     var tmpText = btn.GetComponentInChildren<TextMeshProUGUI>();
    //                     if (tmpText != null)
    //                     {
    //                         tmpText.text = key.ToString();
    //                     }
    //                 }
    //             }
    //             Debug.Log($"Rebound '{control}' to {key}");
    //             break; // This stays to stop after assigning the key    
    //         }
    //     }
    // }

    public void StartListeningForKey(string control)
    {
        // listening = true;
        // control_name = control;
        //------------
        if (controls.ContainsKey(control))
        {
            listening = true;
            control_name = control;
            Debug.Log($"Listening for new keybind for '{control_name}'");
        }
        else
        {
            Debug.LogWarning($"Control '{control}' does not exist in controls dictionary.");
        }
    }

    // Helper to initialize all button labels on Start()
    private void UpdateAllButtonTexts()
    {
        foreach (Button btn in buttons)
        {
            string controlKey = btn.name.Replace("Button", "");
            if (controls.TryGetValue(controlKey, out KeyCode key))
            {
                var tmpText = btn.GetComponentInChildren<TextMeshProUGUI>();
                if (tmpText != null)
                    tmpText.text = key.ToString();
            }
        }
    }

}

