using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcherOnDisable : MonoBehaviour
{
    public Camera[] cameras; // Array to store all cameras in the scene
    private int currentCameraIndex = 0; // Index to track the current active camera

    private void Start()
    {
        // Ensure only the first camera is active at the start
        ActivateCamera(currentCameraIndex);
    }

    private void Update()
    {
        // Check if the current camera has been disabled
        if (!cameras[currentCameraIndex].enabled)
        {
            SwitchToNextCamera();
        }
    }

    private void SwitchToNextCamera()
    {
        // Cycle to the next camera in the array
        currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;

        // Activate the new camera
        ActivateCamera(currentCameraIndex);
    }

    private void ActivateCamera(int index)
    {
        // Disable all cameras first
        foreach (Camera cam in cameras)
        {
            cam.enabled = false;
        }

        // Enable the selected camera
        cameras[index].enabled = true;
    }
}

