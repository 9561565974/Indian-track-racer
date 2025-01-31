using GoogleMobileAds.Api;
using UnityEngine;

public class AdsInitializer : MonoBehaviour
{
    private string appId = "ca-app-pub-2160364857686440~2116462291";  // Replace with your AdMob App ID

    void Start()
    {
        // Initialize the Mobile Ads SDK
        MobileAds.Initialize(initStatus => {
            Debug.Log("AdMob SDK Initialized: " + initStatus);
            // You can check the initialization status and log or perform additional actions here if needed
        });
    }
}

