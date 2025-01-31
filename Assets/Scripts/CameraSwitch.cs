using UnityEngine;
using UnityEngine.UI; // Required to use UI elements

public class CameraSwitch : MonoBehaviour
{
    public Camera thirdPersonCamera;  // Reference to the third-person camera
    public Camera dashCamera;         // Reference to the dash camera
    private bool isDashCamActive = false; // Check if dash cam is active

    void Start()
    {
        // Start with the third-person camera active
        thirdPersonCamera.enabled = true;
        dashCamera.enabled = false;
    }

    // This method will be called when the button is pressed
    public void OnCameraSwitchButtonPressed()
    {
        SwitchCamera();
    }

    // Method to toggle between cameras
    void SwitchCamera()
    {
        // Toggle between third-person and dash cam
        isDashCamActive = !isDashCamActive;

        thirdPersonCamera.enabled = !isDashCamActive;
        dashCamera.enabled = isDashCamActive;
    }
}


















