using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class NameInputManager : MonoBehaviour
{
    public TMP_InputField nameInputField; // Reference to your TextMeshPro Input Field

    public void SubmitName()
    {
        string playerName = nameInputField.text;

        if (!string.IsNullOrEmpty(playerName))
        {
            // Save the name in PlayerPrefs so it persists
            PlayerPrefs.SetString("DriverName", playerName);
            PlayerPrefs.Save();

            // Load the Garage Scene
            SceneManager.LoadScene("Garage");
        }
        else
        {
            Debug.LogWarning("Please enter a name!"); // Optional: Alert if name is empty
        }
    }
}


