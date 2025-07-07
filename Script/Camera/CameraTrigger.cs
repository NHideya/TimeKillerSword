//範囲に入った際カメラの切り替え処理を呼び出すクラス

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTrigger : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera newCamera;
    



    //プレイヤーがカメラの範囲をまたいだ時に実行
    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.CompareTag("Player"))
        {
            //カメラ入れ替え実行
            CameraManager.Instance.SwapCamera(newCamera);
        }
    }

    
}
