using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    public Rigidbody playerCar;        // Assign the car's Rigidbody in the Inspector
    public TextMeshProUGUI speedText;  // Assign the TextMeshProUGUI for the speed display
    public GameObject speedMeter;      // Assign the SpeedMeter canvas here
    public RectTransform arrow;        // Assign the arrow's RectTransform (the needle)
    public float maxSpeed = 240f;      // Max speed for the speedometer
    public float maxArrowRotation = -90f;  // Max rotation angle for the arrow (to the left)
    public Button pauseButton;         // Assign the Pause button in the Inspector
    public Button resumeButton;        // Assign the Resume button in the Inspector

    private bool isPaused = false;     // To track pause state

    void Start()
    {
        // Attach the Pause and Resume functions to the buttons
        pauseButton.onClick.AddListener(PauseGame);
        resumeButton.onClick.AddListener(ResumeGame);
    }

    void Update()
    {
        if (!isPaused)
        {
            // Calculate the car's speed in km/h
            float speed = playerCar.linearVelocity.magnitude * 3.6f;  // Convert to km/h

            // Update the speed text (TextMeshPro version)
            speedText.text = speed.ToString("F0") + " km/h";

            // Rotate the speedometer arrow based on the speed
            float arrowRotation = Mathf.Lerp(0f, maxArrowRotation, speed / maxSpeed);
            arrow.localRotation = Quaternion.Euler(0, 0, arrowRotation);
        }
    }

    // Method to pause the game and hide the speed meter
    public void PauseGame()
    {
        isPaused = true;
        speedMeter.SetActive(false);    // Hide the speed meter UI (including the arrow and text)
        Time.timeScale = 0f;            // Freeze the game
    }

    // Method to resume the game and show the speed meter
    public void ResumeGame()
    {
        isPaused = false;
        speedMeter.SetActive(true);     // Show the speed meter UI again
        Time.timeScale = 1f;            // Resume the game
    }
}




