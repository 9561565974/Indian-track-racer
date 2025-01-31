using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using MertStudio.Car.Sounds;

public class CarController : MonoBehaviour
{
    public WheelCollider frontRightWheelCollider;
    public WheelCollider backRightWheelCollider;
    public WheelCollider frontLeftWheelCollider;
    public WheelCollider backLeftWheelCollider;

    public Transform frontRightWheelTransform;
    public Transform backRightWheelTransform;
    public Transform frontLeftWheelTransform;
    public Transform backLeftWheelTransform;

    public Transform carCentreOfMassTransform;
    public Rigidbody carRigidbody;

    public float motorForce = 1500f;
    public float steeringAngle = 30f;
    public float brakeForce = 5000f;
    public float downforce = 100f;
    public float gripMultiplier = 2.0f;
    public float tractionControlFactor = 0.5f;

    public Button brakeButton;
    private bool isBraking = false;
    private float verticalInput;
    private float horizontalInput;

    // Smoothing for RPM
    private float smoothedRPM = 0f;
    private float smoothingFactor = 0.2f;

    // Reference to the CarAudioController
    public CarAudioController audioController;

    void Start()
    {
        // Set center of mass to improve stability
        carRigidbody.centerOfMass = carCentreOfMassTransform.localPosition;

        brakeButton.onClick.AddListener(OnBrakeButtonPressed);
        brakeButton.onClick.AddListener(OnBrakeButtonReleased);

        // Adjust wheel friction for better grip
        SetWheelFriction(gripMultiplier);
    }

    void FixedUpdate()
    {
        GetInput(); // Get input values each frame

        MotorForce();
        Steering();
        ApplyBrakes();
        ApplyDownforce();
        UpdateWheels();
        UpdateRPM();
    }

    void GetInput()
    {
        verticalInput = SimpleInput.GetAxis("Vertical"); // Replace with your actual input method
        horizontalInput = SimpleInput.GetAxis("Horizontal");
    }

    void MotorForce()
    {
        float motorTorque = motorForce * verticalInput;

        // Apply traction control to reduce torque if slipping
        if (IsCarSlipping())
        {
            motorTorque *= Mathf.Lerp(1f, tractionControlFactor, 0.1f); // Smooth transition
        }

        frontRightWheelCollider.motorTorque = motorTorque;
        frontLeftWheelCollider.motorTorque = motorTorque;
    }

    void ApplyBrakes()
    {
        if (isBraking)
        {
            frontRightWheelCollider.brakeTorque = brakeForce;
            frontLeftWheelCollider.brakeTorque = brakeForce;
            backRightWheelCollider.brakeTorque = brakeForce;
            backLeftWheelCollider.brakeTorque = brakeForce;
        }
        else
        {
            frontRightWheelCollider.brakeTorque = 0f;
            frontLeftWheelCollider.brakeTorque = 0f;
            backRightWheelCollider.brakeTorque = 0f;
            backLeftWheelCollider.brakeTorque = 0f;
        }
    }

    void ApplyDownforce()
    {
        carRigidbody.AddForce(-transform.up * downforce * carRigidbody.linearVelocity.magnitude);
    }

    void Steering()
    {
        frontRightWheelCollider.steerAngle = steeringAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = steeringAngle * horizontalInput;
    }

    void UpdateWheels()
    {
        RotateWheel(frontRightWheelCollider, frontRightWheelTransform);
        RotateWheel(backRightWheelCollider, backRightWheelTransform);
        RotateWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        RotateWheel(backLeftWheelCollider, backLeftWheelTransform);
    }

    void RotateWheel(WheelCollider wheelCollider, Transform transform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        transform.position = pos;
        transform.rotation = rot;
    }

    void UpdateRPM()
    {
        float speed = carRigidbody.linearVelocity.magnitude;
        float wheelRadius = frontLeftWheelCollider.radius;
        float wheelCircumference = 2 * Mathf.PI * wheelRadius;
        float rotationsPerSecond = speed / wheelCircumference;
        float rpm = rotationsPerSecond * 60;

        // Smooth the RPM to reduce abrupt changes
        smoothedRPM = Mathf.Lerp(smoothedRPM, rpm, smoothingFactor);
        audioController.rpm = Mathf.Clamp01(smoothedRPM / 1000f);

        Debug.Log($"Speed: {speed}, RPM: {rpm}, Smoothed RPM: {smoothedRPM}");
    }

    void SetWheelFriction(float grip)
    {
        WheelCollider[] wheels = { frontRightWheelCollider, backRightWheelCollider, frontLeftWheelCollider, backLeftWheelCollider };
        foreach (WheelCollider wheel in wheels)
        {
            WheelFrictionCurve forwardFriction = wheel.forwardFriction;
            forwardFriction.stiffness = grip; // Forward grip for acceleration
            wheel.forwardFriction = forwardFriction;

            WheelFrictionCurve sidewaysFriction = wheel.sidewaysFriction;
            sidewaysFriction.stiffness = grip * 0.8f; // Slightly lower for controlled drift
            wheel.sidewaysFriction = sidewaysFriction;
        }
    }

    bool IsCarSlipping()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(carRigidbody.linearVelocity);
        return Mathf.Abs(localVelocity.x) > 0.5f;
    }

    public void OnBrakeButtonPressed()
    {
        isBraking = true;
    }

    public void OnBrakeButtonReleased()
    {
        isBraking = false;
    }
}

















