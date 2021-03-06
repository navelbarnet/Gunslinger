﻿using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnect : NetworkBehaviour {

    [SerializeField] private Behaviour[] componentsToDisable;   // Assigned components will be disabled
    [SerializeField] private GameObject[] gameObjectsToDisable; // Assigned GameObjects will be disabled

    private GameObject sceneCamera;                             // Defines the scene camera

    // Use this for initialization
    void Start () {
        // If this player is not the local player
		if(!hasAuthority)
        {
            // Disable all assigned components
            for(int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }

            // Disable all assigned GameObjects
            for (int i = 0; i < gameObjectsToDisable.Length; i++)
            {
                gameObjectsToDisable[i].SetActive(false);
            }
        }
        // Else - If this player is the local player
        else
        {
            // Get the scene camera
            sceneCamera = Camera.main.gameObject;
            // If the scene camera exists, disable it
            if(sceneCamera != null)
            {
                sceneCamera.SetActive(false);
            }
        }
	}

    public void DisableComponents() // Used for disabling the winning players components
    {
        // Disable all assigned components
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
        if(sceneCamera)
            sceneCamera.SetActive(true);
    }

    // OnDisable is run when this object is disabled
    void OnDisable()
    {
        // If the scene camera exists, enable it
        if (sceneCamera != null)
        {
            sceneCamera.SetActive(true);
        }
    }

    private void OnEnable()
    {
        // If the scene camera exists, disable it
        if (sceneCamera != null)
        {
            sceneCamera.SetActive(false);
        }
    }
}
