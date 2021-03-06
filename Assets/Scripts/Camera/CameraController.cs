﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script that handles most of the player camera behaviour.

public class CameraController : MonoBehaviour
{
    public Vector3 AimingPosition { get { return aimingPosition; } }


    [SerializeField] private float sensitivity;                     // Defines the amount of camera sensitivity
    [SerializeField] private float damping;                         // Defines the amount of camera damping
    [SerializeField] private float minAngle;                        // Defines the camera's lowest angle
    [SerializeField] private float maxAngle;                        // Defines the camera's highest angle
    
    private Vector2 mouseInput;                                     // Contains the raw mouse input
    private Vector2 smoothV;                                        // 
    private Vector2 mouseLook;                                      // 

    [SerializeField] private float raycastDistance = 100f;          // Defines the longest raycast distance

    private Vector3 aimingPosition = Vector3.zero;                  // Defines the current aiming position
    private Transform cameraChild;                                  // Defines the child that contains the camera component


    void Start ()
    {
        // Get the child that contains the camera component
        cameraChild = transform.GetChild(0);
    }

    void LateUpdate ()
    {
        mouseInput = new Vector2 (Input.GetAxisRaw ("Mouse X"), Input.GetAxisRaw ("Mouse Y"));
        mouseInput = Vector2.Scale (mouseInput, new Vector2 (sensitivity * damping, sensitivity * damping));
        
        smoothV.x = Mathf.Lerp (smoothV.x, mouseInput.x, 1.0f / damping);
        smoothV.y = Mathf.Lerp (smoothV.y, mouseInput.y, 1.0f / damping);

        mouseLook += smoothV;
        mouseLook.y = Mathf.Clamp (mouseLook.y, minAngle, maxAngle);

        transform.localRotation = Quaternion.AngleAxis (-mouseLook.y, Vector3.right);
        transform.parent.localRotation = Quaternion.AngleAxis (mouseLook.x, transform.parent.up);

        // Update the current aiming position
        UpdateAimingPosition();
    }

    // Updates the current aiming position
    void UpdateAimingPosition ()
    {
        // Defines the value returned by the following raycast
        RaycastHit hit;

        // If the raycast intersects with a collider in front of the camera
        if (Physics.Raycast(cameraChild.position, cameraChild.forward, out hit, raycastDistance) && hit.collider.tag != "Player")
        {
            // Draw a debug ray indicating a successful raycast
            Debug.DrawRay(cameraChild.position, cameraChild.forward * hit.distance, Color.green);

            // Set the aiming coordinate to the point of the intersection
            aimingPosition = hit.point;
        }
        // Else - If the raycast did not intersect with a collider in front of the camera
        else
        {
            // Draw a debug ray indicating an unsuccessful raycast
            Debug.DrawRay(cameraChild.position, cameraChild.forward * raycastDistance, Color.red);

            // Set the aiming coordinate to the point of the longest raycast
            aimingPosition = cameraChild.position + cameraChild.forward * raycastDistance;
        }
    }
}
