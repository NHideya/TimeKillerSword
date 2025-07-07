//敵キャラクターの抽象クラス

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;



public class Enemy : MonoBehaviour
{
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected GameObject Enemyparent;
    [Header("敵パラメーター")]
    [SerializeField] public float maxhealth;
    [SerializeField] protected float recoilLength;
    [SerializeField] protected float recoilFactor;
    [SerializeField] protected bool isRecoiling = false;
    [SerializeField] protected float damage;

    protected float health;
    
    [Header("移動用設定")]

    [SerializeField] protected float speed;

    [SerializeField] protected float runspeed;
    [SerializeField] protected Transform wall;

    [SerializeField] protected Transform ground;

    protected bool canWalk;

    

   


   
   [Space(5)]
   [Header("敵の視野の設定")]

   [SerializeField] protected float viewRange;

   [SerializeField] protected float viewAngle;

   [SerializeField] protected LayerMask obstacleMask;


    protected Vector3 directionToPlayer; 
    protected float distance;

    [Space(5)]
    [Header("気絶用設定")]
    [SerializeField] protected float stun;
    [SerializeField] protected float chageAmountOfStun;
    [SerializeField] protected float decreaseAmountOfStun;
    [SerializeField] public float stunTime;

    [SerializeField] private float stundamage;
    public float sinceTimestun;

    [SerializeField] protected GameObject Timer;
    [SerializeField] protected GameObject stunready;

    [Space(5)]
    [Header("敵AI")]
    [SerializeField] protected BehaviourTree tree;

    [Space(5)]
    
    [HideInInspector] public EnemyState eState;

    [Header("探索範囲")]
    [SerializeField] protected Transform pointA;
    [SerializeField] protected Transform pointB;

    [Space(5)]
    [Header("サウンドエフェクト")]
    [SerializeField] protected AudioClip attackSound;
    [SerializeField] protected AudioClip counterSound;
     [SerializeField] protected AudioClip stanSound;
     [SerializeField] protected AudioClip beforeattackSound;
     
      [Space(5)]
    [Header("死亡時エフェクト")]
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

