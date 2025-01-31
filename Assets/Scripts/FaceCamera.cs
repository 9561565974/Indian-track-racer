using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    void Update()
    {
        // Make the text face the main camera
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0); // Rotate if necessary for correct orientation
    }
}

