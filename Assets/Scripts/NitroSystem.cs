using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NitroSystem : MonoBehaviour
{
    // Nitro settings
    public float nitroMultiplier = 2f;    // Nitro speed boost multiplier
    public float nitroDuration = 3f;       // Duration of nitro boost
    public float nitroCooldown = 5f;       // Cooldown before nitro can be used again
    public float nitroAcceleration = 10f;  // Extra speed added during nitro

    // Nitro UI and effects
    public Slider nitroBar;                // Nitro bar reference
    public ParticleSystem[] nitroParticles;  // Array of particle systems for nitro effects
    public Button nitroButton;             // Nitro button reference

    private bool isNitroActive = false;    // Is nitro currently active?
    private bool canUseNitro = true;       // Can the player use nitro?
    private float nitroElapsed = 0f;       // Track nitro time

    // Reference to the car's speed from CarController
    private CarController carController;

    void Start()
    {
        // Find the CarController script on the same GameObject
        carController = GetComponent<CarController>();

        // Add button listener for nitro activation
        nitroButton.onClick.AddListener(ActivateNitro);

        // Ensure all particle systems do not play on awake
        foreach (var particleSystem in nitroParticles)
        {
            particleSystem.Stop();
        }

        // Initialize the nitro bar to full
        nitroBar.value = 1f;
    }

    void FixedUpdate()
    {
        // Apply additional acceleration only when nitro is active
        if (isNitroActive)
        {
            carController.GetComponent<Rigidbody>().AddForce(transform.forward * nitroAcceleration, ForceMode.Acceleration);
        }
    }

    void Update()
    {
        // Update the nitro bar's value based on time
        if (isNitroActive)
        {
            nitroElapsed += Time.deltaTime;
            nitroBar.value = Mathf.Lerp(1f, 0f, nitroElapsed / nitroDuration);
        }

        // Check if nitro should end
        if (nitroElapsed >= nitroDuration && isNitroActive)
        {
            DeactivateNitro();
        }
    }

    public void ActivateNitro()
    {
        if (canUseNitro)
        {
            canUseNitro = false;
            isNitroActive = true;
            nitroElapsed = 0f;

            // Apply nitro speed boost from the CarController
            carController.motorForce *= nitroMultiplier;

            // Activate all nitro visual effects
            foreach (var particleSystem in nitroParticles)
            {
                particleSystem.Play();
            }
            nitroBar.value = 1f;    // Reset nitro bar to full
        }
    }

    void DeactivateNitro()
    {
        isNitroActive = false;

        // Reset car speed to normal in CarController
        carController.motorForce /= nitroMultiplier;

        // Stop all nitro visual effects
        foreach (var particleSystem in nitroParticles)
        {
            particleSystem.Stop();
        }

        // Start cooldown before nitro can be used again
        StartCoroutine(NitroCooldown());
    }

    IEnumerator NitroCooldown()
    {
        // Wait for cooldown period
        yield return new WaitForSeconds(nitroCooldown);

        // Reset nitro bar to full
        nitroBar.value = 1f;

        // Allow nitro to be used again
        canUseNitro = true;
    }
}



