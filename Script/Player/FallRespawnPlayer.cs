using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallRespawnPlayer : MonoBehaviour
{
    // Start is called before the first frame update
   private void OnCollisionEnter2D(Collision2D _other)
   {
     if (_other.gameObject.CompareTag("Player"))
     {
        Debug.Log("HIT");
        StartCoroutine(PlayerController.Instance.FallRespawn());
     }
   }

}
