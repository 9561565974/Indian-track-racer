using UnityEngine;

public class RotatingSlider : MonoBehaviour
{
    public float rotationSpeed = 100f;

    void Update()
    {
        // Rotate the slider continuously
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}

