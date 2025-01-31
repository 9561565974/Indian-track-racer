using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowCar : MonoBehaviour
{
    public Transform car;
    public float followDistance = 6f;
    public float followHeight = 2f;
    public float smoothSpeed = 0.125f;
    public float rotationSpeed = 70f; // Speed for manual rotation
    public Vector2 rotationLimits = new Vector2(-20, 45); // Vertical rotation limits

    private Vector3 defaultOffset;
    private float rotationY = 0f;

    private InputAction lookAction; // For handling look input
    private Vector2 lookInput;      // Stores the input values for looking around

    void Awake()
    {
        // Create a new input action for looking around
        lookAction = new InputAction("Look", binding: "<Mouse>/delta");
        lookAction.AddBinding("<Gamepad>/rightStick");
        lookAction.Enable();
    }

    void Start()
    {
        // Set initial offset based on the starting position
        defaultOffset = new Vector3(0, followHeight, -followDistance);
    }

    void LateUpdate()
    {
        // Get look input from the InputAction
        lookInput = lookAction.ReadValue<Vector2>();

        // Apply rotation based on input
        float horizontalInput = lookInput.x * rotationSpeed * Time.deltaTime;
        float verticalInput = -lookInput.y * rotationSpeed * Time.deltaTime;

        // Update rotationY with limits for vertical input
        rotationY = Mathf.Clamp(rotationY + verticalInput, rotationLimits.x, rotationLimits.y);
        Quaternion rotation = Quaternion.Euler(rotationY, car.eulerAngles.y + horizontalInput, 0);

        // Calculate target position with rotation applied to the offset
        Vector3 targetPosition = car.position + rotation * defaultOffset;

        // Smoothly move camera to target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        transform.LookAt(car.position + Vector3.up * followHeight * 0.5f); // Look slightly above the car center
    }

    void OnDestroy()
    {
        // Disable the input action when the script is destroyed
        lookAction.Disable();
    }
}


