using Apps;
using UnityEngine;
using UnityEngine.UI;

public class ExampleScript : MonoBehaviour
{
    public InputField inputField;
    public GameObject NoAdsButton;

    private void Start()
    {
        ADSManager.LoadInterstitial();
        ADSManager.LoadRewardedVideo();

        NoAdsButton.SetActive(!ADSManager.IsNoAds);
    }

    public void ShowRewardedVideo()
    {
        ADSManager.ShowRewardedVideo("Coins3X", OnCompletedRewardedVideo, OnClosedRewardedVideo, OnFailedRewardedVideo);
    }

    public void ShowInterstitial()
    {
        ADSManager.ShowInterstitial("OnFinished", OnClosedInterstitial, OnFailedInterstitial);
    }

    private void OnClosedInterstitial()
    {
        inputField.text = "OnClosedInterstitial";
    }

    private void OnFailedInterstitial()
    {
        inputField.text = "OnFailedInterstitial";
    }

    private void OnCompletedRewardedVideo()
    {
        inputField.text = "OnCompletedRewardedVideo";
    }

    private void OnClosedRewardedVideo()
    {
        inputField.text = "OnClosedRewardedVideo";
    }

    private void OnFailedRewardedVideo()
    {
        inputField.text = "OnFailedRewardedVideo";
    }

    public void OnClickRemoveAds()
    {
        ADSManager.RemoveAds();
        NoAdsButton.SetActive(!ADSManager.IsNoAds);
        EventsLogger.IAPEvent(new InAppInfo("rmvads", "US", 1.99f, "Removead"));
    }

    public void BuyProduct()
    {
        EventsLogger.IAPEvent(new InAppInfo("rmvads", "US", 1.99f, "Removead"));
    }

    public void ShowBanner()
    {
        ADSManager.DisplayBanner();
    }

    public void HideBanner()
    {
        ADSManager.HideBanner();
    }

    public void SendEvent()
    {
        EventsLogger.CustomEvent(inputField.text);
    }

    public void SendEvent(string eventName)
    {
        EventsLogger.CustomEvent(eventName);
    }
}