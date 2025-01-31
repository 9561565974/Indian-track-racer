using UnityEngine;
using TMPro;
using System.Collections;

public class Finish : MonoBehaviour
{
    [Header("Finish UI Var")]
    public GameObject finishUI;
    public GameObject playerUI;
    public GameObject playerCar;
    public TMP_Text status;
    public TMP_Text coinsWonText;
    public int coinsWonOnWin = 500;  // Coins won per race

    public CoinManager coinManager;  // Reference to CoinManager

    public GameObject[] playerSpeedometerCanvases;

    [Header("Control Buttons")]
    public GameObject controlButtonsCanvas1;  // Reference to the first control buttons UI
    public GameObject controlButtonsCanvas2;  // Reference to the second control buttons UI
    public GameObject controlButtonsCanvas3;  // Reference to the third control buttons UI

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(finishZoneTimer());
            gameObject.GetComponent<BoxCollider>().enabled = false;

            status.text = "You Win";
            status.color = Color.black;

            // Add coins when the player wins (ensure it's only added once)
            coinManager.AddCoins(coinsWonOnWin);

            // Update the coins won text
            if (coinsWonText != null)
            {
                coinsWonText.text = "+" + coinsWonOnWin.ToString() + " Coins";
            }
        }
        else if (other.gameObject.tag == "OpponentCar")
        {
            StartCoroutine(finishZoneTimer());
            gameObject.GetComponent<BoxCollider>().enabled = false;

            status.text = "You Lose";
            status.color = Color.red;
        }
    }

    private IEnumerator finishZoneTimer()
    {
        yield return new WaitForSeconds(1f);

        finishUI.SetActive(true);
        playerUI.SetActive(false);

        // Disable control button canvases when the Finish UI is shown
        if (controlButtonsCanvas1 != null)
        {
            controlButtonsCanvas1.SetActive(false);
        }

        if (controlButtonsCanvas2 != null)
        {
            controlButtonsCanvas2.SetActive(false);
        }

        if (controlButtonsCanvas3 != null)
        {
            controlButtonsCanvas3.SetActive(false);
        }

        StopGame();
    }

    private void StopGame()
    {
        // Pause the game
        Time.timeScale = 0;

        // Stop all active audio sources
        foreach (var audioSource in Object.FindObjectsByType<AudioSource>(FindObjectsSortMode.InstanceID))
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }

        // Disable player speedometer canvases if they exist
        if (playerSpeedometerCanvases != null && playerSpeedometerCanvases.Length > 0)
        {
            foreach (GameObject canvas in playerSpeedometerCanvases)
            {
                if (canvas != null)
                {
                    canvas.SetActive(false);
                }
            }
        }
    }

}






