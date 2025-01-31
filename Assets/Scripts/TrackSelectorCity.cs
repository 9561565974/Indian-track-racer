using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;

public class TrackSelectorCity : MonoBehaviour
{
    public string trackSceneName; // Set the scene name for this track in the Inspector
    public string rewardedAdUnitId = "ca-app-pub-2160364857686440/2782676709"; // Replace with your AdMob Unit ID
    private RewardedAd rewardedAd;

    void Start()
    {
        // Initialize the Google Mobile Ads SDK
        MobileAds.Initialize(initStatus => { });

        // Load the rewarded ad
        RequestRewardedAd();
    }

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
                Debug.LogError("Rewarded ad failed to load with error: " + error.GetMessage());
                return;
            }

            rewardedAd = ad;
            Debug.Log("Rewarded ad loaded successfully.");
        });
    }

    public void OnTrackButtonClick()
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                Debug.Log("Ad watched. Rewarding the player.");
                LoadTrackScene(); // Load the scene after the ad
            });
        }
        else
        {
            Debug.LogWarning("Rewarded ad not ready, loading track directly.");
            LoadTrackScene(); // Load the scene directly if the ad is not ready
        }
    }

    private void LoadTrackScene()
    {
        if (!string.IsNullOrEmpty(trackSceneName))
        {
            Debug.Log("Loading track scene: " + trackSceneName);
            SceneManager.LoadScene(trackSceneName);
        }
        else
        {
            Debug.LogError("Track scene name is not set.");
        }
    }
}


