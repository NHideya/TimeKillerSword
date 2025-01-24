using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour
{

  
    public bool lookingRight;
    public bool invincible = false;

    public bool EncounterPlayer = false;

    public bool chasing;

    public bool Attackable = false;
    public bool Attacking = false;
    public bool stuning = false;

    public bool canwalk;
    public bool canjump;

    public bool playerinteritory= false;
}
