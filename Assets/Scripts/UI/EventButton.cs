using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;

public class EventButton : MonoBehaviour {

    public bool mainMenu;

    private bool gameStart;
    private bool gamePause;
    private MainController mc;

    public GameObject startPanel;
    public GameObject pausePanel;

    private BannerView bannerView;

    void Awake()
    {
        mc = GameObject.FindObjectOfType<MainController>();
        if (!mainMenu) Time.timeScale = 0f;
    }

    void Start()
    {
#if UNITY_ANDROID
        string appId = "ca-app-pub-3940256099942544~3347511713";
#elif UNITY_IPHONE
        string appId = "ca-app-pub-3940256099942544~1458002511";
#else
        string appId = "unexpected_platform";
#endif

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);

        RequestBanner();
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        gameStart = true;
        startPanel.SetActive(false);
        DestroyBanner();
    }

    public void PauseGame()
    {
        if(mc.life > 0 && gameStart)
        {
            if (gamePause)
            {
                Time.timeScale = 1f;
                gamePause = false;
                pausePanel.SetActive(false);
                DestroyBanner();
            }
            else
            {
                Time.timeScale = 0f;
                gamePause = true;
                pausePanel.SetActive(true);
                RequestBanner();
            }
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }

    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void RequestBanner()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Create a 320x50 banner at the top of the screen.
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().AddTestDevice("2077ef9a63d2b398840261c8221a0c9b").Build();

        // Load the banner with the request.
        bannerView.LoadAd(request);
        bannerView.Show();
    }

    void DestroyBanner()
    {
        bannerView.Destroy();
        bannerView.Hide();
    }

}
