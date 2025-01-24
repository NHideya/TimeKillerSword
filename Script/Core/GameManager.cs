using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public string transitionedFromScene;
    public static GameManager Instance { get; private set; }
    public Vector2 platformingRespawnPoint;
    public Vector2 respawnPoint;
    [SerializeField] Bench bench;
    
    [SerializeField] private FadeUI pauseMenu;
     [SerializeField] private FadeUI optionMenu;
      [SerializeField] private FadeUI menuBackGround;
      [SerializeField] private FadeUI difficultyMenu;

      

    [SerializeField] private float fadeTime;
    
    public bool gameisPaused;

    public bool newGame;
    private PlayerInput playerInput;
    private InputAction inputAction;

    private string currentScene;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        SaveData.Instance.Initialize();
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        SaveScene();
        DontDestroyOnLoad(gameObject);
        bench = FindObjectOfType<Bench>();
        playerInput = GetComponent<PlayerInput>();
        inputAction = playerInput.actions.FindAction("Pause");
    }

    private void Start()
    {
        currentScene = "Title";
         SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {


        if( inputAction.WasReleasedThisFrame() && !gameisPaused)
        {
            Pause();
        }
         else if( inputAction.WasReleasedThisFrame() && gameisPaused)
         {
            UnPauseGame();
         }
    }
    public void Pause()
    {
        //Debug.Log(currentScene);
        if(currentScene == "Title" || currentScene == "Ending") return;
        if(pauseMenu == null) pauseMenu = GameObject.Find("Pause Menu").GetComponent<FadeUI>();
        if(menuBackGround == null) menuBackGround = GameObject.Find("Pause Back").GetComponent<FadeUI>();
            pauseMenu.FadeUIIn(fadeTime);
            menuBackGround.FadeUIIn(fadeTime);
            Time.timeScale = 0;
            gameisPaused =true;
    }
    public void UnPauseGame()
    {
        if(pauseMenu == null) pauseMenu = GameObject.Find("Pause Menu").GetComponent<FadeUI>();
         if(optionMenu == null) optionMenu = GameObject.Find("Options Menu").GetComponent<FadeUI>();
          if(menuBackGround == null) menuBackGround = GameObject.Find("Pause Back").GetComponent<FadeUI>();
          if(difficultyMenu == null) optionMenu = GameObject.Find("Difficulty Menu").GetComponent<FadeUI>();
        Time.timeScale =1;
        gameisPaused = false;
        pauseMenu.FadeUIOut(fadeTime);
        optionMenu.FadeUIOut(fadeTime);
        menuBackGround.FadeUIOut(fadeTime);
    }

    public void PauseAndSetTutorial(TutorialType _tutorialType)
    {
        if(currentScene == "Title" || currentScene == "Ending") return;
        TutorialAndPauseManager.Instance.EnableTutorial(_tutorialType);
        if(menuBackGround == null) menuBackGround = GameObject.Find("PauseBack").GetComponent<FadeUI>();
        Time.timeScale = 0;
        gameisPaused =true;
    }

    

    public void SaveScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SaveData.Instance.sceneNames.Add(currentSceneName);
    }

    public void RespawnPlayer()
    {
        
        transitionedFromScene = "";
        SaveData.Instance.LoadBench();
        if (SaveData.Instance.benchSceneName != null)
        {
            SceneManager.LoadScene(SaveData.Instance.benchSceneName);
        }
        if (SaveData.Instance.benchPos != null)
        {
            respawnPoint = SaveData.Instance.benchPos;
        }
        else
        {
            respawnPoint = platformingRespawnPoint;
        }

        PlayerController.Instance.transform.position = respawnPoint;
        StartCoroutine(UIManager.Instance.DeactivateDeathScreen());
        PlayerController.Instance.Respawned();
        
        
        /*
        SceneManager.LoadScene("Area_1");
        PlayerController.Instance.transform.position = new Vector3(-178,0,0);
        PlayerController.Instance.Respawned();
        */
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        currentScene = scene.name;
        Debug.Log(currentScene);
    }
}
