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

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);

        InputSystem.onDeviceChange +=
            (device, change) =>
            {
                switch(change)
                {
                   
            case InputDeviceChange.Removed:
                GameManager.Instance.Pause();
                break;
                }
           
            };
        
    }
    void Start()
    {
       playerInput = GetComponent<PlayerInput>();
       keyboardPress =  playerInput.actions.FindAction("Keyboard");
       xboxPress =  playerInput.actions.FindAction("Xbox");
       ps4Press =  playerInput.actions.FindAction("PS4");
       ps5Press =  playerInput.actions.FindAction("PS5");
       ps4Press =  playerInput.actions.FindAction("Switch");
       currentDevice = InputDeviceType.Keyboard;
       foreach(var device in InputSystem.devices)
       {
        Debug.Log(device);
       }
    }

    void Update()
    {
        Debug.Log(EventSystem.current);
    }

    public void StartShakeController(float _left,float _right,float _time)
    {
        if(coroutine != null)
        StopCoroutine(coroutine);
        coroutine = StartCoroutine(ShakeController(_left,_right,_time));
    }
    
     public void ChangeKeyboard(InputAction.CallbackContext context)
    {
        if(TutorialManager.Instance != null && context.performed)
        {
            Debug.Log("Change Key");
            currentDevice = InputDeviceType.Keyboard;
            TutorialManager.Instance.ActiveInfo(currentDevice);
        }
    }

    public void ChangeXbox(InputAction.CallbackContext context)
    {
        if(TutorialManager.Instance != null && context.performed)
       {
         Debug.Log("Change X");
        currentDevice = InputDeviceType.Xbox;
        TutorialManager.Instance.ActiveInfo(currentDevice);
       }
    }
     public void ChangePS(InputAction.CallbackContext context)
    {
        if(TutorialManager.Instance != null && context.performed)
       {
            if(ps5Press.triggered)
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

     public void ChangeSwitch(InputAction.CallbackContext context)
    {
        if(TutorialManager.Instance != null && context.performed)
       {
        Debug.Log("Change Switch");
        currentDevice = InputDeviceType.Switch;
        TutorialManager.Instance.ActiveInfo(currentDevice);
       }
    }
    

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
