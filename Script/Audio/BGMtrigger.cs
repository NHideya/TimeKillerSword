using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMtrigger : MonoBehaviour
{

    [SerializeField] private Enemy enemy;
  private void OnTriggerEnter2D(Collider2D _other)
   {
    if(_other.CompareTag("Player"))
    {
       BGMController.Instance.bossSignal = enemy != null;
        BGMController.Instance.SetAudioClip();
    }
   }

   
}
