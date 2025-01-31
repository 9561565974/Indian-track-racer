using UnityEngine;

public class OpponentCarController : MonoBehaviour
{
    public WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    public WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    public Transform frontLeftTransform, frontRightTransform;
    public Transform rearLeftTransform, rearRightTransform;

    public WaypointManager waypointManager; // Reference to the WaypointManager
    public float maxSpeed = 20f; // Max speed of the car
    public float minSpeed = 15f;  // Min speed of the car, used when approaching a turn
    public float rotationSpeed = 5f;
    public float waypointThreshold = 1f;
    public float steeringAngle = 30f;
    public float accelerationFactor = 1.5f;  // How quickly the car accelerates
    public float soundDistanceThreshold = 30f; // Distance to hear the engine sound

    public AudioClip idleSound;
    public AudioClip lowAccelerationSound;
    public AudioClip mediumAccelerationSound;
    public AudioClip highAccelerationSound;

    private int currentWaypointIndex = 0;
    private Rigidbody rb;
    private float verticalInput;
    private float horizontalInput;

    private AudioSource audioSource;
    private float speed;
    private GameObject playerCar; // Reference to the Player's car

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Ensure the WaypointManager is assigned
        if (waypointManager == null || waypointManager.waypoints.Count == 0)
        {
            Debug.LogError("WaypointManager not set or contains no waypoints.");
            return;
        }

        // Find the player car by its tag
        playerCar = GameObject.FindGameObjectWithTag("Player");

        // Setup AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    void FixedUpdate()
    {
        if (waypointManager.waypoints.Count == 0) return;

        Steering();
        MoveTowardsWaypoint();
        AdjustSpeed();  // Adjust speed based on the player's position
        UpdateWheelPoses();
        UpdateEngineSound();
    }

    void MoveTowardsWaypoint()
    {
        Vector3 direction = (waypointManager.waypoints[currentWaypointIndex].position - transform.position).normalized;
        rb.MovePosition(transform.position + direction * maxSpeed * Time.deltaTime);
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, waypointManager.waypoints[currentWaypointIndex].position) < waypointThreshold)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypointManager.waypoints.Count;
        }
    }

    void AdjustSpeed()
    {
        if (playerCar != null)
        {
            // Calculate the distance from the opponent car to the player car
            float distanceToPlayer = Vector3.Distance(transform.position, playerCar.transform.position);

            // Adjust maxSpeed based on distance to player
            maxSpeed = Mathf.Lerp(minSpeed, 30f, Mathf.Clamp01(distanceToPlayer / 20f)); // Adjust speed based on proximity
        }

        // Update the car's speed using Rigidbody velocity magnitude
        speed = rb.linearVelocity.magnitude;
    }


    void Steering()
    {
        // Get the direction to the current waypoint
        Vector3 targetDirection = (waypointManager.waypoints[currentWaypointIndex].position - transform.position).normalized;

        // Calculate the angle between the car's forward direction and the target direction
        float angle = Vector3.SignedAngle(transform.forward, targetDirection, Vector3.up);

        // Adjust the steering angle based on the angle to the waypoint
        float steerInput = Mathf.Clamp(angle / 45f, -1f, 1f); // Scale angle to [-1, 1]
        float dynamicSteeringAngle = Mathf.Lerp(steeringAngle, steeringAngle / 2, speed / maxSpeed);
        frontLeftWheelCollider.steerAngle = dynamicSteeringAngle * steerInput;
        frontRightWheelCollider.steerAngle = dynamicSteeringAngle * steerInput;

    }


    void UpdateWheelPoses()
    {
        UpdateWheelPose(frontLeftWheelCollider, frontLeftTransform);
        UpdateWheelPose(frontRightWheelCollider, frontRightTransform);
        UpdateWheelPose(rearLeftWheelCollider, rearLeftTransform);
        UpdateWheelPose(rearRightWheelCollider, rearRightTransform);
    }

    void UpdateWheelPose(WheelCollider collider, Transform transform)
    {
        collider.GetWorldPose(out Vector3 pos, out Quaternion quat);
        transform.position = pos;
        transform.rotation = quat;
    }

    void UpdateEngineSound()
    {
        if (playerCar == null || audioSource == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerCar.transform.position);

        // Only play sound if within the distance threshold
        if (distanceToPlayer <= soundDistanceThreshold)
        {
            audioSource.enabled = true;

            // Adjust sound based on speed
            if (speed < 5f)
            {
                audioSource.clip = idleSound;
            }
            else if (speed < 15f)
            {
                audioSource.clip = lowAccelerationSound;
            }
            else if (speed < 25f)
            {
                audioSource.clip = mediumAccelerationSound;
            }
            else
            {
                audioSource.clip = highAccelerationSound;
            }

            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.enabled = false;
        }
    }
}













