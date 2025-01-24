using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttackLayer : MonoBehaviour
{
    [HideInInspector] public EnemyState eState;
    [HideInInspector] public Enemy enemy;

    [SerializeField] float recoilPower;
    [SerializeField] Vector2 powerDir;
    [SerializeField] float damage;
    [SerializeField] bool parryable;
    // Start is called before the first frame update
    void Start()
    {
        eState = GetComponentInParent<EnemyState>();
        enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.CompareTag("Player") && !PlayerController.Instance.pState.invincible)
        {

            PlayerController.Instance.TakeDamege(damage, eState.lookingRight,parryable, enemy);
            if (!PlayerController.Instance.pState.counter)
                PlayerController.Instance.rb.velocity = (eState.lookingRight ? recoilPower : -recoilPower) * powerDir.normalized;
            gameObject.SetActive(false);
        }
    }
}
