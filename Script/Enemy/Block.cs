using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Enemy
{
    [SerializeField] float gaindamage;
    protected override void Start()
    {

       
        Health = maxhealth;
        defaultposition = transform.position;
        rb.gravityScale = 12f;
         eState = GetComponent<EnemyState>();
          Timer.SetActive(false);
        stunready.SetActive(false);
        
        
    }
     protected override void Update()
    {
       
        if (Health <= 0 && !isdeath)
        {
            Destroy(gameObject);
            
        }
        
        
       
    }

    override public void EnemyHit(float _damageDone)
    {
        Health -= Mathf.Clamp(_damageDone - gaindamage,0,100);
       
        

    }
}
