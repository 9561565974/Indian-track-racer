using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RaceCompletion : MonoBehaviour
{
    public TextMeshProUGUI lapText;    // To display lap progress
    public TextMeshProUGUI timerText; // To display the timer
    private int currentLap = 0;
    private int totalLaps = 1;

    private float raceStartTime;      // Start time of the race
    private bool raceFinished = false; // To check if the race is completed
    private bool raceStarted = false; // To check if the timer has started

    private void Start()
    {
        // Initialize the lap text
        UpdateLapText();

        // Start the coroutine to delay the timer
        StartCoroutine(StartTimerAfterDelay());
    }

    private void Update()
    {
        if (raceStarted && !raceFinished)
        {
            // Update the timer text while the race is ongoing
            float currentTime = Time.time - raceStartTime;
            timerText.text = FormatTime(currentTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that triggered is the player
        if (other.CompareTag("Player"))
        {
            // Update lap count
            currentLap++;
            if (currentLap >= totalLaps)
            {
                currentLap = totalLaps;
                raceFinished = true; // Stop the timer
                Debug.Log("Race Finished!");
            }

            // Update the lap text
            UpdateLapText();
        }
    }

    private void UpdateLapText()
    {
        lapText.text = $"{currentLap}/{totalLaps}";
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt((time * 1000) % 1000);
        return $"{minutes:00}:{seconds:00}:{milliseconds:000}";
    }

    private IEnumerator StartTimerAfterDelay()
    {
        // Wait for 4 seconds
        yield return new WaitForSeconds(4f);

        // Start the race
        raceStartTime = Time.time;
        raceStarted = true;
        Debug.Log("Race Timer Started!");
    }
}
