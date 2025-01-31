using System.Collections.Generic;
using UnityEngine;

public class MultiCarCameraFollow : MonoBehaviour
{
    public List<Transform> targets; // List to store the player car transforms
    public Vector3 offset; // Offset from the center point of all cars
    public float smoothTime = 0.5f; // Smoothing time for the camera movement
    public float minZoom = 40f; // Minimum field of view (zoom level)
    public float maxZoom = 10f; // Maximum field of view (zoom level)
    public float zoomLimiter = 50f; // Factor to control zoom level based on distance between cars

    private Vector3 velocity; // Velocity for smooth camera movement
    private Camera cam; // Reference to the camera component

    private void Start()
    {
        cam = GetComponent<Camera>(); // Get the camera component attached to this script
    }

    private void LateUpdate()
    {
        if (targets.Count == 0) return; // If there are no targets, do nothing

        Move(); // Move the camera to follow the cars
        Zoom(); // Adjust the zoom level based on the distance between cars
    }

    private void Move()
    {
        // Calculate the center point between all the cars
        Vector3 centerPoint = GetCenterPoint();

        // Calculate the new position for the camera
        Vector3 newPosition = centerPoint + offset;

        // Smoothly move the camera to the new position
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    private void Zoom()
    {
        // Calculate the greatest distance between any two cars
        float greatestDistance = GetGreatestDistance();

        // Calculate the new field of view based on the greatest distance
        float newZoom = Mathf.Lerp(maxZoom, minZoom, greatestDistance / zoomLimiter);

        // Smoothly adjust the camera's field of view
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

    private Vector3 GetCenterPoint()
    {
        // If there's only one car, return its position
        if (targets.Count == 1)
        {
            return targets[0].position;
        }

        // Calculate the bounds of all the cars' positions
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 1; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        // Return the center of the bounds as the center point
        return bounds.center;
    }

    private float GetGreatestDistance()
    {
        // Calculate the bounds of all the cars' positions
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 1; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        // Return the greatest distance across the bounds (size on the longest axis)
        return bounds.size.x;
    }
}

