using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isAttackable : MonoBehaviour
{
    [HideInInspector] public EnemyState eState;
    // Start is called before the first frame update
    void Start()
    {
        eState = GetComponentInParent<EnemyState>();
    }

    private void OnTriggerStay2D(Collider2D _other)
    {
        if (_other.CompareTag("Player")) eState.Attackable = true;
    }

    private void OnTriggerExit2D(Collider2D _other)
    {
        if (_other.CompareTag("Player")) eState.Attackable = false;
    }
}
