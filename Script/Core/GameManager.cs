//プレイヤーの再配置、シーンの管理を行うクラス

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



    //初期化処理
    private void Awake()
    {   
        //fpsの上限を60に固定
        Application.targetFrameRate = 60;
        SaveData.Instance.Initialize();
        //singletonを設定
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

    //初期化処理
    private void Start()
    {
        currentScene = "Title";
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {

        //一時停止ボタンを押したとき、一時停止の実行を解除を行う
        if( inputAction.WasReleasedThisFrame() && !gameisPaused)
        {
            Pause();
        }
         else if( inputAction.WasReleasedThisFrame() && gameisPaused)
         {
            UnPauseGame();
         }
    }

    //ゲームを一時停止し、ポーズ画面を表示する
    //TODO: GUIと相互依存関係のためMV(R)Pパターンで解消する必要がある
    public void Pause()
    {
        //Debug.Log(currentScene);
        //タイトル画面かエンド画面奈良処理終了
        if (currentScene == "Title" || currentScene == "Ending") return;
        //相互依存箇所
        if (pauseMenu == null) pauseMenu = GameObject.Find("Pause Menu").GetComponent<FadeUI>();
        if (menuBackGround == null) menuBackGround = GameObject.Find("Pause Back").GetComponent<FadeUI>();
        pauseMenu.FadeUIIn(fadeTime);
        menuBackGround.FadeUIIn(fadeTime);
        //
        //ゲーム内時間を停止させる
        Time.timeScale = 0;
        gameisPaused = true;
    }

    //ゲームを一時停止を解除し、ポーズ画面を非表示する
    //TODO: GUIと相互依存関係のためMV(R)Pパターンで解消する必要がある
    public void UnPauseGame()
    {
        //相互依存箇所
        if (pauseMenu == null) pauseMenu = GameObject.Find("Pause Menu").GetComponent<FadeUI>();
        if (optionMenu == null) optionMenu = GameObject.Find("Options Menu").GetComponent<FadeUI>();
        if (menuBackGround == null) menuBackGround = GameObject.Find("Pause Back").GetComponent<FadeUI>();
        if (difficultyMenu == null) optionMenu = GameObject.Find("Difficulty Menu").GetComponent<FadeUI>();
        //
        //ゲーム内時間を戻す
        Time.timeScale = 1;
        gameisPaused = false;
        //相互依存箇所
        pauseMenu.FadeUIOut(fadeTime);
        optionMenu.FadeUIOut(fadeTime);
        menuBackGround.FadeUIOut(fadeTime);
        //
    }

    
    //ゲームを一時停止させチュートリアルを表示させる処理
    public void PauseAndSetTutorial(TutorialType _tutorialType)
    {
        if (currentScene == "Title" || currentScene == "Ending") return;
        TutorialAndPauseManager.Instance.EnableTutorial(_tutorialType);
        if (menuBackGround == null) menuBackGround = GameObject.Find("PauseBack").GetComponent<FadeUI>();
        Time.timeScale = 0;
        gameisPaused = true;
    }

    //Savedataに現在のシーン名を送る処理

    public void SaveScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SaveData.Instance.sceneNames.Add(currentSceneName);
    }


    //ゲームオーバー時、リトライを押すと実行
    //セーブデータを読み、最後にセーブした地点にプレイヤーを再配置する

    //TODO: GUIと相互依存関係のためMV(R)Pパターンで解消する必要がある
    public void RespawnPlayer()
    {

        transitionedFromScene = "";
        //セーブデータを読み込む
        SaveData.Instance.LoadBench();
        //セーブポイントが記録されてるなら、最後に利用したセーブデータを呼び出す
        if (SaveData.Instance.benchSceneName != null)
        {
            SceneManager.LoadScene(SaveData.Instance.benchSceneName);
        }
        //セーブポイントの座標が記録されているなら、座標を呼び出す
        if (SaveData.Instance.benchPos != null)
        {
            respawnPoint = SaveData.Instance.benchPos;
        }
        //座標が存在しない場合シーンの出入り口をリスポーンポイントにする
        else
        {
            respawnPoint = platformingRespawnPoint;
        }

        //プレイヤーを再配置する
        PlayerController.Instance.transform.position = respawnPoint;
        //相互依存箇所
        StartCoroutine(UIManager.Instance.DeactivateDeathScreen());
        //
        PlayerController.Instance.Respawned();


        /*
        SceneManager.LoadScene("Area_1");
        PlayerController.Instance.transform.position = new Vector3(-178,0,0);
        PlayerController.Instance.Respawned();
        */
    }
    //シーン読み込みのたび実行
    //現在のシーン名を記録する
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        currentScene = scene.name;
        Debug.Log(currentScene);
    }
}
