using UnityEngine;
using UnityEngine.UI;

public class ControlSwitcher : MonoBehaviour
{
    public GameObject steeringControlCanvas;
    public GameObject buttonControlCanvas;
    public GameObject gyroControlCanvas;

    public GyroSteeringControl gyroSteeringControl; // Reference to the GyroSteeringControl component

    // UI Buttons
    public Button steeringButton;
    public Button buttonButton;
    public Button gyroButton;

    private GameObject playerCar; // Cached reference to the player car

    void Start()
    {
        // Cache the player car reference
        playerCar = GameObject.FindGameObjectWithTag("Player");
        if (playerCar == null)
        {
            Debug.LogError("No car with the 'Player' tag found in the scene.");
        }

        // Set up button listeners to switch between control schemes
        steeringButton.onClick.AddListener(ActivateSteeringControl);
        buttonButton.onClick.AddListener(ActivateButtonControl);
        gyroButton.onClick.AddListener(ActivateGyroControl);

        // Load the previously saved control scheme and apply it
        LoadControlScheme();
    }

    // Activate Steering Control
    public void ActivateSteeringControl()
    {
        Debug.Log("Activating Steering Control...");
        SetActiveCanvas(steeringControlCanvas);

        // Save the selected control scheme
        SaveControlScheme("Steering");

        // Disable Gyro control
        gyroSteeringControl.DisableGyroControl();
    }

    // Activate Button Control
    public void ActivateButtonControl()
    {
        Debug.Log("Activating Button Control...");
        SetActiveCanvas(buttonControlCanvas);

        // Save the selected control scheme
        SaveControlScheme("Button");

        // Disable Gyro control
        gyroSteeringControl.DisableGyroControl();
    }

    // Activate Gyro Control
    public void ActivateGyroControl()
    {
        Debug.Log("Activating Gyro Control...");
        SetActiveCanvas(gyroControlCanvas);

        // Save the selected control scheme
        SaveControlScheme("Gyro");

        if (playerCar != null)
        {
            Rigidbody playerCarRigidbody = playerCar.GetComponent<Rigidbody>();
            if (playerCarRigidbody != null)
            {
                // Set the active car's Rigidbody in the GyroSteeringControl
                gyroSteeringControl.SetActiveCar(playerCarRigidbody);
                gyroSteeringControl.EnableGyroControl(); // Enable gyro control
            }
            else
            {
                Debug.LogError("Player car does not have a Rigidbody component.");
            }
        }
        else
        {
            Debug.LogError("Player car is missing. Cannot enable Gyro Control.");
        }
    }

    // Set active canvas and deactivate others
    private void SetActiveCanvas(GameObject activeCanvas)
    {
        steeringControlCanvas.SetActive(activeCanvas == steeringControlCanvas);
        buttonControlCanvas.SetActive(activeCanvas == buttonControlCanvas);
        gyroControlCanvas.SetActive(activeCanvas == gyroControlCanvas);
    }

    // Save the control scheme to PlayerPrefs
    private void SaveControlScheme(string controlScheme)
    {
        PlayerPrefs.SetString("ControlScheme", controlScheme);
        PlayerPrefs.Save();
        Debug.Log($"Control scheme '{controlScheme}' saved.");
    }

    // Load the saved control scheme from PlayerPrefs
    private void LoadControlScheme()
    {
        if (PlayerPrefs.HasKey("ControlScheme"))
        {
            string controlScheme = PlayerPrefs.GetString("ControlScheme");
            Debug.Log($"Loaded control scheme: {controlScheme}");

            // Apply the saved control scheme
            switch (controlScheme)
            {
                case "Steering":
                    ActivateSteeringControl();
                    break;
                case "Button":
                    ActivateButtonControl();
                    break;
                case "Gyro":
                    ActivateGyroControl();
                    break;
                default:
                    Debug.LogWarning("Unknown control scheme. Defaulting to Steering.");
                    ActivateSteeringControl();
                    break;
            }
        }
        else
        {
            Debug.Log("No saved control scheme found. Defaulting to Steering.");
            ActivateSteeringControl();
        }
    }
}





