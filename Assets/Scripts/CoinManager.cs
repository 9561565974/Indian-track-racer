using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public int totalCoins;  // Do not initialize to 10000 here; load it from PlayerPrefs instead
    public TMP_Text coinText;  // Reference to TMP_Text to update UI

    void Start()
    {
        LoadCoins();  // Load saved coins when the game starts
        UpdateCoinDisplay();  // Update the coin display in the UI
    }

    public bool SpendCoins(int amount)
    {
        if (totalCoins >= amount)
        {
            totalCoins -= amount;
            SaveCoins();  // Save the updated coin amount
            UpdateCoinDisplay();  // Update the coin display
            Debug.Log("Coins deducted: " + amount);
            return true;
        }
        else
        {
            Debug.Log("Not enough coins.");
            return false;
        }
    }

    public void AddCoins(int amount)
    {
        LoadCoins();  // Ensure we load the latest coin balance before adding
        Debug.Log("Coins before addition: " + totalCoins);

        totalCoins += amount;  // Add the specified amount of coins (500 in this case)

        Debug.Log("Added " + amount + " coins. New totalCoins: " + totalCoins);

        SaveCoins();  // Save the updated coin amount after adding coins
        UpdateCoinDisplay();  // Update the coin display
    }

    private void SaveCoins()
    {
        PlayerPrefs.SetInt("TotalCoins", totalCoins);  // Save the updated coin balance to PlayerPrefs
        PlayerPrefs.Save();  // Ensure it's written to disk
        Debug.Log("Coins Saved: " + totalCoins);  // Log to confirm the coin value is saved
    }

    private void LoadCoins()
    {
        // Load the coin balance from PlayerPrefs, default to 0 if not set
        totalCoins = PlayerPrefs.GetInt("TotalCoins", totalCoins);
        Debug.Log("Coins Loaded: " + totalCoins);  // Log to check if loading is correct
    }

    private void UpdateCoinDisplay()
    {
        if (coinText != null)
        {
            coinText.text = totalCoins.ToString();  // Update the coin display text
        }
    }
}





