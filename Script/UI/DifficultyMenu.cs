using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DifficultyMenu : MonoBehaviour
{

    [SerializeField]
	TextMeshProUGUI text;
    
    public void SetCasualMode(bool _isOn)
    {
        PlayerController.Instance.CasualMode(_isOn);
    }

    public void SetMaxDamage(float _volume)
    {
        int _intValue;
        _intValue = (int)Mathf.Round(_volume);
        PlayerController.Instance.maxDamage = _intValue;
        text.SetText("{0}",_intValue);

    }
  
}
