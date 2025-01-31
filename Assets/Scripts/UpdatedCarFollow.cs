using UnityEngine;

public class UpdatedCarFollow : MonoBehaviour
{
    public Transform car;             // Reference to the car transform
    public float distance = 8.0f;     // Distance behind the car
    public float height = 2.5f;       // Height above the car
    public float rotationDampening = 3.0f; // Smoothness of rotation changes
    public float heightDampening = 2.0f;   // Smoothness of height changes
    public float zoomRatio = 20.0f;   // Ratio for FOV zooming
    public float defaultFOV = 60.0f;  // Default camera field of view

    private Vector3 rotationVector;   // Tracks rotation changes
    private Camera cameraComponent;  // Reference to the camera component

    void Start()
    {
        // Cache the camera component for performance
        cameraComponent = GetComponent<Camera>();
        if (cameraComponent == null)
        {
            Debug.LogError("Camera component not found on this GameObject!");
        }
    }

    void LateUpdate()
    {
        if (car == null) return; // Ensure car reference is assigned

        // Get the desired rotation and height
        float wantedAngle = rotationVector.y;
        float wantedHeight = car.position.y + height;

        // Smoothly interpolate current angle and height
        float currentAngle = Mathf.LerpAngle(transform.eulerAngles.y, wantedAngle, rotationDampening * Time.deltaTime);
        float currentHeight = Mathf.Lerp(transform.position.y, wantedHeight, heightDampening * Time.deltaTime);

        // Convert the angle to a rotation
        Quaternion currentRotation = Quaternion.Euler(0, currentAngle, 0);

        // Set the position of the camera behind the car
        transform.position = car.position - currentRotation * Vector3.forward * distance;
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

        // Always look at the car
        transform.LookAt(car);
    }

    void FixedUpdate()
    {
        if (car == null || car.GetComponent<Rigidbody>() == null) return; // Ensure rigidbody is assigned

        // Get the car's velocity in local space
        Vector3 localVelocity = car.InverseTransformDirection(car.GetComponent<Rigidbody>().linearVelocity);

        // Determine if the car is reversing
        if (localVelocity.z < -0.5f)
        {
            rotationVector.y = car.eulerAngles.y + 180; // Reverse camera direction
        }
        else
        {
            rotationVector.y = car.eulerAngles.y; // Normal forward camera
        }

        // Adjust the field of view based on the car's speed
        float acceleration = car.GetComponent<Rigidbody>().linearVelocity.magnitude;
        if (cameraComponent != null)
        {
            cameraComponent.fieldOfView = Mathf.Lerp(cameraComponent.fieldOfView, defaultFOV + acceleration * zoomRatio, Time.deltaTime);
        }
    }
}
