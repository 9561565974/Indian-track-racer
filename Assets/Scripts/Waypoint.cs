using UnityEngine;

public class Waypoint : MonoBehaviour
{
    private WaypointLapTimer lapTimer;

    void Start()
    {
        // Cache the reference to WaypointLapTimer in Start() for better performance
        lapTimer = Object.FindFirstObjectByType<WaypointLapTimer>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && lapTimer != null)
        {
            // Call CheckWaypoint on the lapTimer with the current waypoint's transform
            lapTimer.CheckWaypoint(transform);
        }
    }
}
