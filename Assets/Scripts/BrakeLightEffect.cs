using UnityEngine;

public class BrakeLightEffect : MonoBehaviour
{
    public Light leftBrakeLight; // Assign the left brake light component here
    public Light rightBrakeLight; // Assign the right brake light component here
    private CarController carController; // Reference to the CarController script

    private void Start()
    {
        // Get the CarController component from the parent GameObject (car)
        carController = GetComponent<CarController>();

        leftBrakeLight.enabled = false; // Start with the left light turned off
        rightBrakeLight.enabled = false; // Start with the right light turned off
    }

    private void Update()
    {
        // Check if the car is braking using reflection
        bool isBraking = (bool)typeof(CarController).GetField("isBraking",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(carController);

        if (isBraking)
        {
            EnableBrakeLights();
        }
        else
        {
            DisableBrakeLights();
        }
    }

    private void EnableBrakeLights()
    {
        leftBrakeLight.enabled = true;  // Enable left brake light
        rightBrakeLight.enabled = true; // Enable right brake light

        // Log to console for debugging
        Debug.Log("Brake lights enabled.");
    }

    private void DisableBrakeLights()
    {
        leftBrakeLight.enabled = false;  // Disable left brake light
        rightBrakeLight.enabled = false; // Disable right brake light

        // Log to console for debugging
        Debug.Log("Brake lights disabled.");
    }
}







