using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [Header("All Menu's")]

    public GameObject pauseMenuUI;
    public GameObject playerUI;
    public static bool GameIsStopped = false;

    public void Resume ()
    {
        pauseMenuUI.SetActive(false);
        playerUI.SetActive(true);
        Time.timeScale = 1f;
        GameIsStopped = false;
        

    }
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        playerUI.SetActive(false);
        Time.timeScale = 0f;
        GameIsStopped = true;

        
    }

    public void Restart()
    {
        // Get the currently active scene and reload it
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);  // Reload the current scene
        Time.timeScale = 1f;  // Reset the time scale in case it was paused or slowed down
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Garage");
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");

#if UNITY_EDITOR
        // If in Unity Editor, stop play mode
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // Quit the game in the built version
            Application.Quit();
#endif
    }

    public void LoadTrack()
    {
        SceneManager.LoadScene("Tracks");
        Time.timeScale = 1f;
    }


}
