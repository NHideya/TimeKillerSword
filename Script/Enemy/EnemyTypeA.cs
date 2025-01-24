using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTypeA : Enemy
{
    [SerializeField] protected Transform attack1position;

    [SerializeField] protected Vector2 attack1Area;
    [Space(5)]
    [Header("effect")]

    [SerializeField] Transform effectposition;
    [SerializeField] protected GameObject slash;
    [SerializeField] protected GameObject beforeattack;
    [Space(5)]
    [Header("Attackset")]
    [SerializeField] protected Transform attackposition1;
    [SerializeField] protected Vector2 AttackArea1;

    [SerializeField] protected GameObject CA1;

    [SerializeField] protected GameObject bullet1;
    protected override void Start()
    {

       
        base.Start();
        rb.gravityScale = 12f;
    }
    // Update is called once per frame
    protected override void Update()
    {
        if (!eState.EncounterPlayer)
        {
            eState.EncounterPlayer = IsFound();
        }

        base.Update();

    }

    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(ground.position, 0.1f);
        Gizmos.DrawWireSphere(wall.position, 0.1f);
        Gizmos.DrawWireCube(attackposition1.position, AttackArea1);
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
                transform.localScale = new Vector3(-0.005f, transform.localScale.y, transform.localScale.z);

            }
            else
            {
                transform.localScale = new Vector3(0.005f, transform.localScale.y, transform.localScale.z);
            }
        }
        else
        {
            Vector3 _direction = transform.position - PlayerController.Instance.transform.position;
            if (_direction.x > 0)
            {
                transform.localScale = new Vector3(0.005f, transform.localScale.y, transform.localScale.z);
                eState.lookingRight = false;
            }
            else if (_direction.x < 0)
            {
                transform.localScale = new Vector3(-0.005f, transform.localScale.y, transform.localScale.z);
                eState.lookingRight = true;
            }
        }
    }

    IEnumerator Attack1()
    {
        StartCoroutine(SetVFX(beforeattack, effectposition, 500, new Vector3(eState.lookingRight ? -2 : 2, 1, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1)));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(SetVFX(slash, effectposition, 500, new Vector3(0, 0.9f, 0), new Vector3(-30, 0, 0), new Vector3(-3, 2, 2)));
        Hit(attackposition1, AttackArea1, new Vector2(1,0),30,10,true);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(SetVFX(slash, effectposition, 500, new Vector3(0, 0.9f, 0), new Vector3(30, 0, 0), new Vector3(-3, 2, 2)));
        Hit(attackposition1, AttackArea1, new Vector2(1, 0), 30, 0,true);
    }

    IEnumerator Attack2()
    {
        movetime = 0.5f;
        chagevelocity = new Vector2(20,0);
        CA1.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        CA1.SetActive(false);
    }

    IEnumerator Attack3()
    {
        ShotBullet(bullet1);
        yield return new WaitForSeconds(0.2f);
        ShotBullet(bullet1);
        yield return new WaitForSeconds(0.2f);
        ShotBullet(bullet1);
        yield return new WaitForSeconds(0.2f);
    }
    IEnumerator Attack4()
    {
        StartCoroutine(SetVFX(beforeattack, effectposition, 500, new Vector3(eState.lookingRight ? -2 : 2, 1, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1)));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(SetVFX(slash, effectposition, 500, new Vector3(0, 0.9f, 0), new Vector3(-30, 0, 0), new Vector3(-3, 2, 2)));
        Hit(attackposition1, AttackArea1, new Vector2(1, 0), 30, 10, false);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(SetVFX(slash, effectposition, 500, new Vector3(0, 0.9f, 0), new Vector3(30, 0, 0), new Vector3(-3, 2, 2)));
        Hit(attackposition1, AttackArea1, new Vector2(1, 0), 30, 0, false);
    }
}
