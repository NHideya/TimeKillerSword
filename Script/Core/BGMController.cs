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

     private void Awake()
    {
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
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        bossSignal = false;
        SceneManager.sceneLoaded += OnSceneLoaded;
        StartCoroutine(FadeChangeBGM(titleBGM));
        
      
    }

    public void SetAudioClip()
    {
        
      
       if(coroutine != null)
        StopCoroutine(coroutine);
        if(bossSignal)
        {
            bossSignal = false;
            coroutine = StartCoroutine(FadeChangeBGM(bossBGM));
        }
        else if(SceneManager.GetActiveScene().name == "Area_6" || SceneManager.GetActiveScene().name == "Area_1")
        {
             coroutine =StartCoroutine(FadeChangeBGM(mapBGM2));
        }
        else if(SceneManager.GetActiveScene().name == "Title")
        {
              coroutine =StartCoroutine(FadeChangeBGM(titleBGM));
        }
        else if(SceneManager.GetActiveScene().name == "Ending")
        {
             coroutine =StartCoroutine(FadeChangeBGM(null));
        }
        else
        {
             coroutine =StartCoroutine(FadeChangeBGM(mapBGM1));
        }
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetAudioClip();
    }
    public IEnumerator FadeChangeBGM(AudioClip _audioClip)
    {
        
        if(audioSource.clip == _audioClip)
        {
            yield break;
        }
        //float _currentvol = audioSource.volume;
        while(audioSource.volume > 0)
        {
            SetVol(FadeDirection.Out);
            yield return null;
        }
        audioSource.volume = 0;
        audioSource.clip = _audioClip;
        audioSource.Play();
        while(audioSource.volume < 1)
        {
            SetVol(FadeDirection.In);
            yield return null;
        }
        audioSource.volume = 1; 
    }
    void SetVol(FadeDirection _fadeDirection)
    {
        audioSource.volume +=  Time.deltaTime * (1/fadeTime) *  (_fadeDirection == FadeDirection.Out ? -1 : 1);
    }
   
}
