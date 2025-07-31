using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class PlayerInputHandler : MonoBehaviour
{
    public Creature playerCreature;

    public List<Creature> creatures;

    public Transform cameraTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 movement = new Vector3(0, 0, 0);
        // if (Input.GetKey(KeyCode.A))
        // {
        //     Debug.Log("I'm holding A!");
        //     movement += new Vector3(-1, 0, 0);
        // }

        // if (Input.GetKey(KeyCode.D))
        // {
        //     Debug.Log("I'm holding D!");
        //     movement += new Vector3(1, 0, 0);
        // }
        // if (Input.GetKey(KeyCode.W))
        // {
        //     Debug.Log("I'm holding W!");
        //     movement += new Vector3(0, 0, 1);
        // }
        // if (Input.GetKey(KeyCode.S))
        // {
        //     Debug.Log("I'm holding S!");
        //     movement += new Vector3(0, 0, -1);
        // }
        Vector3 movement = Vector3.zero;

        if (Input.GetKey(KeyCode.A)) movement += Vector3.left;
        if (Input.GetKey(KeyCode.D)) movement += Vector3.right;
        if (Input.GetKey(KeyCode.W)) movement += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) movement += Vector3.back;

        Vector3 cameraMoveForward = cameraTransform.forward * movement.z;
        Vector3 cameraMoveRight = cameraTransform.right * movement.x;
        Vector3 cameraAdjustedMovement = cameraMoveForward + cameraMoveRight;

        cameraAdjustedMovement.y = 0;

        // ✅ Call jump input BEFORE movement
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Jump!");
            playerCreature.Jump();
        }

        // Hold to crouch
        playerCreature.SetCrouch(Input.GetKey(KeyCode.LeftControl));

        //toggle crouch
        // if (Input.GetKeyDown(KeyCode.LeftControl))
        // {
        //     playerCreature.SetCrouch(!playerCreature.IsCrouching());
        // }

        playerCreature.Move(cameraAdjustedMovement);

        


        if (Input.GetKeyDown(KeyCode.Q))
        {
            playerCreature.SpawnObject();
        }

        // for (int i = 0; i < creatures.Count; i++)
        // {
        //     creatures[i].Move(movement);
        // }
        foreach (var creature in creatures)
        {
            creature.Move(movement);
        }

        
        

    }
}

