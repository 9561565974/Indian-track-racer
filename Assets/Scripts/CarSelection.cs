using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CarSelection : MonoBehaviour
{
    [Header("Button and Canvas")]
    public Button nextButton;
    public Button previousButton;
    public Button buyButton;

    [Header("Text Display")]
    public TMP_Text priceText;

    [Header("Cameras")]
    public GameObject cam1;
    public GameObject cam2;

    [Header("Buttons and Canvas")]
    public GameObject SelectionCanvas;
    public GameObject SkipButton;
    public GameObject PlayButton;

    private int currentCar;
    private GameObject[] carList;
    private bool[] carPurchased; // Track if each car has been purchased
    public int[] carPrices; // Public array to set prices in the Inspector
    public CoinManager coinManager;

    private void Awake()
    {
        // Initialize carList array with the number of child objects
        carList = new GameObject[transform.childCount];

        // Populate the carList array with child GameObjects
        for (int i = 0; i < transform.childCount; i++)
        {
            carList[i] = transform.GetChild(i).gameObject;
        }

        // Initialize carPurchased array with the number of cars
        carPurchased = new bool[carList.Length];

        // Ensure UI and camera setup
        if (SelectionCanvas != null)
            SelectionCanvas.SetActive(false);

        if (PlayButton != null)
            PlayButton.SetActive(false); // Disable PlayButton initially

        if (SkipButton != null)
            SkipButton.SetActive(true);

        if (cam2 != null)
            cam2.SetActive(false);

        // Call chooseCar with default index
        chooseCar(0);
    }

    private void Start()
    {
        // Retrieve the selected car index from PlayerPrefs
        currentCar = PlayerPrefs.GetInt("CarSelected", 0);

        // Load purchase status for each car
        for (int i = 0; i < carList.Length; i++)
        {
            // Mark car index 0 as purchased forever
            if (i == 0)
            {
                carPurchased[i] = true;
                PlayerPrefs.SetInt("CarPurchased_" + i, 1); // Save as purchased
            }
            else
            {
                carPurchased[i] = PlayerPrefs.GetInt("CarPurchased_" + i, 0) == 1;
            }
        }

        // Call chooseCar to handle car selection and button states
        chooseCar(currentCar);
    }


    private void chooseCar(int index)
    {
        // Ensure car list is populated
        if (carList == null || carList.Length == 0)
        {
            Debug.LogError("No cars found in the scene.");
            return;
        }

        // Set navigation buttons
        if (previousButton != null)
            previousButton.interactable = (currentCar != 0);

        if (nextButton != null)
            nextButton.interactable = (currentCar != carList.Length - 1);

        // Activate selected car and deactivate others
        for (int i = 0; i < carList.Length; i++)
        {
            if (carList[i] != null)
                carList[i].SetActive(i == index);
            else
                Debug.LogError("Car at index " + i + " is null.");
        }

        // Update UI for car price and Buy button
        if (carPurchased[index])
        {
            if (buyButton != null)
                buyButton.gameObject.SetActive(false);

            if (PlayButton != null)
                PlayButton.SetActive(true); // Enable PlayButton only if car is purchased
        }
        else
        {
            if (buyButton != null)
                buyButton.gameObject.SetActive(true);

            if (PlayButton != null)
                PlayButton.SetActive(false); // Disable PlayButton if car is not purchased

            if (priceText != null && index < carPrices.Length)
                priceText.text = "Price: " + carPrices[index] + " Coins";
        }
    }

    public void switchCar(int switchCars)
    {
        currentCar = Mathf.Clamp(currentCar + switchCars, 0, carList.Length - 1);
        chooseCar(currentCar);
    }

    public void playGame()
    {
        PlayerPrefs.SetInt("CarSelected", currentCar);
        SceneManager.LoadScene("Tracks");
    }

    public void skipButton()
    {
        if (SelectionCanvas != null)
            SelectionCanvas.SetActive(true);

        if (SkipButton != null)
            SkipButton.SetActive(false);

        if (cam1 != null)
            cam1.SetActive(false);

        if (cam2 != null)
            cam2.SetActive(true);

        chooseCar(currentCar);
    }

    public void buyCar()
    {
        if (currentCar >= carPrices.Length)
        {
            Debug.LogError("Car price for index " + currentCar + " is not set.");
            return;
        }

        int carPrice = carPrices[currentCar];

        if (coinManager == null)
        {
            Debug.LogError("coinManager is not assigned!");
            return;
        }

        if (coinManager.SpendCoins(carPrice))
        {
            carPurchased[currentCar] = true;
            PlayerPrefs.SetInt("CarPurchased_" + currentCar, 1);
            chooseCar(currentCar);
        }
        else
        {
            Debug.Log("Not enough coins to buy this car.");
        }
    }
}





