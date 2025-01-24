using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Bench : MonoBehaviour
{
    public bool interacted;
    public bool inRange;
    [SerializeField] FadeUI tutorial;
    [SerializeField] FadeUI done;
    [SerializeField] GameObject keyboardUI;
    [SerializeField] GameObject xboxUI;
    [SerializeField] GameObject psUI;
    [SerializeField] GameObject switchUI;

    private InputDeviceType past;
    

    // Start is called before the first frame update
    private PlayerInput playerInput;
    private InputAction inputAction;
    void Start()
    {
         playerInput = GetComponent<PlayerInput>();
        inputAction = playerInput.actions.FindAction("Interact");
        past = InputDeviceType.Keyboard;
        SwapUI(DeviceManager.Instance.currentDevice);
    }

    // Update is called once per frame
    void Update()
    {
        if (inRange && inputAction.WasReleasedThisFrame())
        {
            interacted = true;

            SaveData.Instance.benchSceneName = SceneManager.GetActiveScene().name;
            SaveData.Instance.benchPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
            SaveData.Instance.SaveBench();
            SaveData.Instance.SavePlayerData();
            PlayerController.Instance.MaxHeal();
            tutorial.FadeUIOut(0.1f);
            StartCoroutine(SaveUI());
        }
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        
        if (_collision.CompareTag("Player"))
        {
            SwapUI(DeviceManager.Instance.currentDevice);
            inRange = true;
            tutorial.FadeUIIn(0.1f);
        }
    }
    private void OnTriggerExit2D(Collider2D _collision)
    {
        if (_collision.CompareTag("Player"))
        {
            inRange = false;
            interacted = false;
            tutorial.FadeUIOut(0.1f);
        }
    }
    IEnumerator SaveUI()
    {
        done.FadeUIIn(0.1f);
        yield return new WaitForSeconds(1f);
        done.FadeUIOut(0.1f);
    }

    private void SwapUI(InputDeviceType _current)
    {
        if(  _current != past)
        {
            switch(past)
            {
                case InputDeviceType.Keyboard :
                    keyboardUI.SetActive(false);
                    break;
                case InputDeviceType.Xbox :
                    xboxUI.SetActive(false);
                    break;
                case InputDeviceType.PS4 :
                    psUI.SetActive(false);
                    break;
                case InputDeviceType.PS5 :
                    psUI.SetActive(false);
                    break;
                case InputDeviceType.Switch :
                    switchUI.SetActive(false);
                    break;
            }
            switch(_current)
            {
                case InputDeviceType.Keyboard :
                    keyboardUI.SetActive(true);
                    break;
                case InputDeviceType.Xbox :
                    xboxUI.SetActive(true);
                    break;
                case InputDeviceType.PS4 :
                    psUI.SetActive(true);
                    break;
                case InputDeviceType.PS5 :
                    psUI.SetActive(true);
                    break;
                case InputDeviceType.Switch :
                    switchUI.SetActive(true);
                    break;
            }
            past = _current;
        }
    }
}
