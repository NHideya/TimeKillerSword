using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingMenu : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    // Start is called before the first frame update
    public void SetMasterVolume(float _volume)
    {
        audioMixer.SetFloat("Volume",20f * Mathf.Log(_volume,10));
    }
     public void SetBGMVolume(float _volume)
    {
        audioMixer.SetFloat("BGM",20f * Mathf.Log(_volume,10));
    }
     public void SetSEVolume(float _volume)
    {
        audioMixer.SetFloat("SE",20f * Mathf.Log(_volume,10));
    }
    
    public void SetFullScreen(bool _isFull)
    {
        Screen.fullScreen = _isFull;
    }
    
    public void Quit()
    {
        Application.Quit();
    }
}
