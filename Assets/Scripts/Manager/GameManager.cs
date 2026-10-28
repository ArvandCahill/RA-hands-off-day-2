using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [Header("Sub-Managers")]
    [field: SerializeField] public AudioManager audioManager { get; private set; }

    public SceneLoader SceneLoader { get; private set; }

    [Header("Game State")]
    public GameState gameState;

    [Header("Game Settings")]
    public bool isBgmOn = true;
    public bool isSfxOn = true;
    public bool isVibrationOn = true;

    [Header("Save Settings")]
    public bool isFirstTimePlaying;
    public bool isFirstTimeShop;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;

        SceneLoader = GetComponent<SceneLoader>();

        LoadResource();
    }

    void Start()
    {
        Application.targetFrameRate = 60;

        audioManager?.Initialize(isBgmOn, isSfxOn);

        ApplyAudioSettings();
    }

    public void ApplyAudioSettings()
    {
        audioManager?.SetBgmActive(isBgmOn);
        audioManager?.SetSfxActive(isSfxOn);
    }

    //private void Update()
    //{
    //    if (Input.touchCount > 0)
    //    {
    //        Touch touch = Input.GetTouch(0);

    //        if (touch.phase == TouchPhase.Began)
    //        {
    //            audioManager.PlaySFX("Tap");
    //        }
    //    }
    //}

    public void SetGameState(GameState newGameState) => gameState = newGameState;

    private void LoadResource()
    {
        
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;


        //if (sceneName == "MainMenu" || sceneName == "ModeSelector") admobInit.DestroyBanner();
        //else admobInit.RequestBanner();
        //StartCoroutine(admobInit.ShowInterstitalWithDelay(sceneCount));
    }
}