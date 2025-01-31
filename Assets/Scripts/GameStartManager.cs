using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Required for IEnumerator

public class GameStartManager : MonoBehaviour
{
    public GameObject termsCanvas;      // Reference to the Terms and Conditions Canvas
    public GameObject nameInputCanvas; // Reference to the Name Input Canvas

    void Start()
    {
        // Initially, only show the Terms and Conditions Canvas
        termsCanvas.SetActive(true);
        nameInputCanvas.SetActive(false);

        // Start the process
        StartCoroutine(HandleGameStart());
    }

    private IEnumerator HandleGameStart()
    {
        // Check if the player's name is already saved
        if (PlayerPrefs.HasKey("DriverName"))
        {
            yield return new WaitForSeconds(7f);
            // Name exists, show Terms Canvas until Garage Scene is fully loaded
            yield return StartCoroutine(LoadSceneWithTerms("Garage"));
        }
        else
        {
            // Wait for the Terms Canvas display time (e.g., 7 seconds)
            yield return new WaitForSeconds(7f);

            // Hide Terms Canvas and show Name Input Canvas
            termsCanvas.SetActive(false);
            nameInputCanvas.SetActive(true);
        }
    }

    private IEnumerator LoadSceneWithTerms(string sceneName)
    {
        // Begin loading the scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        // Wait until the scene is almost fully loaded
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                // Scene is ready, keep showing Terms Canvas until activation
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}