    //オブジェクトが有効化された際実行
    //初期化処理
    protected virtual void Start()
    {

        context = CreateBehaviourTreeContext();
        context.player = PlayerController.Instance;
        if (pointA != null)
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
    // 毎フレーム実行
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

        //AIが実装されている場合、AIを制御するBehaviourTreeを更新する
        if (tree)
        {
            tree.Update();
        }
        if(!eState.Attacking)
        {
            LookingDirection();
            SetAnimation();
        }

        //BehaviourTreeから信号が送られた時に実行
        //主に攻撃等のアクションを行う
        if (coroutinesignal)
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

    //BehaviourTreeから呼び出される
    //指定されたアクション(coroutinename)を実行する
    protected virtual void SetCoroutine()
    {
        coroutinesignal = false;
        StartCoroutine(coroutinename);
    }

    //移動速度に応じて歩行アニメーションと待機アニメーションを切り替える
    protected virtual void SetAnimation()
    {
        if (rb.velocity.x != 0)
        {
            anim.SetFloat("Blend", 1);
        }
        else
        {
            anim.SetFloat("Blend", 0);
        }
    }

    //移動先に地面があり、なおかつ壁がない場合移動可能を返す
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

    //被ダメージ時呼び出し
    //ダメージ分体力を減らす
    virtual public void EnemyHit(float _damageDone)
    {
        eState.EncounterPlayer = true;
        Health -= _damageDone * (eState.stuning ? stundamage : 1);
        if (!eState.Attacking)
            transform.DOPunchPosition(new Vector3(0.05f, 1.5f, 0), 0.3f, 60, 0);

    }

    //BehaviourTree(AI)が利用するデータを渡す
    protected virtual Context CreateBehaviourTreeContext()
    {
        return Context.CreateFromGameObject(gameObject);
    }

    //プレイヤーを目視したか否かを判定する
    protected virtual bool IsFound()
    {

        //プレイヤーが半径distanceの範囲にいるか判定
        distance = Vector3.Distance(PlayerController.Instance.transform.position,transform.position);
        if(distance <= viewRange)
        {
            directionToPlayer = (PlayerController.Instance.transform.position - transform.position).normalized;
            
            //プレイヤーが敵の視野(viewAngle)の範囲にいるか判定
            if (Vector3.Angle(transform.right * (eState.lookingRight ? 1 : -1), directionToPlayer) < viewAngle / 2)
            {
                //プレイヤーと敵の間に目視を妨げる障害物があるかをレイを飛ばし判定
                if (!Physics2D.Raycast(transform.position, directionToPlayer, distance, obstacleMask))
                {

                    //すべての条件を満たしたときのみ目視したと判定
                    return true;
                }
            }
        }
        return false;
    }
   
   //歩行可能か判断し、可能ならプレイヤーの方向へ移動する 
    protected virtual void ChasePlayer()
    {
        anim.SetBool("Walking", true);
        canWalk = !Physics2D.OverlapCircle(wall.position, 0.3f, obstacleMask) && Physics2D.OverlapCircle(ground.position, 0.2f, obstacleMask);
        if (canWalk) transform.position = Vector3.MoveTowards(transform.position, new Vector3(PlayerController.Instance.transform.position.x, transform.position.y, 0), runspeed * Time.deltaTime);
        //eState.chasing = false;
    }

    //攻撃判定の判定を生成し、判定内にいるプレイヤーにダメージを与える
    protected virtual void Hit(Transform _attackTransform, Vector2 _attackArea, Vector2 _powerDir, float _recoilPower, float _damage, bool _parryable)
    {
        //長方形の攻撃判定を生成
        //プレイヤーの
        Collider2D objectToHit = Physics2D.OverlapBox(_attackTransform.position, _attackArea, 0f, playerLayer);
        if (objectToHit && !PlayerController.Instance.pState.invincible)
        {
            PlayerController.Instance.TakeDamege(_damage, eState.lookingRight, _parryable, this);

            if (!PlayerController.Instance.pState.counter)
                PlayerController.Instance.rb.velocity = (eState.lookingRight ? _recoilPower : -_recoilPower) * _powerDir.normalized;
            //PlayerController.Instance.HitStopTime(0.5f,1,0.3f);
        }

    }


    //体力を管理するプロパティ

    //TODO: GUIと相互依存しているためMV(R)Pパターンで解消する必要がある

    virtual public float Health
    {
        get { return health; }
        set
        {
            if (health != value)
            {
                health = Mathf.Clamp(value, 0, maxhealth);
                //体力の増減をGUIの表示と連動させる処理

                //相互依存箇所
                if (HPChangeCallBack != null)
                {
                    HPChangeCallBack.Invoke();
                }
                //
            }
        }
    }

    //気絶ゲージを管理するプロパティ

    //TODO: GUIと相互依存しているためMV(R)Pパターンで解消する必要がある
    public float Stun
    {
        get { return stun; }
        set
        {
            if (stun != value)
            {
                stun = Mathf.Clamp(value, 0, 100);
                //気絶ゲージの増減をGUIの表示と連動させる処理

                //相互依存箇所
                if (StanChangeCallBack != null)
                {
                    StanChangeCallBack.Invoke();
                }
                //
            }
        }
    }

    //プレイヤーがジャストガードした際呼び出される
    //敵の気絶ゲージを増加させる
    virtual public void ChargeStun(bool _isParry)
    {
        Stun += _isParry ? chageAmountOfStun : decreaseAmountOfStun;
        if (stun >= 100) stunready.SetActive(true);
        else stunready.SetActive(false);
    }


    //プレイヤーがカウンターした時に実行
    //気絶ゲージが最大なら、エフェクトを発生させ気絶させる一連の処理
    virtual public void toStun()
    {
        if (Stun >= 100)
        {
            audioSource.pitch = 1f;
            DeviceManager.Instance.StartShakeController(0.3f, 0.5f, 0.2f);
            audioSource.PlayOneShot(counterSound);
            eState.Attacking = false;
            anim.SetTrigger("Stun");
            anim.SetBool("isStun", true);
            rb.velocity = Vector2.zero;
            PlayerController.Instance.HitStopTime(0.1f, 1f, 0.5f);
            StopCoroutine(coroutinename);
            stunready.SetActive(false);
            transform.DOPunchPosition(new Vector3(0.5f, 1.5f, 0), 0.3f, 60, 0);
            StartCoroutine(standuration());

        }
    }

    //気絶状態をアニメーションに合わせるためのディレイ処理

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

    //敵が横移動に高速移動するメソッド
    //攻撃時に利用

    protected virtual void ChargeMove()
    {
        if (movetime >= 0)
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
