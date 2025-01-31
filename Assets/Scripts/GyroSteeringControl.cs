using UnityEngine;

public class GyroSteeringControl : MonoBehaviour
{
    public float steeringSensitivity = 3.0f; // Adjust sensitivity for steering
    private Rigidbody activeCarRigidbody;

    void Start()
    {
        // Enable the gyroscope
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
        }
        else
        {
            Debug.LogWarning("Gyroscope not supported on this device.");
        }
    }

    void Update()
    {
        // Check if gyroscope is enabled and an active car is set
        if (Input.gyro.enabled && activeCarRigidbody != null)
        {
            // Get the device tilt (rotation rate around Y-axis)
            float tilt = Input.gyro.rotationRateUnbiased.y;

            // Apply tilt to the car's rotation for steering
            float steeringAngle = tilt * steeringSensitivity;

            // Rotate the car to simulate steering
            activeCarRigidbody.AddRelativeTorque(Vector3.up * steeringAngle, ForceMode.Acceleration);
        }
    }

    // Set the active car to control
    public void SetActiveCar(Rigidbody carRigidbody)
    {
        activeCarRigidbody = carRigidbody;
    }

    // Enable gyro control
    public void EnableGyroControl()
    {
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
        }
        else
        {
            Debug.LogWarning("Gyroscope not supported on this device.");
        }
    }

    // Disable gyro control
    public void DisableGyroControl()
    {
        Input.gyro.enabled = false;
    }
}





