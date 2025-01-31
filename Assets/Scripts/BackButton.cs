using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    // Function to load a specific track scene
    public void OnTrackImageClick()
    {
        // Load the scene named "Garage"
        SceneManager.LoadScene("Garage");
    }
}

