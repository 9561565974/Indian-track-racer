using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    public GameObject loadingScreen;  // Reference to the loading screen
    public Image rotatingSlider;      // The rotating slider image

    // Call this method to load a scene with a loading screen
    public void LoadScene(string sceneName)
    {
        // Show the loading screen and start loading the scene asynchronously
        loadingScreen.SetActive(true);
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    // Coroutine to load the scene asynchronously
    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // While the scene is not done loading
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);  // Normalize progress
            if (rotatingSlider != null)
                rotatingSlider.fillAmount = progress;  // Update the rotating slider

            yield return null;  // Wait until the next frame
        }
    }
}














