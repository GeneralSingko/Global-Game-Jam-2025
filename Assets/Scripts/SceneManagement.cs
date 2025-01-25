using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagement : MonoBehaviour
{
    public static SceneManagement Instance;

    [Header("Load Scene Name Strings")]
    [SerializeField] string startLoad;
    [SerializeField] string mainMenuScene;

    [Header("Same menu buttons")]
    [SerializeField] GameObject mainMenuButtons;
    [SerializeField] GameObject settingsButtons;

    [Header("Buttons Functions Caller")]
    [Header("Main Menu")]
    [SerializeField] Button startButton;
    [SerializeField] Button optionsButton;
    [SerializeField] Button exitButton;
    [SerializeField] Button backToMenuButton;

    public void Awake()
    {
        //Initiates singleton Scene Manager
        if(Instance == null)
        {
            Instance = this;
        } else if(Instance != this)
        {
            Destroy(gameObject); 
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Main Menu stuff
        startButton.onClick.AddListener(StartGame);
        optionsButton.onClick.AddListener(LoadOptions);
        backToMenuButton.onClick.AddListener(UnloadOptions);
        exitButton.onClick.AddListener(ExitGame);
    }

    //Loads first level when start is pressed
    void StartGame()
    {
        SceneManager.LoadScene(startLoad);
        Time.timeScale = 1;
        InGameSceneManager.Instance.gameIsPaused = false;
    }

    void LoadOptions()
    {
        mainMenuButtons.SetActive(false);
        settingsButtons.SetActive(true);
    }

    void UnloadOptions()
    {
        mainMenuButtons.SetActive(true);
        settingsButtons.SetActive(false);
    }

    void ExitGame()
    {
        Application.Quit();
    }
}
