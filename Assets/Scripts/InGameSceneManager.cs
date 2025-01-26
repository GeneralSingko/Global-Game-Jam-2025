using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameSceneManager : MonoBehaviour
{
    public static InGameSceneManager Instance;

    [Header("Load Scene Name Strings")]
    [SerializeField] string mainMenuScene;
    [SerializeField] string startLoad;
    [SerializeField] string afterThisLevel;

    [Header("Same menu buttons")]
    [SerializeField] GameObject openPauseMenu;
    [SerializeField] GameObject inGameOptions;
    [SerializeField] GameObject gameOverScene;
    [SerializeField] GameObject winScreen;

    [Header("Buttons Functions Caller")]
    [Header("Game Scene")]
    [SerializeField] Button unpause;
    [SerializeField] Button settings;
    [SerializeField] Button backToMenu;
    [SerializeField] Button backToMainMenu;
    [SerializeField] Button backToMainMenuLose;
    [SerializeField] Button retry;
    [SerializeField] Button winMenu;

    [Header("UI Buttons")]
    [SerializeField] KeyCode pauseButton = KeyCode.Escape;

    [Header("Wave Amount handler")]
    [SerializeField] EnemySpawner enemySpawner;
    [SerializeField] int amountOfWaves;
    [SerializeField] int winWaveCount;

    public bool gameIsPaused;

    public void Awake()
    {
        //Initiates singleton Scene Manager
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameIsPaused = false;
        //In Game stuff
        unpause.onClick.AddListener(UnloadPauseMenu);
        settings.onClick.AddListener(LoadInGameOptions);
        backToMenu.onClick.AddListener(UnloadInGameOptions);
        backToMainMenu.onClick.AddListener(BackToMainMenu);
        retry.onClick.AddListener(retryLevel);
        backToMainMenuLose.onClick.AddListener(BackToMainMenu);
        winMenu.onClick.AddListener(BackToMainMenu);
    }

    private void Update()
    {
        if (Input.GetKeyDown(pauseButton) && openPauseMenu.activeSelf == false)
        {
            LoadPauseMenu();
        } else if((Input.GetKeyDown(pauseButton) && openPauseMenu.activeSelf == true))
        {
            UnloadPauseMenu();
        }

        if (enemySpawner.waveCount >= amountOfWaves)
        {
            AfterLevel();
        }
        if (enemySpawner.waveCount >= winWaveCount)
        {
            WinGame();
        }
    }
    //In game UI
    void UnloadPauseMenu()
    {
        Time.timeScale = 1;
        gameIsPaused = false;
        openPauseMenu.SetActive(false);
    }
    void LoadPauseMenu()
    {
        Time.timeScale = 0;
        gameIsPaused = true;
        openPauseMenu.SetActive(true);
        inGameOptions.SetActive(false);
    }
    void LoadInGameOptions()
    {
        openPauseMenu.SetActive(false);
        inGameOptions.SetActive(true);
    }

    void UnloadInGameOptions()
    {
        openPauseMenu.SetActive(true);
        inGameOptions.SetActive(false);
    }

    void BackToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(mainMenuScene);
    }

    public void GameOverScreen()
    {
        Time.timeScale = 0;
        gameIsPaused = true;
        gameOverScene.SetActive(true);
    }

    void retryLevel()
    {
        gameIsPaused = false;
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
        Time.timeScale = 1;
    }

    //Scene transition 
    void AfterLevel()
    {
        SceneManager.LoadScene(afterThisLevel);
    }
    void WinGame()
    {
        Time.timeScale = 0;
        winScreen.SetActive(true);
        openPauseMenu.SetActive(false);
        inGameOptions.SetActive(false);
    }
}
