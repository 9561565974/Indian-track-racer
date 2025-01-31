using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WaypointManager : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();

    void Awake()
    {
        // Automatically populate the waypoints list with child transforms
        foreach (Transform waypoint in transform)
        {
            waypoints.Add(waypoint);
        }
    }

    // Optional: Draw Gizmos in the editor to visualize the waypoints
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform waypoint = transform.GetChild(i);

            // Draw a sphere at each waypoint position
            Gizmos.DrawSphere(waypoint.position, 1f);

            // Draw lines between waypoints
            if (i > 0)
            {
                Gizmos.DrawLine(transform.GetChild(i - 1).position, waypoint.position);
            }
        }
    }
}
