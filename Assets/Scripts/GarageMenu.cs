using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GarageMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public Toggle soundToggle;
    public Button pauseButton;
    public Button resumeButton;
    public Button quitButton;

    // List of buttons to disable when the menu is active
    public List<Button> buttonsToDisable;

    private bool isPaused = false;

    void Start()
    {
        pauseMenuPanel.SetActive(false);

        pauseButton.onClick.AddListener(TogglePause);
        resumeButton.onClick.AddListener(ResumeGame);
        quitButton.onClick.AddListener(QuitGame);
        soundToggle.onValueChanged.AddListener(SetSound);

        LoadPreferences();
    }

    void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pauseMenuPanel.SetActive(true);

        // Completely disable all specified buttons when the menu is active
        DisableButtons();
    }

    void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseMenuPanel.SetActive(false);

        // Re-enable all specified buttons when the menu is hidden
        EnableButtons();

        SavePreferences();
    }

    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void SetSound(bool isSoundOn)
    {
        AudioListener.volume = isSoundOn ? 1 : 0;
    }

    void SetControlScheme(string scheme)
    {
        PlayerPrefs.SetString("ControlScheme", scheme);
        PlayerPrefs.Save();
    }

    void LoadPreferences()
    {
        if (PlayerPrefs.HasKey("SoundEnabled"))
        {
            soundToggle.isOn = PlayerPrefs.GetInt("SoundEnabled") == 1;
            AudioListener.volume = soundToggle.isOn ? 1 : 0;
        }
    }

    void SavePreferences()
    {
        PlayerPrefs.SetInt("SoundEnabled", soundToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    // Disable all specified buttons
    void DisableButtons()
    {
        foreach (Button button in buttonsToDisable)
        {
            if (button != null)
            {
                button.gameObject.SetActive(false); // Make each button invisible and non-functional
            }
        }
    }

    // Enable all specified buttons
    void EnableButtons()
    {
        foreach (Button button in buttonsToDisable)
        {
            if (button != null)
            {
                button.gameObject.SetActive(true); // Make each button visible and functional again
            }
        }
    }
}



















