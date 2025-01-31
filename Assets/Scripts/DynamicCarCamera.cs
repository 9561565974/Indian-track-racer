using UnityEngine;

public class DynamicCarCamera: MonoBehaviour
{
    public Transform car; // The car's transform to follow  
    public Vector3 offset; // Offset from the car  
    public float smoothSpeed = 0.125f; // Smoothing speed  

    void LateUpdate()
    {
        if (car != null)
        {
            Vector3 desiredPosition = car.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            transform.LookAt(car.position); // Optional: make the camera look at the car  
        }
    }
}