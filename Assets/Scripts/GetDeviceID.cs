using UnityEngine;

public class GetDeviceID : MonoBehaviour
{
    void Start()
    {
        // Get the unique device identifier
        string deviceId = SystemInfo.deviceUniqueIdentifier;

        // Print the device ID to the console (this will show up in Unity's console window)
        Debug.Log("Device ID: " + deviceId);
    }
}

