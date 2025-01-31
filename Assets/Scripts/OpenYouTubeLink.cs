using UnityEngine;
using UnityEngine.UI;

public class OpenYouTubeLink : MonoBehaviour
{
    public Button youtubeButton; // Reference to the Button

    // YouTube channel URL
    private string youtubeURL = "https://www.youtube.com/@EpicGamePresents";

    void Start()
    {
        // Add listener for the button click
        if (youtubeButton != null)
        {
            youtubeButton.onClick.AddListener(OpenYouTubeChannel);
        }
    }

    // Method to open the YouTube channel link
    void OpenYouTubeChannel()
    {
        Application.OpenURL(youtubeURL);
    }
}

