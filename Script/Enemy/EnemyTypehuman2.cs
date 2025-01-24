using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyTypehuman2 : Enemy
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

    

    protected override void Start()
    {

       
        base.Start();
        rb.gravityScale = 12f;
    }
    // Update is called once per frame
    protected override void Update()
    {
       if(eState.stuning) CA1.SetActive(false);

        base.Update();
        if (!eState.EncounterPlayer)
        {
            eState.EncounterPlayer = IsFound();
        }

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
                transform.localScale = new Vector3(-0.0075f, transform.localScale.y, transform.localScale.z);

            }
            else
            {
                transform.localScale = new Vector3(0.0075f, transform.localScale.y, transform.localScale.z);
            }
        }
        else
        {
            Vector3 _direction = transform.position - PlayerController.Instance.transform.position;
            if (_direction.x > 0)
            {
                transform.localScale = new Vector3(0.0075f, transform.localScale.y, transform.localScale.z);
                eState.lookingRight = false;
            }
            else if (_direction.x < 0)
            {
                transform.localScale = new Vector3(-0.0075f, transform.localScale.y, transform.localScale.z);
                eState.lookingRight = true;
            }
        }
    }
    IEnumerator Attack1()
    {
        anim.SetTrigger("Attack");
        anim.SetInteger("AttackType", 0);
        anim.SetFloat("Blend",0);
        rb.velocity = new Vector2(0,rb.velocity.y);
        eState.Attacking = true;
        StartCoroutine(SetVFX(beforestrongattack, effectposition, 500, new Vector3(eState.lookingRight ? -2 : 2, 1, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1)));
        audioSource.pitch =2f;
        audioSource.PlayOneShot(beforeattackSound);
        yield return new WaitForSeconds(1f);
        audioSource.pitch =1f;
        audioSource.PlayOneShot(attackSound);
        rb.velocity = new Vector2(eState.lookingRight ? 50 : -50, rb.velocity.y);
        StartCoroutine(SetVFX(strongslash, effectposition, 500, new Vector3(eState.lookingRight ? 2.4f : -2.4f, -0.18f, 0), new Vector3(-15, 0, 0), new Vector3(-1.5f, 1.5f, 1.5f)));
        Hit(attackposition1, AttackArea1, new Vector2(1, 0), 30, 10, false);
        yield return new WaitForSeconds(0.5f);
        eState.Attacking = false;

    }

    IEnumerator Attack2()
    {
        anim.SetTrigger("Attack");
        anim.SetInteger("AttackType", 1);
        anim.SetFloat("Blend",0);
        rb.velocity = new Vector2(0, rb.velocity.y);
        eState.Attacking = true;
        audioSource.pitch =2f;
        audioSource.PlayOneShot(beforeattackSound);
        StartCoroutine(SetVFX(beforeattack, effectposition, 500, new Vector3(eState.lookingRight ? -2 : 2, 1, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1)));
        yield return new WaitForSeconds(0.5f);
       
        audioSource.pitch =0.8f;
        audioSource.PlayOneShot(attackSound);
        rb.velocity = new Vector2(eState.lookingRight ? 80 : -80, rb.velocity.y);
        StartCoroutine(SetVFX(slash, effectposition, -1, new Vector3(eState.lookingRight ? 1.27f : -1.27f, 1.43f, 0), Vector3.zero, new Vector3(-1.5f, 1.5f, 1.5f)));
        CA1.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        CA1.SetActive(false);
        LookingDirection();
        yield return new WaitForSeconds(0.333f);
        audioSource.pitch =1f;
        audioSource.PlayOneShot(attackSound);
        rb.velocity = new Vector2(eState.lookingRight ? 80 : -80, rb.velocity.y);
        StartCoroutine(SetVFX(slash, effectposition, 500, new Vector3(eState.lookingRight ? 1.53f : -1.53f, -0.38f, 0), Vector3.zero, new Vector3(-1.5f, 1.5f, 1.5f)));
        CA1.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        CA1.SetActive(false);
        eState.Attacking = false;
    }

     IEnumerator Attack3()
    {
        anim.SetTrigger("Attack");
        anim.SetInteger("AttackType", 2);
        anim.SetFloat("Blend",0);
        rb.velocity = new Vector2(0,rb.velocity.y);
        eState.Attacking = true;
        audioSource.pitch =2f;
        audioSource.PlayOneShot(beforeattackSound);
        StartCoroutine(SetVFX(beforeattack, effectposition, 500, new Vector3(eState.lookingRight ? -2 : 2, 1, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1)));
        yield return new WaitForSeconds(0.5f);
        audioSource.pitch =1f;
        audioSource.PlayOneShot(attackSound);
        rb.velocity = new Vector2(eState.lookingRight ? 50 : -50, rb.velocity.y);
        StartCoroutine(SetVFX(slash, effectposition, -1, new Vector3(eState.lookingRight ? 1.27f : -1.27f, 1.43f, 0), Vector3.zero, new Vector3(-1.5f, 1.5f, 1.5f)));
        Hit(attackposition1, AttackArea1, new Vector2(1, 0), 30, 10, true);
        yield return new WaitForSeconds(0.5833f);
        audioSource.pitch =1f;
        audioSource.PlayOneShot(attackSound);
        rb.velocity = new Vector2(eState.lookingRight ? 50 : -50, rb.velocity.y);
        StartCoroutine(SetVFX(slash, effectposition, 500, new Vector3(eState.lookingRight ? 1.53f : -1.53f, -0.38f, 0), Vector3.zero, new Vector3(-1.5f, 1.5f, 1.5f)));
        Hit(attackposition1, AttackArea1, new Vector2(1, 0), 40, 10, true);
        yield return new WaitForSeconds(0.5f);

        eState.Attacking = false;
    }

   

   
    IEnumerator RandomMove()
    {
        anim.SetTrigger("Attack");
        anim.SetInteger("AttackType", 3);
        rb.velocity = new Vector2(0, rb.velocity.y);
        transform.DOLocalMoveX(Random.Range(-1f,1f) > 0 ? 1f: -1f,0.5f).SetRelative().SetEase(Ease.InOutQuart);
        yield return new WaitForSeconds(0.45f);
        
    }
    protected override IEnumerator DeathAction()
    {
        isdeath = true;
        eState.Attacking = false;
        eState.stuning = true;
        anim.SetTrigger("Stun");
        rb.velocity = Vector2.zero;
        anim.SetBool("isStun", true);
         GameObject _deathEffect = Instantiate(
           deathEffect,
            transform.position ,
            Quaternion.identity
        );
         _deathEffect.transform.localScale = 4 *  _deathEffect.transform.localScale;
        _deathEffect.transform.position = new Vector3(_deathEffect.transform.position.x,_deathEffect.transform.position.y,_deathEffect.transform.position.z);
        Destroy(Enemyparent);
        yield return null;
    }

}
