using UnityEngine;
using TMPro;

public class WaypointLapTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;       // TextMeshPro UI for displaying the timer
    public TextMeshProUGUI startTimerText;  // TextMeshPro UI for displaying the start countdown timer
    public float startTimer = 5f;          // Countdown timer before the lap begins
    public float totalTime = 120f;         // Total time for the lap (adjust as needed)
    private float currentTime;

    public GameObject finishPanel;         // UI Panel for race result
    public TextMeshProUGUI finishMessage;  // TextMeshPro UI for success or failure message

    public GameObject waypointManager;     // Parent GameObject for waypoints
    public Transform player;               // Player's Transform
    private Transform[] waypoints;         // Array to store all waypoints
    private int currentWaypointIndex = 0;  // Track current waypoint

    public float waypointReachDistance = 5f; // Distance to consider a waypoint reached
    private bool lapStarted = false;       // Flag to indicate if the lap timer is running

    void Start()
    {
        currentTime = totalTime;
        finishPanel.SetActive(false);

        // Fetch all waypoints dynamically from the WaypointManager
        waypoints = waypointManager.GetComponentsInChildren<Transform>();

        // Remove the first element if it's the parent object
        if (waypoints.Length > 0 && waypoints[0] == waypointManager.transform)
        {
            waypoints = waypoints[1..]; // Slicing to skip the parent object
        }
    }

    void Update()
    {
        if (!lapStarted)
        {
            // Handle the countdown timer before the lap starts
            if (startTimer > 0)
            {
                startTimer -= Time.deltaTime;
                startTimerText.text = $"Race Starts In: {Mathf.Ceil(startTimer)}s";
            }
            else
            {
                // Start the lap timer
                lapStarted = true;
                startTimerText.gameObject.SetActive(false); // Hide the countdown timer
            }
        }
        else
        {
            // Handle the lap timer
            currentTime -= Time.deltaTime;
            timerText.text = $"Time Left: {Mathf.Max(0, currentTime):F2}s";

            // Check if time runs out
            if (currentTime <= 0)
            {
                FinishRace(false);
            }

            // Check if the player is close to the current waypoint
            CheckWaypoint(waypoints[currentWaypointIndex]); // Pass the correct waypoint Transform
        }
    }

    public void CheckWaypoint(Transform waypoint) // Now accepts a waypoint Transform
    {
        if (currentWaypointIndex < waypoints.Length)
        {
            if (waypoints[currentWaypointIndex] == waypoint)
            {
                currentWaypointIndex++;

                // Check if all waypoints are completed
                if (currentWaypointIndex >= waypoints.Length)
                {
                    FinishRace(true);
                }
            }
        }
    }

    private void FinishRace(bool success)
    {
        finishPanel.SetActive(true);
        finishMessage.text = success ? "Race Complete!" : "Time's Up! Try Again.";
        Time.timeScale = 0; // Pause the game
    }
}

