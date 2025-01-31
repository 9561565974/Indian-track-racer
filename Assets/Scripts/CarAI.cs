using UnityEngine;

public class CarAI : MonoBehaviour
{
    public Transform[] waypoints;
    public float maxSpeed = 20.0f;
    public float slowSpeed = 10.0f;
    public float acceleration = 5.0f;
    public float rotationSpeed = 5.0f;
    public float turnSpeedAdjustment = 3.0f;
    public float thresholdDistance = 3.0f;

    private int currentWaypoint = 0;
    private float currentSpeed = 0.0f;
    public Rigidbody carRigidbody;

    void FixedUpdate()
    {
        // Get the direction towards the next waypoint
        Vector3 direction = (waypoints[currentWaypoint].position - transform.position).normalized;
        Quaternion toRotation = Quaternion.LookRotation(direction);

        // Smoothly rotate the car towards the next waypoint
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime * rotationSpeed);

        // Calculate angle to determine if we should slow down on a turn
        float angle = Vector3.Angle(transform.forward, waypoints[currentWaypoint].position - transform.position);

        if (angle > 30)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, slowSpeed, Time.deltaTime * turnSpeedAdjustment);
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, Time.deltaTime * acceleration);
        }

        // Move the car forward
        carRigidbody.MovePosition(carRigidbody.position + transform.forward * currentSpeed * Time.deltaTime);

        // Check if the car is close enough to the waypoint
        if (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < thresholdDistance)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;  // Move to the next waypoint
        }
    }

    void OnDrawGizmos()
    {
        // Draw lines to the next waypoint for debugging
        if (waypoints.Length > 0)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, waypoints[currentWaypoint].position);
        }
    }
}

