using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RaceCountdown : MonoBehaviour
{
    public int countdownTime = 3; // Countdown start time
    public TMP_Text countdownDisplay; // UI Text to display countdown
    public GameObject[] playerCars; // Array to store player cars
    public GameObject[] aiCars; // Array to store AI cars

    private void Start()
    {
        StartCoroutine(CountdownToStart());
    }

    IEnumerator CountdownToStart()
    {
        // Disable player and AI car controls
        ToggleCarControls(false);

        // Countdown loop
        while (countdownTime > 0)
        {
            countdownDisplay.text = countdownTime.ToString(); // Update the UI Text with the current countdown time
            yield return new WaitForSeconds(1f); // Wait for one second
            countdownTime--; // Decrement the countdown time
        }

        // Start the race
        countdownDisplay.text = "GO!"; // Display "GO!" message
        ToggleCarControls(true); // Enable player and AI car controls

        yield return new WaitForSeconds(1f); // Wait for one second before hiding the "GO!" message
        countdownDisplay.gameObject.SetActive(false); // Hide the countdown display
    }

    private void ToggleCarControls(bool state)
    {
        foreach (GameObject car in playerCars)
        {
            car.GetComponent<CarController>().enabled = state; // Enable/disable the player's car controller
        }

        foreach (GameObject aiCar in aiCars)
        {
            aiCar.GetComponent<OpponentCarController>().enabled = state; // Enable/disable the AI car controller
        }
    }
}

