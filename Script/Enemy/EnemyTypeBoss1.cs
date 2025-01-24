using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyTypeBoss1 : Enemy
{
   

    [Header("effect")]

    [SerializeField] Transform effectposition;
    [SerializeField] protected GameObject slash;
    [SerializeField] protected GameObject beforeattack;
    [SerializeField] protected GameObject strongslash;
    [SerializeField] protected GameObject beforestrongattack;
    [Space(5)]
    [Header("Attackset")]
    [SerializeField] protected Transform attackposition1;
    [SerializeField] protected Vector2 AttackArea1;
    [SerializeField] protected GameObject CA1;
    [SerializeField] protected GameObject gate;
    [SerializeField] protected GameObject BossHp;

    

    
    

    protected override void Start()
    {

       
        base.Start();
        rb.gravityScale = 12f;
        //GameObject Canvas = GameObject.Find("Canvas");
        //BossHp = Canvas.transform.Find("BossHPGauge").gameObject;
        GameObject Canvas = GameObject.Find("Canvas");
        BossHp = Canvas.transform.Find("BossHPGauge").gameObject;
        
        BossHp.SetActive(false);
        gate.SetActive(false);
    }
    // Update is called once per frame
    protected override void Update()
    {
       if(eState.stuning) CA1.SetActive(false);
       eState.EncounterPlayer = eState.playerinteritory;
      
        base.Update();
        
       
    }

    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(ground.position, 0.1f);
        Gizmos.DrawWireSphere(wall.position, 0.1f);
        Gizmos.DrawWireCube(attackposition1.position, AttackArea1);
        Gizmos.DrawWireSphere(transform.position,viewRange);
        //Gizmos.DrawWireCube(attack1position.position, attack1Area);
        //Gizmos.DrawWireCube(new Vector2(attack1position.position.x - 1.5f, attack1position.position.y), new Vector2(attack1Area.x - 1f, attack1Area.y));
    }
    

    public override void LookingDirection()
    {
        if (!eState.EncounterPlayer)
        {
            if(rb.velocity.x != 0) eState.lookingRight = rb.velocity.x > 0 ? true : false;
            if (eState.lookingRight)
            {
                transform.localScale = new Vector3(-0.01f, transform.localScale.y, transform.localScale.z);

            }
            else
            {
                transform.localScale = new Vector3(0.01f, transform.localScale.y, transform.localScale.z);
            }
        }
        else
        {
            Vector3 _direction = transform.position - PlayerController.Instance.transform.position;
            if (_direction.x > 0)
            {
                transform.localScale = new Vector3(0.01f, transform.localScale.y, transform.localScale.z);
                eState.lookingRight = false;
            }
            else if (_direction.x < 0)
            {
                transform.localScale = new Vector3(-0.01f, transform.localScale.y, transform.localScale.z);
                eState.lookingRight = true;
            }
        }
    }

    IEnumerator Attack1()
    {
        bool _AttackType = Random.Range(-1f,1f) > 0;
        eState.Attacking = true;
         anim.SetTrigger("Attack");
        anim.SetInteger("AttackType", 0);
        rb.velocity = new Vector2(0, rb.velocity.y);
        audioSource.pitch =2f;
        audioSource.PlayOneShot(beforeattackSound);
        StartCoroutine(SetVFX(beforeattack, effectposition, 500, new Vector3(eState.lookingRight ? -2 : 2, 1, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1)));
        yield return new WaitForSeconds(0.375f);
        audioSource.pitch =2f;
        audioSource.PlayOneShot(beforeattackSound);
        StartCoroutine(SetVFX(beforeattack, effectposition, 500, new Vector3(eState.lookingRight ? -1 : 1, 0.9f, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1)));
        yield return new WaitForSeconds(0.375f);
        audioSource.pitch =2f;
        audioSource.PlayOneShot(beforeattackSound);
        StartCoroutine(SetVFX(beforeattack, effectposition, 500, new Vector3(eState.lookingRight ? -2 : 2, 0.8f, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1)));
        yield return new WaitForSeconds(0.375f);
        audioSource.pitch =2f;
        audioSource.PlayOneShot(beforeattackSound);
        StartCoroutine(SetVFX(_AttackType ? beforeattack : beforestrongattack, effectposition, 500, new Vector3(eState.lookingRight ? -1 : 1, 0.7f, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1)));
        yield return new WaitForSeconds(0.375f);
        LookingDirection();
        yield return new WaitForSeconds(0.02f);
         audioSource.pitch =0.5f;
        audioSource.PlayOneShot(attackSound);
         StartCoroutine(SetVFX(slash, effectposition, 500, new Vector3(0,-0.58f, 0), new Vector3(195, 0, 0), new Vector3(-4.67f, 3, 3)));
        Hit(attackposition1, AttackArea1, new Vector2(1, 0), 5, damage, true);
        yield return new WaitForSeconds(0.375f);
         audioSource.pitch =0.5f;
        audioSource.PlayOneShot(attackSound);
        StartCoroutine(SetVFX(slash, effectposition, 500, new Vector3(0,-0.58f, 0), new Vector3(-15, 0, 0), new Vector3(-4.67f, 3, 3)));
        Hit(attackposition1, AttackArea1, new Vector2(1, 0), 5, damage, true);
        yield return new WaitForSeconds(0.375f);
         audioSource.pitch =0.5f;
        audioSource.PlayOneShot(attackSound);
         StartCoroutine(SetVFX(slash, effectposition, 500, new Vector3(0,-0.58f, 0), new Vector3(195, 0, 0), new Vector3(-4.67f, 3, 3)));
        Hit(attackposition1, AttackArea1, new Vector2(1, 0), 5, damage, true);
        yield return new WaitForSeconds(0.375f);
         audioSource.pitch =0.5f;
        audioSource.PlayOneShot(attackSound);
         StartCoroutine(SetVFX(_AttackType ? slash : strongslash, effectposition, 500, new Vector3(0,-0.58f, 0), new Vector3(-15, 0, 0), new Vector3(-4.67f, 3, 3)));
        Hit(attackposition1, AttackArea1, new Vector2(1, 0), 5, damage, _AttackType);
        yield return new WaitForSeconds(0.375f);
        eState.Attacking = false;
    }

   

    IEnumerator Attack2()
    {
        eState.Attacking = true;
         anim.SetTrigger("Attack");
        anim.SetInteger("AttackType", 1);
        rb.velocity = new Vector2(0, rb.velocity.y);
        audioSource.pitch =2f;
        audioSource.PlayOneShot(beforeattackSound);
        StartCoroutine(SetVFX(beforeattack, effectposition, 500, new Vector3(eState.lookingRight ? -2 : 2, 1, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1)));
        yield return new WaitForSeconds(0.25f);
        audioSource.pitch =2f;
        audioSource.PlayOneShot(beforeattackSound);
        StartCoroutine(SetVFX(beforeattack, effectposition, 500, new Vector3(eState.lookingRight ? -1 : 1, 0.8f, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1)));
        yield return new WaitForSeconds(0.25f);
        audioSource.pitch =2f;
        audioSource.PlayOneShot(beforeattackSound);
        StartCoroutine(SetVFX(beforeattack, effectposition, 500, new Vector3(eState.lookingRight ? -2 : 2, 0.6f, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1)));
        yield return new WaitForSeconds(1f);
        LookingDirection();
        yield return new WaitForSeconds(0.02f);
        audioSource.pitch =0.5f;
        audioSource.PlayOneShot(attackSound);
        StartCoroutine(SetVFX(slash, effectposition, 500, new Vector3(0,-0.58f, 0), new Vector3(195, 0, 0), new Vector3(-4.67f, 3, 3)));
        Hit(attackposition1, AttackArea1, new Vector2(1, 0), 5, damage, true);
        yield return new WaitForSeconds(0.25f);
         audioSource.pitch =0.5f;
        audioSource.PlayOneShot(attackSound);
         StartCoroutine(SetVFX(slash, effectposition, 500, new Vector3(0,-0.58f, 0), new Vector3(-15, 0, 0), new Vector3(-4.67f, 3, 3)));
        Hit(attackposition1, AttackArea1, new Vector2(1, 0), 5, damage, true);
        yield return new WaitForSeconds(0.25f);
         audioSource.pitch =0.5f;
        audioSource.PlayOneShot(attackSound);
         StartCoroutine(SetVFX(slash, effectposition, 500, new Vector3(0,-0.58f, 0), new Vector3(195, 0, 0), new Vector3(-4.67f, 3, 3)));
        Hit(attackposition1, AttackArea1, new Vector2(1, 0), 5, damage, true);
        yield return new WaitForSeconds(1f);
        eState.Attacking = false;

    }

    IEnumerator Attack3()
    {
        eState.Attacking = true;
         anim.SetTrigger("Attack");
        anim.SetInteger("AttackType", 2);
        rb.velocity = new Vector2(0, rb.velocity.y);
       
        yield return new WaitForSeconds(0.375f);
        StartCoroutine(SetVFX(beforeattack, effectposition, 500, new Vector3(eState.lookingRight ? -1 : 1, 0.9f, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1)));
        
        audioSource.pitch =2f;
        audioSource.PlayOneShot(beforeattackSound);
        yield return new WaitForSeconds(0.75f);
        audioSource.pitch =2f;
        audioSource.PlayOneShot(beforeattackSound);
        StartCoroutine(SetVFX(beforeattack, effectposition, 500, new Vector3(eState.lookingRight ? -1 : 1, 0.7f, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1)));
        yield return new WaitForSeconds(0.75f);
        LookingDirection();
        yield return new WaitForSeconds(0.02f);
         audioSource.pitch =0.5f;
        audioSource.PlayOneShot(attackSound);
        rb.velocity = new Vector2(eState.lookingRight ? 80 : -80, rb.velocity.y);
         StartCoroutine(SetVFX(slash, effectposition, 500, new Vector3(0,-0.58f, 0), new Vector3(195, 0, 0), new Vector3(-4.67f, 3, 3)));
        CA1.SetActive(true);
        yield return new WaitForSeconds(0.375f);
        CA1.SetActive(false);
        LookingDirection();
        yield return new WaitForSeconds(0.375f);
         audioSource.pitch =0.5f;
        audioSource.PlayOneShot(attackSound);
        rb.velocity = new Vector2(eState.lookingRight ? 80 : -80, rb.velocity.y);
         StartCoroutine(SetVFX(slash, effectposition, 500, new Vector3(0,-0.58f, 0), new Vector3(-15, 0, 0), new Vector3(-4.67f, 3, 3)));
        CA1.SetActive(true);
        yield return new WaitForSeconds(0.375f);
        CA1.SetActive(false);
        //LookingDirection();
        eState.Attacking = false;
    }
    IEnumerator Attack4()
    {
        bool [] _AttackType;
        _AttackType = new bool[4];
        _AttackType[0] = Random.Range(-1f,1f) > 0;
        _AttackType[1] = Random.Range(-1f,1f) > 0;
        _AttackType[2] = Random.Range(-1f,1f) > 0;
        _AttackType[3] = Random.Range(-1f,1f) > 0;

        eState.Attacking = true;
         anim.SetTrigger("Attack");
        anim.SetInteger("AttackType", 0);
        rb.velocity = new Vector2(0, rb.velocity.y);
        audioSource.pitch =2f;
        audioSource.PlayOneShot(beforeattackSound);
        StartCoroutine(SetVFX(_AttackType[0] ? beforeattack : beforestrongattack, effectposition, 500, new Vector3(eState.lookingRight ? -2 : 2, 1, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1)));
        yield return new WaitForSeconds(0.375f);
        audioSource.pitch =2f;
        audioSource.PlayOneShot(beforeattackSound);
        StartCoroutine(SetVFX(_AttackType[1] ? beforeattack : beforestrongattack, effectposition, 500, new Vector3(eState.lookingRight ? -1 : 1, 0.9f, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1)));
        yield return new WaitForSeconds(0.375f);
        audioSource.pitch =2f;
        audioSource.PlayOneShot(beforeattackSound);
        StartCoroutine(SetVFX(_AttackType[2] ? beforeattack : beforestrongattack, effectposition, 500, new Vector3(eState.lookingRight ? -2 : 2, 0.8f, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1)));
        yield return new WaitForSeconds(0.375f);
        audioSource.pitch =2f;
        audioSource.PlayOneShot(beforeattackSound);
        StartCoroutine(SetVFX(_AttackType[3] ? beforeattack : beforestrongattack, effectposition, 500, new Vector3(eState.lookingRight ? -1 : 1, 0.7f, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1)));
        yield return new WaitForSeconds(0.375f);
        LookingDirection();
        yield return new WaitForSeconds(0.02f);
        audioSource.pitch =0.5f;
        audioSource.PlayOneShot(attackSound);
         StartCoroutine(SetVFX(_AttackType[0] ? slash : strongslash, effectposition, 500, new Vector3(0,-0.58f, 0), new Vector3(195, 0, 0), new Vector3(-4.67f, 3, 3)));
        Hit(attackposition1, AttackArea1, new Vector2(1, 0), 5, damage, _AttackType[0]);
        yield return new WaitForSeconds(0.375f);
         audioSource.pitch =0.5f;
        audioSource.PlayOneShot(attackSound);
        StartCoroutine(SetVFX(_AttackType[1] ? slash : strongslash, effectposition, 500, new Vector3(0,-0.58f, 0), new Vector3(-15, 0, 0), new Vector3(-4.67f, 3, 3)));
        Hit(attackposition1, AttackArea1, new Vector2(1, 0), 5, damage, _AttackType[1]);
        yield return new WaitForSeconds(0.375f);
         audioSource.pitch =0.5f;
        audioSource.PlayOneShot(attackSound);
         StartCoroutine(SetVFX(_AttackType[2] ? slash : strongslash, effectposition, 500, new Vector3(0,-0.58f, 0), new Vector3(195, 0, 0), new Vector3(-4.67f, 3, 3)));
        Hit(attackposition1, AttackArea1, new Vector2(1, 0), 5, damage, _AttackType[2]);
        yield return new WaitForSeconds(0.375f);
         audioSource.pitch =0.5f;
        audioSource.PlayOneShot(attackSound);
         StartCoroutine(SetVFX(_AttackType[3] ? slash : strongslash, effectposition, 500, new Vector3(0,-0.58f, 0), new Vector3(-15, 0, 0), new Vector3(-4.67f, 3, 3)));
        Hit(attackposition1, AttackArea1, new Vector2(1, 0), 5, damage, _AttackType[3]);
        yield return new WaitForSeconds(0.375f);
        eState.Attacking = false;
    }


    IEnumerator RandomMove()
    {
        eState.Attacking = true;
        rb.velocity = new Vector2(0, rb.velocity.y);
        rb.velocity = new Vector2(40f, rb.velocity.y);
        yield return new WaitForSeconds(0.375f);
        rb.velocity = new Vector2(-40f, rb.velocity.y);
        yield return new WaitForSeconds(0.375f);
        rb.velocity = new Vector2(40f, rb.velocity.y);
        yield return new WaitForSeconds(0.375f);
        rb.velocity = new Vector2(-40f, rb.velocity.y);
        yield return new WaitForSeconds(0.375f);
        eState.Attacking = false;

    }
    
    protected override IEnumerator DeathAction()
    {
        isdeath = true;
        transform.DOPunchPosition(new Vector3(0.05f, 1.5f, 0), 0.8f, 300, 0);
        BossHp.SetActive(false);
        eState.Attacking = false;
        eState.stuning = true;
        anim.SetTrigger("Stun");
        rb.velocity = Vector2.zero;
        anim.SetBool("isStun", true);
        PlayerController.Instance.HitStopTime(0.2f,1f,1f);
        //SaveData.Instance.SaveBossBeated();
        SaveData.Instance.bossBeated = true;
        
         yield return new WaitForSeconds(1f);
          BGMController.Instance.SetAudioClip();
         GameObject _deathEffect = Instantiate(
           deathEffect,
            transform.position ,
            Quaternion.identity
        );
        DeviceManager.Instance.StartShakeController(1f,1f,1f);
        PlayerController.Instance.HitStopTime(0.1f,1f,0.5f);
       CameraManager.Instance.StartCamerabounce(0.5f,120);
        _deathEffect.transform.position = new Vector3(_deathEffect.transform.position.x,_deathEffect.transform.position.y -3f,_deathEffect.transform.position.z);
        
        Destroy(Enemyparent);
    }


}
