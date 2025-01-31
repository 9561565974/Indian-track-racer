using UnityEngine;
using TMPro;

public class DriverNameDisplay : MonoBehaviour
{
    public TextMeshPro driverNameText; // Drag the 3D TextMeshPro component here

    void Start()
    {
        // Retrieve the saved name from PlayerPrefs and display it
        string driverName = PlayerPrefs.GetString("DriverName", "Driver");
        driverNameText.text = driverName;
    }
}

