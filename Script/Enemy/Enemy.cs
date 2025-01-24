using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected GameObject Enemyparent;
    [Header("Enemy Setting")]
    [SerializeField] public float maxhealth;
    [SerializeField] protected float recoilLength;
    [SerializeField] protected float recoilFactor;
    [SerializeField] protected bool isRecoiling = false;
    [SerializeField] protected float damage;

    protected float health;
    
    [Header("Move Setting")]

    [SerializeField] protected float speed;

    [SerializeField] protected float runspeed;
    [SerializeField] protected Transform wall;

    [SerializeField] protected Transform ground;

    protected bool canWalk;

    

   


   
   [Space(5)]
   [Header("Serch Setting")]

   [SerializeField] protected float viewRange;

   [SerializeField] protected float viewAngle;

   [SerializeField] protected LayerMask obstacleMask;


    protected Vector3 directionToPlayer; 
    protected float distance;

    [Space(5)]
    [Header("Stun Settitng")]
    [SerializeField] protected float stun;
    [SerializeField] protected float chageAmountOfStun;
    [SerializeField] protected float decreaseAmountOfStun;
    [SerializeField] public float stunTime;

    [SerializeField] private float stundamage;
    public float sinceTimestun;

    [SerializeField] protected GameObject Timer;
    [SerializeField] protected GameObject stunready;

    [Space(5)]
    [Header("Behaviour Tree")]
    [SerializeField] protected BehaviourTree tree;

    [Space(5)]
    
    [HideInInspector] public EnemyState eState;

    [Header("Patrol")]
    [SerializeField] protected Transform pointA;
    [SerializeField] protected Transform pointB;

    [Space(5)]
    [Header("Sound")]
    [SerializeField] protected AudioClip attackSound;
    [SerializeField] protected AudioClip counterSound;
     [SerializeField] protected AudioClip stanSound;
     [SerializeField] protected AudioClip beforeattackSound;
     
      [Space(5)]
    [Header("DeathSetting")]
    [SerializeField] protected GameObject deathEffect;

    protected float recoilTimer;
    public Rigidbody2D rb;
    protected AudioSource audioSource;
    public float TimeSincePatrol;

    public Transform target;

    public bool coroutinesignal;

    public string coroutinename;

    protected float movetime;

    protected Vector2 chagevelocity;

    protected bool isBossEnemy;

    public delegate void ChangeDelegate();

    public ChangeDelegate HPChangeCallBack;

    public ChangeDelegate StanChangeCallBack;

    public ChangeDelegate StanTimerCallBack;

    protected Vector3 defaultposition;
    





    protected Animator anim;

    protected Context context;

    protected bool isdeath;

    // Start is called before the first frame update
    protected virtual void Start()
    {

        context = CreateBehaviourTreeContext();
        context.player = PlayerController.Instance;
       if(pointA  != null) 
       {
            target = pointA;
            context.patroltargetA = pointA;
       }
        if (pointB != null) context.patroltargetB = pointB;
        tree = tree.Clone();
        tree.Bind(context);
        eState = GetComponent<EnemyState>();
        anim = GetComponentInChildren<Animator>();
        coroutinesignal = false;
        Health = maxhealth;
        defaultposition = transform.position;
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        Timer.SetActive(false);
        stunready.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        isdeath = false;
    }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        

    }

    protected virtual void OnDrawGizmos()
    {

    }
    // Update is called once per frame
    protected virtual void Update()
    {
        if(GameManager.Instance.gameisPaused) return;
        if(!PlayerController.Instance.pState.alive)
        {
            return;
        }

        if(!eState.playerinteritory)
        {
            transform.position = defaultposition;
            return;
        }
        if (Health <= 0 && !isdeath)
        {
            //Destroy(Enemyparent);
            StopCoroutine(coroutinename);
            StartCoroutine(DeathAction());
            return;
        }
        if (eState.stuning)
        {
            StunCount();
            return;
        }
        SetCanWalk();
        if (tree)
        {
            tree.Update();
        }
        if(!eState.Attacking)
        {
            LookingDirection();
            SetAnimation();
        }
        
        
        if(coroutinesignal)
        {
            SetCoroutine();
        }
        if (isRecoiling)
        {
            if (recoilTimer < recoilLength)
            {
                recoilTimer += Time.deltaTime;
            }
            else
            {
                isRecoiling = false;
                recoilTimer = 0;
            }
        }
    }

    protected virtual void FixedUpdate()
    {
        ChargeMove();
    }

    protected virtual void SetCoroutine()
    {
        coroutinesignal = false;
        StartCoroutine(coroutinename);
    }

    protected virtual void SetAnimation()
    {
        if(rb.velocity.x != 0)
        {
            anim.SetFloat("Blend",1);
        }
        else
        {
            anim.SetFloat("Blend", 0);
        }
    }

    protected virtual void SetCanWalk()
    {
        eState.canwalk = !Physics2D.OverlapCircle(wall.position, 0.1f, obstacleMask) && Physics2D.OverlapCircle(ground.position, 0.1f, obstacleMask);
        
    }

    virtual public void  LookingDirection()
    {
  
        if(!eState.EncounterPlayer)
        {
            if(eState.lookingRight)
            {
                transform.localScale = new Vector2(1,transform.localScale.y);
            
            }
            else
            {
                transform.localScale = new Vector2(-1, transform.localScale.y);
              
            }
        }
        else
        {
            Vector3 _direction = transform.position - PlayerController.Instance.transform.position;
            if(_direction.x > 0)
            {
                transform.localScale = new Vector2(-1, transform.localScale.y);
                eState.lookingRight = false;
            }
            else if(_direction.x < 0)
            {
                transform.localScale = new Vector2(1, transform.localScale.y);
                eState.lookingRight = true;
            }
        }
    }
    virtual public void EnemyHit(float _damageDone)
    {
        eState.EncounterPlayer = true;
        Health -= _damageDone * (eState.stuning ? stundamage : 1);
        if(!eState.Attacking)
            transform.DOPunchPosition(new Vector3(0.05f, 1.5f, 0), 0.3f, 60, 0);

    }


    protected virtual Context CreateBehaviourTreeContext()
    {
        return Context.CreateFromGameObject(gameObject);
    }


    protected virtual bool IsFound()
    {
        distance = Vector3.Distance(PlayerController.Instance.transform.position,transform.position);
        if(distance <= viewRange)
        {
            directionToPlayer = (PlayerController.Instance.transform.position - transform.position).normalized;
            if(Vector3.Angle(transform.right *(eState.lookingRight ? 1:-1),directionToPlayer) < viewAngle / 2)
            {
                if(!Physics2D.Raycast(transform.position,directionToPlayer,distance,obstacleMask))
                {
                    return true;
                }
            }
        }
        return false;
    }

    protected virtual void ChasePlayer()
    {
        anim.SetBool("Walking",true);
        canWalk = !Physics2D.OverlapCircle(wall.position, 0.3f, obstacleMask) && Physics2D.OverlapCircle(ground.position, 0.2f, obstacleMask);
        if (canWalk) transform.position = Vector3.MoveTowards(transform.position, new Vector3(PlayerController.Instance.transform.position.x, transform.position.y, 0), runspeed * Time.deltaTime);
        //eState.chasing = false;
    }

    protected virtual void Hit(Transform _attackTransform, Vector2 _attackArea,  Vector2 _powerDir,float _recoilPower ,float _damage,bool _parryable)
    {
        Collider2D objectToHit = Physics2D.OverlapBox(_attackTransform.position,_attackArea,0f,playerLayer);
        if(objectToHit && !PlayerController.Instance.pState.invincible)
        {
            PlayerController.Instance.TakeDamege(_damage,eState.lookingRight,_parryable,this);
            
            if(!PlayerController.Instance.pState.counter)
                PlayerController.Instance.rb.velocity = (eState.lookingRight ? _recoilPower : -_recoilPower) * _powerDir.normalized; 
            //PlayerController.Instance.HitStopTime(0.5f,1,0.3f);
        }
       
    }

    virtual public float Health
    {
        get{return health;}
        set
        {
            if(health != value)
            {
                health = Mathf.Clamp(value,0,maxhealth);
                if(HPChangeCallBack != null)
                {
                    HPChangeCallBack.Invoke();
                }
            }
        }
    }

    public float Stun
    {
        get{return stun;}
        set
        {
            if(stun != value)
            {
                stun = Mathf.Clamp(value,0,100);
                if(StanChangeCallBack != null)
                {
                    StanChangeCallBack.Invoke();
                }
            }
        }
    }
    

    virtual public void ChargeStun(bool _isParry)
    {
        Stun += _isParry ? chageAmountOfStun : decreaseAmountOfStun;
        if(stun >= 100) stunready.SetActive(true);
        else stunready.SetActive(false);
    }

    virtual public void toStun()
    {
        if(Stun >= 100)
        {
            audioSource.pitch =1f;
            DeviceManager.Instance.StartShakeController(0.3f,0.5f,0.2f);
            audioSource.PlayOneShot(counterSound);
            eState.Attacking = false;
            anim.SetTrigger("Stun");
            anim.SetBool("isStun",true);
            rb.velocity = Vector2.zero;
           PlayerController.Instance.HitStopTime(0.1f,1f,0.5f);
            StopCoroutine(coroutinename);
            stunready.SetActive(false);
            transform.DOPunchPosition(new Vector3(0.5f,1.5f,0),0.3f,60,0);
            StartCoroutine(standuration());
            
        }
    }

    IEnumerator standuration()
    {
       
        yield return new WaitForSeconds(0.75f);
        audioSource.pitch =1.4f;
        audioSource.PlayOneShot(stanSound);
        DeviceManager.Instance.StartShakeController(0.8f,0.8f,0.5f);
        transform.DOPunchPosition(new Vector3(0.5f, 2f, 0), 0.3f, 30, 0);
        eState.stuning = true;
        Timer.SetActive(true);
        
    }

    
    virtual public void StunCount()
    {
       if(sinceTimestun < stunTime)
       {
            sinceTimestun += Time.deltaTime;
            StanTimerCallBack.Invoke();
            
       }
       else
       {
            sinceTimestun = 0;
            Stun = 0;
            eState.stuning = false;
            anim.SetBool("isStun", false);
            Timer.SetActive(false);
       }
    }

    protected virtual void ChargeMove()
    {
        if(movetime >= 0)
        {
            movetime -= Time.deltaTime;
            rb.velocity = chagevelocity;
        }
    }

    protected virtual void ShotBullet(GameObject _bullet)
    {
        Instantiate(_bullet,transform.position,Quaternion.identity);
    }


     public IEnumerator SetVFX(GameObject _effect, Transform _position, int _order, Vector3 _offset, Vector3 _angle, Vector3 _scale)
    {
        _effect = Instantiate(_effect, _position);
        _effect.transform.eulerAngles = _angle;
        _effect.transform.localScale = _scale;
        _effect.transform.position += _offset;
        Renderer vfxRenderer = _effect.GetComponent<Renderer>();
        vfxRenderer.sortingOrder = _order;
        yield return null;
       

    }

    protected virtual IEnumerator DeathAction()
    {
        yield return null;
    }

}
