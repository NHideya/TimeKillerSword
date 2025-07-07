//セーブエリア(ベンチ)の処理を行うクラス

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
    
    private PlayerInput playerInput;
    private InputAction inputAction;

    //初期化処理
    void Start()
    {
        //表示するキーのためインプットを取得する
         playerInput = GetComponent<PlayerInput>();
        inputAction = playerInput.actions.FindAction("Interact");
        past = InputDeviceType.Keyboard;
        SwapUI(DeviceManager.Instance.currentDevice);
    }

    //毎フレーム呼び出される
    void Update()
    {
        //プレイヤーがセーブエリア範囲内にいて、インタラクティブボタンを押した際に実行
        if (inRange && inputAction.WasReleasedThisFrame())
        {
            interacted = true;

            //現在のシーン、プレイヤーのステータスと座標を保存
            SaveData.Instance.benchSceneName = SceneManager.GetActiveScene().name;
            SaveData.Instance.benchPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
            SaveData.Instance.SaveBench();
            SaveData.Instance.SavePlayerData();
            PlayerController.Instance.MaxHeal();
            tutorial.FadeUIOut(0.1f);
            StartCoroutine(SaveUI());
        }
    }


    //プレイヤーがセーブエリアに入った際実行
    private void OnTriggerEnter2D(Collider2D _collision)
    {

        if (_collision.CompareTag("Player"))
        {
            //GUIを表示する
            SwapUI(DeviceManager.Instance.currentDevice);
            inRange = true;
            tutorial.FadeUIIn(0.1f);
        }
    }

    //プレイヤーがセーブエリアから出た際実行
    private void OnTriggerExit2D(Collider2D _collision)
    {
        if (_collision.CompareTag("Player"))
        {
            inRange = false;
            interacted = false;
            //GUIを消す
            tutorial.FadeUIOut(0.1f);
        }
    }
    IEnumerator SaveUI()
    {
        done.FadeUIIn(0.1f);
        yield return new WaitForSeconds(1f);
        done.FadeUIOut(0.1f);
    }

    //現在のコントローラーを記録するメソッド
    private void SwapUI(InputDeviceType _current)
    {
        if (_current != past)
        {
            switch (past)
            {
                case InputDeviceType.Keyboard:
                    keyboardUI.SetActive(false);
                    break;
                case InputDeviceType.Xbox:
                    xboxUI.SetActive(false);
                    break;
                case InputDeviceType.PS4:
                    psUI.SetActive(false);
                    break;
                case InputDeviceType.PS5:
                    psUI.SetActive(false);
                    break;
                case InputDeviceType.Switch:
                    switchUI.SetActive(false);
                    break;
            }
            switch (_current)
            {
                case InputDeviceType.Keyboard:
                    keyboardUI.SetActive(true);
                    break;
                case InputDeviceType.Xbox:
                    xboxUI.SetActive(true);
                    break;
                case InputDeviceType.PS4:
                    psUI.SetActive(true);
                    break;
                case InputDeviceType.PS5:
                    psUI.SetActive(true);
                    break;
                case InputDeviceType.Switch:
                    switchUI.SetActive(true);
                    break;
            }
            past = _current;
        }
    }
}
