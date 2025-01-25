using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameSceneManager : MonoBehaviour
{
    public static InGameSceneManager Instance;

    [Header("Load Scene Name Strings")]
    [SerializeField] string mainMenuScene;
    [SerializeField] string startLoad;

    [Header("Same menu buttons")]
    [SerializeField] GameObject openPauseMenu;
    [SerializeField] GameObject inGameOptions;
    [SerializeField] GameObject gameOverScene;

    [Header("Buttons Functions Caller")]
    [Header("Game Scene")]
    [SerializeField] Button unpause;
    [SerializeField] Button settings;
    [SerializeField] Button backToMenu;
    [SerializeField] Button backToMainMenu;
    [SerializeField] Button backToMainMenuLose;
    [SerializeField] Button retry;

    [Header("UI Buttons")]
    [SerializeField] KeyCode pauseButton = KeyCode.Escape;

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
    }

    private void Update()
    {
        if (Input.GetKeyDown(pauseButton))
        {
            LoadPauseMenu();
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
        Time.timeScale = 1;
        gameIsPaused = false;
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
