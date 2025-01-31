using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DriftCarController : MonoBehaviour
{
    public float motorForce = 1500f;          // Power applied to the wheels
    public float maxSteerAngle = 30f;         // Max steering angle
    public float driftFactor = 0.95f;         // Reduced sideways friction for drifting
    public float normalFactor = 1f;           // Default sideways friction
    public float driftTorque = 300f;          // Extra torque for drifting
    public float driftThresholdSpeed = 20f;   // Minimum speed required to start drifting
    public float driftSteerThreshold = 15f;   // Steering angle required to trigger drift

    public WheelCollider frontLeftWheel, frontRightWheel, rearLeftWheel, rearRightWheel;

    private Rigidbody rb;
    private bool isDrifting = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0); // Lower center of mass for stability
    }

    void Update()
    {
        HandleMotor();
        HandleSteering();
        ApplyDriftIfNeeded();
    }

    private void HandleMotor()
    {
        float motorInput = Input.GetAxis("Vertical") * motorForce;
        rearLeftWheel.motorTorque = motorInput;
        rearRightWheel.motorTorque = motorInput;
    }

    private void HandleSteering()
    {
        float steerInput = Input.GetAxis("Horizontal") * maxSteerAngle;
        frontLeftWheel.steerAngle = steerInput;
        frontRightWheel.steerAngle = steerInput;
    }

    private void ApplyDriftIfNeeded()
    {
        // Check if car is moving fast enough and steering at a high angle
        float steerInput = Mathf.Abs(Input.GetAxis("Horizontal"));

        if (rb.linearVelocity.magnitude > driftThresholdSpeed && steerInput > driftSteerThreshold / maxSteerAngle)
        {
            if (!isDrifting)
            {
                StartDrift();
            }
        }
        else if (isDrifting)
        {
            StopDrift();
        }
    }

    private void StartDrift()
    {
        isDrifting = true;
        AdjustWheelFriction(rearLeftWheel, driftFactor);
        AdjustWheelFriction(rearRightWheel, driftFactor);

        // Add torque for smoother drifting
        float driftDirection = Input.GetAxis("Horizontal") * driftTorque;
        rb.AddTorque(transform.up * driftDirection, ForceMode.Acceleration);
    }

    private void StopDrift()
    {
        isDrifting = false;
        AdjustWheelFriction(rearLeftWheel, normalFactor);
        AdjustWheelFriction(rearRightWheel, normalFactor);
    }

    private void AdjustWheelFriction(WheelCollider wheel, float sidewaysFriction)
    {
        WheelFrictionCurve frictionCurve = wheel.sidewaysFriction;
        frictionCurve.stiffness = sidewaysFriction;
        wheel.sidewaysFriction = frictionCurve;
    }
}

