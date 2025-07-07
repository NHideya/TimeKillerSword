using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class DeviceManager : MonoBehaviour
{
    public static DeviceManager Instance;

   
    private PlayerInput playerInput;

    private InputAction keyboardPress;
    private InputAction xboxPress;
    private InputAction ps4Press;
    private InputAction ps5Press;
    private InputAction switchPress;

    [HideInInspector] public InputDeviceType currentDevice;

    

    private Coroutine coroutine;

    
    //初期化処理
    private void Awake()
    {
        //インスタンスをsingletonに設定
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);


        //デバイスの抜き差しで呼び出すメソッドの登録を行う
        //デバイスが抜かれたときにゲームを一時停止する
        InputSystem.onDeviceChange +=
            (device, change) =>
            {
                switch (change)
                {

                    case InputDeviceChange.Removed:
                        GameManager.Instance.Pause();
                        break;
                }

            };

    }

    //初期化処理
    void Start()
    {
       playerInput = GetComponent<PlayerInput>();
       keyboardPress =  playerInput.actions.FindAction("Keyboard");
       xboxPress =  playerInput.actions.FindAction("Xbox");
       ps4Press =  playerInput.actions.FindAction("PS4");
       ps5Press =  playerInput.actions.FindAction("PS5");
       ps4Press =  playerInput.actions.FindAction("Switch");
       currentDevice = InputDeviceType.Keyboard;
       
       //起動時に現在利用可能な入力デバイスを全てコンソールへ表示
       foreach (var device in InputSystem.devices)
        {
            Debug.Log(device);
        }
    }

    void Update()
    {
        Debug.Log(EventSystem.current);
    }

    //コントローラーの振動を開始する処理
    //振動量は_reft(低周波)_right(高周波)により決められる
    //_timeはコントローラーが振動する時間を決める
    public void StartShakeController(float _left, float _right, float _time)
    {
        //すでに振動していたら振動を停止させる
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(ShakeController(_left, _right, _time));
    }

    //キーボードの任意のキーを押した時に実行
    //GUIの表示の変更を呼び出す
     public void ChangeKeyboard(InputAction.CallbackContext context)
    {
        if (TutorialManager.Instance != null && context.performed)
        {
            Debug.Log("Change Key");
            currentDevice = InputDeviceType.Keyboard;
            TutorialManager.Instance.ActiveInfo(currentDevice);
        }
    }

    //XBOXおコントローラーの任意のキーを押した時に実行
    //GUIの表示の変更を呼び出す
    public void ChangeXbox(InputAction.CallbackContext context)
    {
        if (TutorialManager.Instance != null && context.performed)
        {
            Debug.Log("Change X");
            currentDevice = InputDeviceType.Xbox;
            TutorialManager.Instance.ActiveInfo(currentDevice);
        }
    }
    
    //DualShockの任意のキーを押した時に実行
    //GUIの表示の変更を呼び出す
     public void ChangePS(InputAction.CallbackContext context)
    {
        if (TutorialManager.Instance != null && context.performed)
        {
            if (ps5Press.triggered)
            {
                Debug.Log("PS5");
                currentDevice = InputDeviceType.PS5;
                TutorialManager.Instance.ActiveInfo(currentDevice);
            }
            else
            {
                Debug.Log("PS4");
                currentDevice = InputDeviceType.PS4;
                TutorialManager.Instance.ActiveInfo(currentDevice);
            }
        }
    }

    //Switchコントローラーの任意のキーを押した時に実行
    //GUIの表示の変更を呼び出す
     public void ChangeSwitch(InputAction.CallbackContext context)
    {
        if (TutorialManager.Instance != null && context.performed)
        {
            Debug.Log("Change Switch");
            currentDevice = InputDeviceType.Switch;
            TutorialManager.Instance.ActiveInfo(currentDevice);
        }
    }

    //コントローラーを指定の強さ(_left,_right)で_time秒振動させる非同期処理    
    IEnumerator ShakeController(float _left,float _right,float _time)
    {
        Gamepad gamepad = Gamepad.current;
        if(gamepad == null)
        {
            yield break;
        }
        gamepad.SetMotorSpeeds(_left,_right);
        yield return new WaitForSeconds(_time);
        gamepad.SetMotorSpeeds(0,0);
    }
    
}
