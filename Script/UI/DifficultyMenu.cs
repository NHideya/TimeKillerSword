//難易度設定GUIのボタンが押された際の処理を行うクラス

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DifficultyMenu : MonoBehaviour
{

    [SerializeField]
	TextMeshProUGUI text;

    //カジュアルモードのチェックボックスが変更された際に実行
    //難易度に関わるデータの変更を行う    
    public void SetCasualMode(bool _isOn)
    {
        PlayerController.Instance.CasualMode(_isOn);
    }

    //最大ダメージ量のバーが変更された際に実行
    public void SetMaxDamage(float _volume)
    {
        int _intValue;
        _intValue = (int)Mathf.Round(_volume);
        PlayerController.Instance.maxDamage = _intValue;
        text.SetText("{0}", _intValue);

    }
  
}
