//シーン間のBGMの切り替えを処理するクラス

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BGMController : MonoBehaviour
{
    public static BGMController Instance;

    AudioSource audioSource;
    [SerializeField] AudioClip titleBGM;
    [SerializeField] AudioClip mapBGM1;
   [SerializeField] AudioClip mapBGM2;
   [SerializeField] AudioClip bossBGM;
   [SerializeField] float fadeTime;
   public bool bossSignal;
   private Coroutine coroutine;

  

   public enum FadeDirection
    {
        In,
        Out
    }

    //実行時に呼び出し
    //初期化処理
    private void Awake()
    {
        //singletonに設定
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);

    }
    // メンバの初期化
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        bossSignal = false;
        SceneManager.sceneLoaded += OnSceneLoaded;
        StartCoroutine(FadeChangeBGM(titleBGM));
        
      
    }


    //シーン切り替え時に呼び出し
    //現在のシーンに対応したBGMを入れ替える処理を呼び出す

    //TODO: open-closedの原則に反しており、シーンが増えるたびにコードの変更が必要となるため処理の改善が必要
    public void SetAudioClip()
    {


        if (coroutine != null)
            StopCoroutine(coroutine);
        if (bossSignal)
        {
            bossSignal = false;
            coroutine = StartCoroutine(FadeChangeBGM(bossBGM));
        }
        else if (SceneManager.GetActiveScene().name == "Area_6" || SceneManager.GetActiveScene().name == "Area_1")
        {
            coroutine = StartCoroutine(FadeChangeBGM(mapBGM2));
        }
        else if (SceneManager.GetActiveScene().name == "Title")
        {
            coroutine = StartCoroutine(FadeChangeBGM(titleBGM));
        }
        else if (SceneManager.GetActiveScene().name == "Ending")
        {
            coroutine = StartCoroutine(FadeChangeBGM(null));
        }
        else
        {
            coroutine = StartCoroutine(FadeChangeBGM(mapBGM1));
        }
    }

    //シーン切り替えを行うたびに呼ばれるイベント処理
    //BGMの切り替え処理のエントリーポイントとして利用
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetAudioClip();
    }
    
    //現在流れているBGMをフェードアウトし、_audioClipのBGMをフェードインする非同期処理
    public IEnumerator FadeChangeBGM(AudioClip _audioClip)
    {

        //BGMが同じなら非同期処理を終了
        if (audioSource.clip == _audioClip)
        {
            yield break;
        }
        //現在のBGMのボリュームをフェードアウトする処理
        while (audioSource.volume > 0)
        {
            SetVol(FadeDirection.Out);
            yield return null;
        }
        audioSource.volume = 0;
        audioSource.clip = _audioClip;
        audioSource.Play();
         //_audioClipのBGMのボリュームをフェードインする処理
        while (audioSource.volume < 1)
        {
            SetVol(FadeDirection.In);
            yield return null;
        }
        audioSource.volume = 1;
    }

    //イン、アウトの情報を受け取りBGMのボリュームを微小に増加せる
    void SetVol(FadeDirection _fadeDirection)
    {
        audioSource.volume +=  Time.deltaTime * (1/fadeTime) *  (_fadeDirection == FadeDirection.Out ? -1 : 1);
    }
   
}
