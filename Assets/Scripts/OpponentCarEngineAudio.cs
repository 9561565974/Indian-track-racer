using UnityEngine;

public class OpponentCarEngineAudio : MonoBehaviour
{
    public AudioSource engineAudioSource; // Reference to the AudioSource component for engine sound
    public float maxDistance = 50f;  // Maximum distance at which sound is audible
    public float minSpeed = 15f;     // Minimum speed at which the sound starts
    public float maxSpeed = 30f;     // Maximum speed at which the sound reaches full volume
    public float volumeRange = 0.5f; // Adjust this to control the audio volume range
    public float pitchRange = 2f;    // Adjust this to control the audio pitch range

    private float currentSpeed;
    private float distanceToPlayer;
    private float adjustedSpeed;
    private GameObject playerCar;    // Reference to the Player's car
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Ensure AudioSource is assigned in the Inspector
        if (engineAudioSource == null)
        {
            engineAudioSource = GetComponent<AudioSource>();
        }

        // Find the Player car by tag
        playerCar = GameObject.FindGameObjectWithTag("Player");

        if (playerCar == null)
        {
            Debug.LogError("No GameObject with the 'Player' tag found.");
        }
    }

    void Update()
    {
        if (playerCar == null || engineAudioSource == null) return;

        // Calculate the distance to the player car
        distanceToPlayer = Vector3.Distance(transform.position, playerCar.transform.position);

        // If the opponent car is close enough, update the audio based on speed and distance
        if (distanceToPlayer <= maxDistance)
        {
            // Calculate the car's speed
            currentSpeed = rb.linearVelocity.magnitude;

            // Adjust speed for the engine audio
            adjustedSpeed = Mathf.InverseLerp(minSpeed, maxSpeed, currentSpeed);
            float volume = Mathf.Lerp(0, volumeRange, Mathf.InverseLerp(0, maxDistance, distanceToPlayer));
            float pitch = Mathf.Lerp(1, pitchRange, adjustedSpeed);

            // Set the volume and pitch of the engine sound based on speed and distance
            engineAudioSource.volume = volume;  // Set engine volume
            engineAudioSource.pitch = pitch;    // Set engine pitch
        }
        else
        {
            // Mute the engine sound when the opponent car is too far away
            engineAudioSource.volume = 0f;  // Mute the sound
        }
    }
}





