using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletvelocity;
    [SerializeField] float damage;

    [SerializeField] float lifetime;

    [SerializeField] bool parryable;
    private Vector2 direct;

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        direct = new Vector2(PlayerController.Instance.transform.position.x, PlayerController.Instance.transform.position.y) - new Vector2(transform.position.x , transform.position.y);
        direct = direct.normalized;
        rb.velocity = direct * bulletvelocity;
        Destroy(gameObject,lifetime);
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.CompareTag("Player") && !PlayerController.Instance.pState.invincible)
        {

            PlayerController.Instance.TakeDamege(damage, rb.velocity.x > 0 ? true : false,parryable,null);
            Destroy(gameObject);
        }
        if(_other.CompareTag("Ground")) Destroy(gameObject);
        
    }


}
