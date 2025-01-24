using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionToEnding : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.CompareTag("Player"))
        {
            

            PlayerController.Instance.pState.cutscene = true;
           
            MenuFadeController.Instance.CallFadeAndEnding();

        }
    }
}
