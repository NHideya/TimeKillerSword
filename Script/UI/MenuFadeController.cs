using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFadeController : MonoBehaviour
{
    public static MenuFadeController Instance;
    private FadeUI fadeUI;
    [SerializeField] private float fadeTime;

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
    }
    // Start is called before the first frame update
    void Start()
    {
        fadeUI = GetComponent<FadeUI>();
        fadeUI.FadeUIOut(fadeTime);
    }

    public void CallFadeAndStartGame(string _sceneToLoad)
    {
       SaveData.Instance.InitializeFlagForNewGame();
       StartCoroutine(FadeAndNewGame(_sceneToLoad));
    }
    public void CallFadeTransition(string _sceneToLoad)
    {
     
       StartCoroutine(FadeAndNewGame(_sceneToLoad));
    }
     public void CallFadeAndTitle()
    {
      if(PlayerController.Instance != null)
        SceneManager.MoveGameObjectToScene(PlayerController.Instance.gameObject, SceneManager.GetActiveScene());
        if(UIManager.Instance != null)
        SceneManager.MoveGameObjectToScene(UIManager.Instance.gameObject, SceneManager.GetActiveScene());
       

       GameManager.Instance.UnPauseGame();
       StartCoroutine(FadeAndTitle());
    }

     public void CallFadeAndEnding()
    {
        SceneManager.MoveGameObjectToScene(PlayerController.Instance.gameObject, SceneManager.GetActiveScene());
        SceneManager.MoveGameObjectToScene(UIManager.Instance.gameObject, SceneManager.GetActiveScene());
       

       StartCoroutine(FadeAndEnd());
    }

     public void CallFadeAndLoadGame()
    {
       StartCoroutine(FadeAndLoadGame());
    }

     public void QuitPause()
    {
      GameManager.Instance.UnPauseGame();
    }

    public void Respawn()
    {
      GameManager.Instance.RespawnPlayer();
    }
    IEnumerator FadeAndNewGame(string _sceneToLoad)
    {
      
        fadeUI.FadeUIIn(fadeTime);
        yield return new WaitForSeconds(fadeTime);
      SaveData.Instance.newGame = true;
        SceneManager.LoadScene(_sceneToLoad);
    }
    IEnumerator FadeAndTransition(string _sceneToLoad)
    {
      
        fadeUI.FadeUIIn(fadeTime);
        yield return new WaitForSeconds(fadeTime);
      
        SceneManager.LoadScene(_sceneToLoad);
    }
     IEnumerator FadeAndTitle()
    {
      
        fadeUI.FadeUIIn(fadeTime);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene("Title");
    }
    IEnumerator FadeAndEnd()
    {
      
        fadeUI.FadeUIIn(fadeTime);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene("Ending");
    }
    

     IEnumerator FadeAndLoadGame()
    {
        string _loadSceneName = SaveData.Instance.LoadSceneName();
         if(_loadSceneName == "Not_Exist" || _loadSceneName  == "")
         yield break;
         GameManager.Instance.transitionedFromScene = "";
       fadeUI.FadeUIIn(fadeTime);
        yield return new WaitForSeconds(fadeTime);
        SaveData.Instance.newGame = false;
       
        SceneManager.LoadScene(_loadSceneName);
    }
    
}
