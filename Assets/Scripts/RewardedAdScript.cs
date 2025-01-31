using System;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;


public class RewardedAdScript : MonoBehaviour
{
    private RewardedAd rewardedAd;

    // Your Ad Unit ID for Rewarded Ads
    public string rewardedAdUnitId = "ca-app-pub-2160364857686440/2782676709";

    private string sceneToLoad; // Store the track scene to load

    void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });

        // Request and load a rewarded ad
        RequestRewardedAd();
    }

    // Request and load the rewarded ad
    private void RequestRewardedAd()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");
        var adRequest = new AdRequest();

        RewardedAd.Load(rewardedAdUnitId, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Rewarded ad failed to load with error: " + error);
                return;
            }

            rewardedAd = ad;
            Debug.Log("Rewarded ad loaded.");
        });
    }

    // Show the rewarded ad
    public void ShowRewardedAd(string trackScene)
    {
        sceneToLoad = trackScene; // Set the scene to load after the ad

        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                Debug.Log("Reward received: Type: " + reward.Type + ", Amount: " + reward.Amount);
                LoadSelectedScene(); // Load the selected track scene
            });
        }
        else
        {
            Debug.LogError("Rewarded ad is not ready.");
            LoadSelectedScene(); // Load the scene directly if ad is not ready
        }
    }

    private void LoadSelectedScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("No scene specified to load.");
        }
    }
}



