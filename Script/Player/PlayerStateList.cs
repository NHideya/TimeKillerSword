using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateList : MonoBehaviour
{
    public bool jumping = false;
    public bool dashing = false;
    public bool recoilingX;
    public bool recoilingY;
    public bool lookingRight;

    public bool invincible;
    public bool healing;
    public bool casting;
    public bool parry = false;
    public bool graze = false;
    public bool alive;
    public bool cutscene = false;

    public bool damaged = false;

    public bool chargeAttacking = false;
    public bool chargeComboEnable = false;
    public bool counter = false;
}
