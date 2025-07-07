//ゲーム中出現するチュートリアルを制御するクラス

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;
   
    //チュートリアル画面を格納するリスト
    [System.Serializable]
   private struct TutorialInfo
   {
    [SerializeField]
    public TutorialType tutorialType;
    [SerializeField]
    public FadeUI fadeUI;
   }

   private InputDeviceType past;


   
  
    [SerializeField] List<GameObject> keyboardInfo = new List<GameObject>();
    [SerializeField] List<GameObject> xboxInfo = new List<GameObject>();
    [SerializeField] List<GameObject> ps4Info = new List<GameObject>();
    [SerializeField] List<GameObject> ps5Info = new List<GameObject>();
    [SerializeField] List<GameObject> switchInfo = new List<GameObject>();
   

   [SerializeField] List<TutorialInfo> list = new List<TutorialInfo>();

   
   [SerializeField] float fadeTime;
   
    //初期化処理
    private void Awake()
    {
        //singleton二設定
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    //初期化処理
    private void Start()
    {
        past = InputDeviceType.Keyboard;
    }
    //リストから指定されたチュートリアルを選択し、表示する処理
    public void FadeInTutorial(TutorialType _tutorialType)
    {
        foreach(TutorialInfo _tutorialInfo in list)
        {
            if(_tutorialInfo.tutorialType == _tutorialType)
            {
                _tutorialInfo.fadeUI.FadeUIIn(fadeTime);
            }
        }
    }
    
    //リストから指定されたチュートリアルを選択し、非表示する処理
     public void FadeOutTutorial(TutorialType _tutorialType)
    {
        foreach (TutorialInfo _tutorialInfo in list)
        {
            if (_tutorialInfo.tutorialType == _tutorialType)
            {
                _tutorialInfo.fadeUI.FadeUIOut(fadeTime);
            }
        }
    }

    //リスト内のすべてのチュートリアル画面を非表示にする
    public void FadeOutTutorialAll()
    {
        foreach (TutorialInfo _tutorialInfo in list)
        {

            _tutorialInfo.fadeUI.FadeUIOut(fadeTime);

        }
    }
    
    //現在の入力デバイスから、コントローラーに対応したキーが表示されるよう有効化を切り替える

    public void ActiveInfo(InputDeviceType _current)
    {
        if (_current == past)
            return;
        switch (past)
        {
            case InputDeviceType.Keyboard:
                foreach (GameObject temp in keyboardInfo)
                {
                    temp.SetActive(false);
                }
                break;
            case InputDeviceType.Xbox:
                foreach (GameObject temp in xboxInfo)
                {
                    temp.SetActive(false);
                }
                break;
            case InputDeviceType.PS4:
                foreach (GameObject temp in ps4Info)
                {
                    temp.SetActive(false);
                }
                break;
            case InputDeviceType.PS5:
                foreach (GameObject temp in ps5Info)
                {
                    temp.SetActive(false);
                }
                break;
            case InputDeviceType.Switch:
                foreach (GameObject temp in switchInfo)
                {
                    temp.SetActive(false);
                }
                break;
        }

        switch (_current)
        {
            case InputDeviceType.Keyboard:
                foreach (GameObject temp in keyboardInfo)
                {
                    temp.SetActive(true);
                }
                break;
            case InputDeviceType.Xbox:
                foreach (GameObject temp in xboxInfo)
                {
                    temp.SetActive(true);
                }
                break;
            case InputDeviceType.PS4:
                foreach (GameObject temp in ps4Info)
                {
                    temp.SetActive(true);
                }
                break;
            case InputDeviceType.PS5:
                foreach (GameObject temp in ps5Info)
                {
                    temp.SetActive(true);
                }
                break;
            case InputDeviceType.Switch:
                foreach (GameObject temp in switchInfo)
                {
                    temp.SetActive(true);
                }
                break;
        }
        past = _current;
    }
}
