using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;
using DG.Tweening;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    [Header("Horizontal Movement Settings")]
    [SerializeField]
    private float walkspeed = 1;

    [SerializeField] private float durationSound;
    private float sinceSound;

    [Space(5)]
    [Header("Vertical Movement Setting")]
    [SerializeField]
    private float jumpForce = 45;
    private int jumpBufferCounter = 0;

    [SerializeField]
    private int jumpBufferFrames;
    private float coyoteTimeCounter = 0;

    [SerializeField]
    private float coyoteTime;
    private int airJumpCounter = 0;

    [SerializeField]
    private int maxAirJumps;

    

    [Space(5)]
    
    [Header("Ground Cheak Settings")]
    [SerializeField]
    private Transform groundCheakPoint;

    [SerializeField]
    private Transform checker;

    [SerializeField]
    private float groundCheakX = 0.01f;

    [SerializeField]
    private float groundCheakY = 0.2f;

    [SerializeField]
    private LayerMask whatIsGround;

    // Start is called before the first frame update
    [Space(5)]
    [Header("Dash Setting")]
    [SerializeField]
    private float dashSpeed;

    [SerializeField]
    private float dashTime;

    [SerializeField]
    private float dashCooldown;

    [SerializeField]
    GameObject dashEffect;
    [SerializeField]
    Transform effecttransform;

    [Space(5)]
    [Header("Attacking")]
    private bool attack = false;

    [SerializeField]
    private float timeBetweenAttack;

    [SerializeField]
    private float comboResetTime;
    public float timeSinceAttack;

    [SerializeField]
    Transform SideAttackTransform;

    [SerializeField]
    Vector2 SideAttackArea;

    [SerializeField]
    LayerMask attackableLayer;

    [SerializeField]
    float damage;

    [SerializeField]
    GameObject slashEffect;
    [SerializeField]
    GameObject stabEffect;
    [SerializeField]
    GameObject impactEffect;

    [SerializeField]
    GameObject hitEffect;

    private bool restoreTime;

    private float restoreTimeSpeed;

    private AttackState currentAttack;

    public bool Attacking;

    public bool comboStack;

    [Space(5)]
    [Header("Charge Attack Setting")]
    [SerializeField]
    float maxChargeTime;

    [SerializeField]
    float chargeTime;

    [SerializeField]
    private GameObject chargeEffect;
    bool ischarging = false;
    private bool chargeAttack = false;
    private bool charging;

    

    [Space(5)]
    /*
    [Header("Recoil")]
    [SerializeField]
    int recoilXSteps = 5;

    [SerializeField]
    int recoilYSteps = 5;

    [SerializeField]
    public float recoilXspeed = 30;

    [SerializeField]
    public float recoilYspeed = 5;
    int stepsXRecoiled,
        stepsYRecoiled;

    [Space(5)]
    */
    [Header("Health Setting")]
    [SerializeField]
    float health;

    [SerializeField]
    public float maxHealth;

    [SerializeField]
    GameObject bloodSpurt;

    [SerializeField]
    float hitFlashSpeed;

    [SerializeField]
    public int maxLifePoint;

    [SerializeField] 
    GameObject bloodEffectdeath;

    [SerializeField]
    GameObject bloodEffectalive;

    private int lifePoint;
    public delegate void OnChangedDelegate();

    [HideInInspector]
    public OnChangedDelegate onHealthChangedCallback;
    [HideInInspector]
    public OnChangedDelegate onLifePointChangedCallback;
    float healTimer;

    [SerializeField]
    float timeToHeal;

    [SerializeField]
    GameObject deathCutScene;

   

    public float prevhealth;

    

    [Space(5)]
    [Header("Mana Settings")]
    //[SerializeField]
    //UnityEngine.UI.Image manaStorage;

    [SerializeField]
    float mana;
    [SerializeField]
    public int manaMax;

    [SerializeField]
    float manaDrainSpeed;

    [SerializeField]
    float manaGain;

    [HideInInspector]
    public OnChangedDelegate  onManaChangedCallback;

    [Space(5)]
    [Header("Spell Setting")]
    [SerializeField]
    float manaSpellcost = 0.3f;

    [SerializeField]
    float timeBetweenCast = 0.5f;
    float timeSinceCast;

    [Space(5)]
    [Header("Camera Stuff")]
    [SerializeField]
    private float playerFallSpeedthreshold = -10;

    [HideInInspector]
    public PlayerStateList pState;

    [HideInInspector]
    public Rigidbody2D rb;
    private SpriteRenderer sr;

    private Vector2 Axis;
    private float xAxis,
        yAxis;
    bool openMap;

    [Space(5)]
    [Header("Wall Jump Setting")]
    [SerializeField]
    private float wallSlidingSpeed = 2f;

    [SerializeField]
    private Transform wallCheck;

    [SerializeField]
    private float wallJumpingDuration;

    [SerializeField]
    private LayerMask wallLayer;

    [SerializeField]
    private Vector2 wallJumpingPower;
    private float wallJumpingDirection;
    private bool isWallSliding;
    bool isWallJumping;

    [Header("Parry Setting")]
    [SerializeField]
    private float parryGrace;

    [SerializeField]
    private float grazeGrace;

    [SerializeField]
    private float parryCoolTime;

    [SerializeField]
    private float parriedTime;

    [SerializeField]
    private GameObject parryEffect;
    [SerializeField]
    private GameObject parryImpactEffect;
     [SerializeField]
    private GameObject justParryImpactEffect;
    bool parry;

    bool parrytoken;

    [Space(5)]
    [Header("Player Material")]
    [SerializeField]
    private PhysicsMaterial2D airmaterial;
    [SerializeField]
    private PhysicsMaterial2D groundedmaterial;

    private BoxCollider2D bcollider;
    

    [Space(5)]

    [Header("SE Setting")]
    [SerializeField] AudioClip runSound;
    [SerializeField] AudioClip attackSound;
    [SerializeField] AudioClip damageSound;
    [SerializeField] AudioClip parrySound;
    [SerializeField] AudioClip grazeSound;
    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioClip dashSound;
    

    [Space(5)]

    [Header("Dificulty")]
    public bool isCasualMode;
    public int maxDamage;
    [Space(5)]

    Animator anim;
    private float gravity;
    private bool canDash;
    private bool dashed;
    private bool isChangespeed;
    private float playertimescale;
    private AudioSource audioSource;


    private bool Parring;

    private Vector3 fallRespawnPoint;

    private PlayerInput playerInput;

    private InputAction jumpInput;
    private InputAction attackInput;
    private InputAction guardInput;
    private InputAction dashInput;
    private bool attackInputLong;
    

    public static PlayerController Instance;

   

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        pState = GetComponent<PlayerStateList>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        bcollider = GetComponent<BoxCollider2D>();
        if(!SaveData.Instance.newGame)
        {
            SaveData.Instance.LoadPlayerData();
        }
        gravity = rb.gravityScale;
        canDash = true;
        Mana = mana;
       
        Health = maxHealth;
        LifePoint = maxLifePoint;
        pState.alive = true;
        Time.timeScale = 1;
        Attacking = false;
        pState.lookingRight = true;
        Flip();
        timeSinceAttack = 3f;
        playertimescale = 1f;
        Parring = false;
        parrytoken = false;
        deathCutScene.SetActive(false);
        prevhealth = Health;
        audioSource = GetComponent<AudioSource>();
        BGMController.Instance.SetAudioClip();
        playerInput = GetComponent<PlayerInput>();
        jumpInput = playerInput.actions.FindAction("Jump");
        attackInput = playerInput.actions.FindAction("Attack");
        guardInput = playerInput.actions.FindAction("Guard");
        dashInput = playerInput.actions.FindAction("Dash");
        isCasualMode = false;
        maxDamage = 10;



        
       
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(SideAttackTransform.position, SideAttackArea);
        //Gizmos.DrawWireCube(new Vector3(0,0,0), new Vector2(6,4));
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.gameisPaused) return;
        if (pState.cutscene || !pState.alive)
            return;
        if (pState.alive)
        {
            anim.SetBool("Grounded",Grounded());
            GetInput();
            ToggleMap();
            SetMaterial();
            //ChangePlayerSpeed();
            
        }
       
        UpdateJumpVariables();
        RestoreTimeScale();
        ChargeAttackSet();
        StartDash();

        UpdateCameraDampForPlayerFall();
      

        if (pState.dashing || pState.healing || pState.chargeAttacking || pState.damaged )
            return;
        if (pState.alive)
        {
            if (!isWallJumping  && !Attacking && !Parring && !charging)
            {
                Move();
                Flip();
                Jump();
            }
            WallSlide();
            WallJump();
            //Heal();
            

           
            Attack();
            Parry();

        }
        FlashWhiteInvincible();
     
         
        
    }

    private void FixedUpdate()
    {
        if (pState.cutscene)
            return;
        if (pState.dashing)
            return;
        
        //if (Input.GetKeyDown("c")) StartCoroutine(Death());
       
    }

    void GetInput()
    {
        xAxis = Mathf.Round(Axis.x);
        yAxis = Mathf.Round(Axis.y);
        
        if(!attack) attack = attackInput.WasPressedThisFrame();
        
        charging =attackInputLong && !pState.chargeAttacking && !attack && !pState.damaged;
        chargeAttack = attackInput.WasReleasedThisFrame();
        
        parry = guardInput.WasPressedThisFrame() && !Attacking && !charging && !pState.chargeAttacking;
    }

    public void OnMoveGet(InputAction.CallbackContext context)
    {
        Axis = context.ReadValue<Vector2>();
       
    }

    public void OnAttackGet(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            attackInputLong = true;
        }
        if(context.canceled)
        {
            attackInputLong = false;
        }
       
    }
    
    void SetMaterial()
    {
        if(Grounded())
        {
            bcollider.sharedMaterial = groundedmaterial;
        }
        else
        {
            bcollider.sharedMaterial = airmaterial;
        }
    }

  
    void ToggleMap()
    {
        if (openMap)
        {
            UIManager.Instance.mapHandler.SetActive(true);
        }
        else
        {
            UIManager.Instance.mapHandler.SetActive(false);
        }
    }

    void Flip()
    {
        if (xAxis < 0)
        {
            //transform.localScale = new Vector2(0.005f, transform.localScale.y);
            pState.lookingRight = false;
            checker.localScale = new Vector3(200, 200, 200);
            
        }
        else if (xAxis > 0)
        {
            //transform.localScale = new Vector2(-0.005f, transform.localScale.y);
            pState.lookingRight = true;
            checker.localScale = new Vector3(-200, 200, 200);
        }
        anim.SetBool("LookingRight", pState.lookingRight);
    }

    private void Move()
    {
        bool isground = Grounded();
        float JumpBlend = Mathf.Clamp(rb.velocity.y / (2 * jumpForce), -0.5f, 0.5f);
        rb.velocity = new Vector2(walkspeed * playertimescale * xAxis, rb.velocity.y);
        if(rb.velocity.x != 0 && isground)
        {
            if(sinceSound >= durationSound)
            {
                
                audioSource.PlayOneShot(runSound);
                sinceSound = 0;
            }
            else
            {
                sinceSound += Time.deltaTime;
            }
        }
        else
        {
            sinceSound = durationSound;
        }

        
        if (isground && !isWallSliding && rb.velocity.y == 0)
        {
            anim.SetFloat("Blend", (pState.lookingRight ? 1f : 0f) + (xAxis == 0 ? 0f : 2f));
            fallRespawnPoint = transform.position;
            
        }
      
        else
        {
            if (isWallSliding)
            {
                anim.SetFloat("Blend", (pState.lookingRight ? 8f : 9f));
            }
            else
            {
                anim.SetFloat("Blend", (pState.lookingRight ? 6.5f : 4.5f) - JumpBlend);
            }
        }
        //anim.SetBool("Walking", rb.velocity.x != 0 && Grounded());
    }


    public IEnumerator FallRespawn()
    {
        yield return new WaitForSeconds(0.5f);
        transform.position = fallRespawnPoint;
        Health -= 30;

    }

    public void MaxHeal()
    {
        Health = maxHealth;
        LifePoint = maxLifePoint;
        prevhealth = maxHealth;
    }
    void UpdateCameraDampForPlayerFall()
    {
        if (
            rb.velocity.y < playerFallSpeedthreshold
            && !CameraManager.Instance.isLerpingYDamp
            && !CameraManager.Instance.hasLerpingYDamping
        )
        {
            StartCoroutine(CameraManager.Instance.LerpYDamping(true));
        }
        if (
            rb.velocity.y >= 0
            && !CameraManager.Instance.isLerpingYDamp
            && CameraManager.Instance.hasLerpingYDamping
        )
        {
            CameraManager.Instance.hasLerpingYDamping = false;
            StartCoroutine(CameraManager.Instance.LerpYDamping(false));
        }
    }

    void StartDash()
    {
        if (dashInput.WasPressedThisFrame() && canDash && !dashed && !charging && !pState.chargeAttacking)
        {
            pState.invincible = true;
            pState.damaged = false;
            StartCoroutine(Dash());
            dashed = true;
        }
        if (Grounded())
        {
            dashed = false;
        }
    }

    IEnumerator Dash()
    {
        
        anim.SetTrigger("Dash");
        StopCoroutine(StopTakingDamage());
        if(xAxis != 0 )
            Flip();
        canDash = false;
        anim.SetBool("Damaging", false);
        pState.dashing = true;
        pState.damaged = false;
        anim.SetFloat("Blend",pState.lookingRight ? 11f : 10f);
        StartCoroutine(SetVFX(dashEffect,effecttransform,0,new Vector3(0,0,0),new Vector3(90,0,0),new Vector3(1,1,1)));
        rb.gravityScale = 0;
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(dashSound);
        int _dir = pState.lookingRight ? 1 : -1;
        if (xAxis != 0)
            _dir = xAxis > 0 ? 1 : -1;
        rb.velocity = new Vector2(_dir * dashSpeed * playertimescale, 0);
        if (Grounded())
            Instantiate(dashEffect, transform);
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = gravity * Mathf.Pow(playertimescale, 2);
        pState.dashing = false;
        pState.invincible = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
        anim.ResetTrigger("Dash");
    }

    void ChangePlayerSpeed()
    {
        if (isChangespeed)
        {
            playertimescale = 2f;
            rb.gravityScale = gravity * Mathf.Pow(playertimescale, 2);
        }
        else
        {
            playertimescale = 1f;
            rb.gravityScale = gravity;
        }
    }

    public IEnumerator WalkIntoNewScene(Vector2 _exitDir, float _delay)
    {
        if (_exitDir.y > 0)
        {
            rb.velocity = jumpForce * _exitDir;
        }
        if (_exitDir.x != 0)
        {
            xAxis = _exitDir.x > 0 ? 1 : -1;

            Move();
        }
        Flip();
        yield return new WaitForSeconds(_delay);
        pState.cutscene = false;
    }

    public enum AttackState
    {
        None,
        FirstAttack,
        SecondAttack,
        ThirdAttack,
        ChargeCombo1,
        ChargeCombo2,
        ChargeCombo3,
       

    }

    void Attack()
    {
       anim.ResetTrigger("CancelAttack");
       
       timeSinceAttack = Mathf.Clamp(timeSinceAttack + Time.deltaTime,0,3);
        if (timeSinceAttack > comboResetTime || currentAttack == AttackState.ThirdAttack)
        {
            currentAttack = AttackState.None;
            
        }
        if((currentAttack == AttackState.ChargeCombo1 || currentAttack == AttackState.ChargeCombo2 || currentAttack == AttackState.ChargeCombo3) )
        {
            if( (xAxis != 0 || !Grounded()) || Mana < 1 ||(currentAttack == AttackState.ChargeCombo3 && Mana <2))
            {
            currentAttack = AttackState.None;
            anim.SetTrigger("CancelAttack");
            
            }
        }
        if (timeSinceAttack >= timeBetweenAttack)
        {
            Attacking = false;
            anim.SetBool("Attacking",false);
        }
        if (attack  && timeSinceAttack >= timeBetweenAttack )
        {
            attack = false;
            Attacking = true;
            anim.SetBool("Attacking", true);
            rb.velocity = new Vector2(0f, rb.velocity.y);
            if (!Grounded())
                rb.velocity = new Vector2(rb.velocity.x,15);
            timeSinceAttack = 0;
            if (currentAttack == AttackState.None)
            {
                audioSource.pitch =1f;
                audioSource.PlayOneShot(attackSound);
                currentAttack = AttackState.FirstAttack;
                anim.SetTrigger("AttackTrigger");
                anim.SetInteger("AttackType",0);
                Hit(SideAttackTransform, SideAttackArea,damage);
                StartCoroutine(SetVFX(slashEffect,effecttransform,500, new Vector3(0, 0.45f, 0) ,new Vector3(-15,0,0),new Vector3(-2.5f,2,2)));
            }
            else if (currentAttack == AttackState.FirstAttack)
            {
                audioSource.pitch =0.9f;
                audioSource.PlayOneShot(attackSound);
                currentAttack = AttackState.SecondAttack;
                anim.SetTrigger("AttackTrigger");
                anim.SetInteger("AttackType", 1);
                Hit(SideAttackTransform, SideAttackArea,damage);
                StartCoroutine(SetVFX(slashEffect, effecttransform, 0, new Vector3(0, -0.15f, 0), new Vector3(20, 0, 0), new Vector3(-2.5f, 2, 2)));
            }
            else if (currentAttack == AttackState.SecondAttack)
            {
                audioSource.pitch =1.1f;
                audioSource.PlayOneShot(attackSound);
                currentAttack = AttackState.ThirdAttack;
                anim.SetTrigger("AttackTrigger");
                anim.SetInteger("AttackType", 2);
                Hit(SideAttackTransform, SideAttackArea,2*damage);
                StartCoroutine(SetVFX(slashEffect, effecttransform, 500, new Vector3(0, 0.48f, 0), new Vector3(-30, 0, 0), new Vector3(-2.5f, 2, 2)));
            }
            else if(currentAttack == AttackState.ChargeCombo1 && Mana >= 1)
            {
                Mana -= 1;
                StartCoroutine(ChargeCombo1());
            }
            else if (currentAttack == AttackState.ChargeCombo2 && Mana >= 1)
            {
                Mana -= 1;
                StartCoroutine(ChargeCombo2());
            }
            else if (currentAttack == AttackState.ChargeCombo3 && Mana >= 2)
            {
                Mana -= 2;
                StartCoroutine(ChargeCombo3());
            }
           
        }
    }

    private void ChargeAttackSet()
    {
        anim.SetBool("Charging", ischarging);
        if (charging && Grounded() && !Attacking)
        {
            
            if (chargeTime == 0)
                chargeEffect.GetComponent<VisualEffect>().SendEvent("OnPlay");
            ischarging = true;
            chargeTime =Mathf.Clamp(chargeTime + Time.deltaTime,0,maxChargeTime);
        }
        else if (chargeTime >= maxChargeTime && chargeAttack)
        {
            ischarging = false;
            chargeTime = 0;
            pState.chargeAttacking = true;
            chargeEffect.GetComponent<VisualEffect>().SendEvent("Stop");
            StartCoroutine(ChargeAttack());
        }
        else if (!charging)
        {
            ischarging = false;
            chargeEffect.GetComponent<VisualEffect>().SendEvent("Stop");
            chargeTime = 0;
        }
    }

    IEnumerator ChargeAttack()
    {
        audioSource.pitch =1.2f;
        audioSource.PlayOneShot(attackSound);
        rb.velocity = Vector2.zero;
        Hit(SideAttackTransform, SideAttackArea, 1 * damage);
        anim.SetBool("Attacking", true);
        anim.SetFloat("Blend",0);
        anim.SetTrigger("AttackTrigger");
        pState.counter = true;
        timeSinceAttack = comboResetTime - 0.5f;
        StartCoroutine(SetVFX(slashEffect, effecttransform, 500, new Vector3(0, 0.48f, 0), new Vector3(30, 0, 0), new Vector3(-2.5f, 2, 2)));
        yield return new WaitForSeconds(0.2f);
        pState.counter = false;
        yield return new WaitForSeconds(0.55f);
        audioSource.pitch =0.8f;
        audioSource.PlayOneShot(attackSound);
        StartCoroutine(SetVFX(slashEffect, effecttransform, 500, new Vector3(0, -1.25f, 0), new Vector3(pState.lookingRight ? 150 : 30, 90, -90), new Vector3(-2f, 2, 2)));
        Hit(SideAttackTransform, SideAttackArea, 4 * damage);
        yield return new WaitForSeconds(0.03f);
        StartCoroutine(SetVFX(impactEffect, effecttransform, 500, new Vector3(pState.lookingRight ? 4 : -4, -2.22f, 0), new Vector3(0,0,0), new Vector3(2, 2, 2)));
        yield return new WaitForSeconds(0.22f);
        pState.chargeAttacking = false;
        anim.SetBool("Attacking", false);
        currentAttack = AttackState.ChargeCombo1;
    }

    IEnumerator ChargeCombo1()
    {
        timeSinceAttack = comboResetTime -0.5f;
        pState.chargeAttacking = true;
        anim.SetTrigger("AttackTrigger");
        anim.SetBool("Attacking", true);
       
        
        yield return new WaitForSeconds(0.83f);
        audioSource.pitch =1.3f;
                audioSource.PlayOneShot(attackSound);
        StartCoroutine(SetVFX(stabEffect, effecttransform, 500, new Vector3(pState.lookingRight ? 1 : -1, 0, 0), new Vector3(0, 0, 0), new Vector3(-2, 2, 2)));
        Hit(SideAttackTransform, SideAttackArea, 5 * damage);
        yield return new WaitForSeconds(0.17f);
        pState.chargeAttacking = false;
        anim.SetBool("Attacking", false);
        currentAttack = AttackState.ChargeCombo2;
       
    }

    IEnumerator ChargeCombo2()
    {
       
        timeSinceAttack = comboResetTime - 0.5f;
        pState.chargeAttacking = true;
        anim.SetBool("Attacking", true);
        anim.SetTrigger("AttackTrigger");
        yield return new WaitForSeconds(0.42f);
        StartCoroutine(SetVFX(slashEffect, effecttransform, 0, new Vector3(0, 0.48f, 0), new Vector3(30, 0, 0), new Vector3(-2.5f, 2, 2)));
        audioSource.pitch =1.1f;
                audioSource.PlayOneShot(attackSound);
        Hit(SideAttackTransform, SideAttackArea, 5 * damage);
        yield return new WaitForSeconds(0.4133f);
        pState.chargeAttacking = false;
        anim.SetBool("Attacking", false);
        currentAttack = AttackState.ChargeCombo3;
        
    }

    IEnumerator ChargeCombo3()
    {
        
        timeSinceAttack = comboResetTime - 0.5f;
        pState.chargeAttacking = true;
        anim.SetBool("Attacking", true);
        anim.SetTrigger("AttackTrigger");

        yield return new WaitForSeconds(0.42f);
        audioSource.pitch =1.5f;
                audioSource.PlayOneShot(attackSound);
        StartCoroutine(SetVFX(slashEffect, effecttransform, 500, new Vector3(0, 0.9f, 0), new Vector3(-10, 0, pState.lookingRight ? -20 : 20), new Vector3(-3, 2, 2)));
        Hit(SideAttackTransform, SideAttackArea, 5 * damage);
        yield return new WaitForSeconds(0.246f);
        pState.chargeAttacking = false;
        anim.SetBool("Attacking", false);
        currentAttack = AttackState.None;
        timeSinceAttack = 0;
    }

    
    private void Hit(
        Transform _attackTransform,
        Vector2 _attackArea,
        float _damage
    )
    {
        Collider2D[] objectToHit = Physics2D.OverlapBoxAll(
            _attackTransform.position,
            _attackArea,
            0,
            attackableLayer
        );
        List<Enemy> hitEnemies = new List<Enemy>();

       
        for (int i = 0; i < objectToHit.Length; i++)
        {
            Enemy e = objectToHit[i].GetComponent<Enemy>();
            if (e && !hitEnemies.Contains(e))
            {
                StartCoroutine(SetVFX(hitEffect, effecttransform, 500, new Vector3(pState.lookingRight ? 3f:-3f, 1f, 0), new Vector3(0, 0, 0), new Vector3(2f, 2, 2)));
                //audioSource.pitch = 1.7f + Random.Range(-0.3f,0.3f);
                //audioSource.PlayOneShot(hitSound);
                e.EnemyHit(_damage);
                hitEnemies.Add(e);
                
            }
        }
    }

    IEnumerator SetVFX(GameObject _effect, Transform _position,int _order, Vector3 _offset, Vector3 _angle, Vector3 _scale)
    {
        _effect = Instantiate(_effect, _position);
        _effect.transform.eulerAngles = _angle;
        _effect.transform.localScale = _scale;
        _effect.transform.position += _offset;
        Renderer vfxRenderer = _effect.GetComponent<Renderer>();
        vfxRenderer.sortingOrder = _order;
        yield return null;
       
        
    }

   

     void Parry()
      {
        
          if (parry && (parriedTime > parryCoolTime || parrytoken))
            {
                parrytoken = false;
                rb.velocity = new Vector2(0,rb.velocity.y);
                if (!Grounded())
                    pState.recoilingY = true;
             StartCoroutine(SetVFX(parryEffect,effecttransform,500,new Vector3(0,0,0), new Vector3(0,0,0),new Vector3(3,3,3)));
             parriedTime = 0f;
             Parring = true;
             pState.parry = true;
                anim.SetTrigger("Guard");
          }
         else if (parriedTime >= parryCoolTime)
        {
            Parring = false;
        }
        else if ((parryGrace + grazeGrace) <= parriedTime)
        {
            pState.parry = false;
            pState.graze = false;
        }
        else if (parryGrace <= parriedTime)
        {
            pState.parry = false;
            pState.graze = true;
        }
        
        parriedTime = Mathf.Clamp(parriedTime + Time.deltaTime,0f,2f);


    }

    public void CasualMode(bool _onOff)
    {
        isCasualMode = _onOff;
        parryGrace = _onOff ? 0.3f : 0.1f;
        grazeGrace = _onOff ? 0f : 0.1f;
    }
    public void TakeDamege(float _damage, bool _enemyDirection,bool _parryable, Enemy? e)
    {
        if (pState.alive)
        {
            if ((pState.parry || pState.graze) && (_enemyDirection == !pState.lookingRight) && e != null && _parryable)
            {
                if(pState.parry)
                {
                    DeviceManager.Instance.StartShakeController(0.2f,1,0.1f);
                    Health = Mathf.Clamp(Health + 1,0,prevhealth);
                    audioSource.pitch =1.5f;
                    audioSource.PlayOneShot(parrySound);
                    StartCoroutine(SetVFX(justParryImpactEffect, effecttransform, 500, new Vector3(pState.lookingRight ? 3f:-3f, 1f, 0), new Vector3(0, 0, 0), new Vector3(2f, 2, 2)));
                }
                else
                {
                     DeviceManager.Instance.StartShakeController(0,0.7f,0.1f);
                    Health = Mathf.Clamp(Health - 2,1,prevhealth);
                    audioSource.pitch =1f;
                    audioSource.PlayOneShot(grazeSound);
                    StartCoroutine(SetVFX(parryImpactEffect, effecttransform, 500, new Vector3(pState.lookingRight ? 3f:-3f, 1f, 0), new Vector3(0, 0, 0), new Vector3(2f, 2, 2)));
                }
                parrytoken = true;
                Mana += pState.parry ? 0.4f : 0.1f;
                
                e.ChargeStun(pState.parry);
                pState.parry = false;
                pState.graze =false;

            }
            else if (pState.counter && _enemyDirection == !pState.lookingRight && e != null)
            {
                e.toStun();
            }
            else if ((pState.parry || pState.graze) && (_enemyDirection == !pState.lookingRight) && e == null && _parryable)
            {
                if(pState.parry)
                {
                    audioSource.pitch =1.5f;
                    audioSource.PlayOneShot(parrySound);
                }
                else
                {
                    audioSource.pitch =1f;
                    audioSource.PlayOneShot(grazeSound);
                }
                Mana += pState.parry ? 0.4f : 0.1f;
                parrytoken = true;
                pState.parry = false;
                pState.graze = false;
            }
            else
            {
                DeviceManager.Instance.StartShakeController(0.5f,0.5f,0.3f);
                audioSource.PlayOneShot(damageSound);
                prevhealth = Health;
                Health -= isCasualMode ? maxDamage : _damage;
                StopAllCoroutines();
                canDash = true;
                
                StopCoroutine(ChargeAttack());
                StopCoroutine(ChargeCombo1());
                StopCoroutine(ChargeCombo2());
                StopCoroutine(ChargeCombo3());
                if(Health > 0) HitStopTime(0.5f,1,0.3f);                

                pState.chargeAttacking = false;
                 pState.counter = false;
                Attacking = false;

                if (Health <= 0 && LifePoint <= 0)
                {
                    Health = 0;
                    rb.velocity = Vector2.zero;
                    StartCoroutine(Death());
                }
                else if(Health <= 0 && LifePoint > 0)
                {
                    StartCoroutine(Restore());
                }
                else
                {
                    StartCoroutine(StopTakingDamage());
                  
                }
            }
        }
       
    }

    IEnumerator StopTakingDamage()
    {
        pState.invincible = true;
        pState.damaged = true;
        StartCoroutine(SetVFX(bloodSpurt, effecttransform, 0, Vector3.zero, Vector3.zero, new Vector3(3, 3, 3)));
        anim.SetTrigger("Damage");
        anim.SetBool("Damaging",true);
        yield return new WaitForSeconds(0.1f);
        pState.invincible = false;
        yield return new WaitForSeconds(0.4f);
        pState.damaged = false;
        anim.SetBool("Damaging", false);
      
    }

    IEnumerator Restore()
    {
        rb.velocity = Vector2.zero;
        pState.alive = false;
        deathCutScene.SetActive(true);
        anim.SetTrigger("Death");
        anim.SetBool("isDeath",true);
        HitStopTime(0.5f, 1f, 0.5f);
        StartCoroutine(SetVFX(bloodEffectalive, effecttransform, 500, Vector3.zero, Vector3.zero, new Vector3(3, 3, 3)));
        yield return new WaitForSeconds(0.5f);
        anim.SetFloat("Speed",-1);
        Health = maxHealth;
        LifePoint--;
        prevhealth = maxHealth;
        pState.invincible = true;
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("isDeath", false);
        anim.SetFloat("Speed", 0);
        deathCutScene.SetActive(false);
        pState.alive = true;
        pState.damaged = false;
        yield return new WaitForSeconds(0.5f);
        pState.invincible = false;
        anim.SetFloat("Speed", 1);
    }

    void FlashWhiteInvincible()
    {
        sr.material.color = pState.invincible
            ? Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time * hitFlashSpeed, 1.0f))
            : Color.white;
    }

    void RestoreTimeScale()
    {
        
        if (restoreTime)
        {
           
            if (Time.timeScale < 1)
            {
                Time.timeScale = Mathf.Clamp(Time.timeScale +  Time.unscaledDeltaTime * restoreTimeSpeed,0,1);
            }
            else
            {
                Time.timeScale = 1;
                restoreTime = false;
            }
        }
    }

    public void HitStopTime(float _newTimeScale, float _restoreSpeed, float _delay)
    {
        restoreTimeSpeed = _restoreSpeed;
        Time.timeScale = _newTimeScale;
        if (_delay > 0)
        {
            StopCoroutine(StartTimeAgain(_delay));
            StartCoroutine(StartTimeAgain(_delay));
        }
        else
        {
            restoreTime = true;
        }
    }
    

    IEnumerator StartTimeAgain(float _delay)
    {
        
        yield return new WaitForSecondsRealtime(_delay);
        restoreTime = true;
    }

    IEnumerator Death()
    {
        rb.velocity = Vector2.zero;
        pState.alive = false;
        anim.SetTrigger("Death");
        anim.SetBool("isDeath",true);
        deathCutScene.SetActive(true);
        StartCoroutine(SetVFX(bloodEffectdeath,effecttransform,500,Vector3.zero,Vector3.zero,new Vector3(3,3,3)));
        HitStopTime(0.5f,1f,0.5f);
        TutorialManager.Instance.FadeOutTutorialAll();
        yield return new WaitForSeconds(0.5f);
        
        StartCoroutine(UIManager.Instance.ActivateDeathScreen());
        yield return new WaitForSeconds(3f);
        anim.SetBool("isDeath", false);
        deathCutScene.SetActive(false);
    }
       

    public void Respawned()
    {
        if (!pState.alive)
        {
            pState.alive = true;
            Health = maxHealth;
            LifePoint = maxLifePoint;
            prevhealth = maxHealth;
            Mana = 0;
            anim.SetBool("isDeath", false);
            deathCutScene.SetActive(false);
            pState.damaged = false;
            BGMController.Instance.SetAudioClip();
            StartCoroutine(UIManager.Instance.DeactivateDeathScreen());
            //anim.Play("player_Idle");
        }
    }

    public float Health
    {
        get { return health; }
        set
        {
            if (health != value)
            {
                health = Mathf.Clamp(value, 0, maxHealth);

                if (onHealthChangedCallback != null)
                {
                    onHealthChangedCallback.Invoke();
                }
            }
        }
    }

    public int LifePoint
    {
        get { return lifePoint; }
        set
        {
            if (lifePoint != value)
            {
                lifePoint = Mathf.Clamp(value, 0, maxLifePoint);

                if (onLifePointChangedCallback != null)
                {
                    onLifePointChangedCallback.Invoke();
                }
            }
        }
    }

    void Heal()
    {
        if (
            Input.GetButton("Healing")
            && Health < maxHealth
            && Mana > 0
            && Grounded()
            && !pState.dashing
        )
        {
            pState.healing = true;
            //healing
            healTimer += Time.deltaTime;
            if (healTimer >= timeToHeal)
            {
                Health++;
                healTimer = 0;
            }
            Mana -= Time.deltaTime * manaDrainSpeed;
        }
        else
        {
            pState.healing = false;
            healTimer = 0;
        }
    }


    public float Mana
    {
        get { return mana; }
        set
        {
            if (mana != value)
            {
                mana = Mathf.Clamp(value, 0, manaMax);
                if(onManaChangedCallback != null)
                {
                    onManaChangedCallback.Invoke();
                }
            }
        }
    }
    

    public bool Grounded()
    {
        if (
            Physics2D.Raycast(groundCheakPoint.position, Vector2.down, groundCheakY, whatIsGround)
            || Physics2D.Raycast(
                groundCheakPoint.position + new Vector3(groundCheakX, 0, 0),
                Vector2.down,
                groundCheakY,
                whatIsGround
            )
            || Physics2D.Raycast(
                groundCheakPoint.position + new Vector3(-groundCheakX, 0, 0),
                Vector2.down,
                groundCheakY,
                whatIsGround
            )
        )
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Jump()
    {
        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0 && !pState.jumping)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce * playertimescale);
            pState.jumping = true;
        }
        else if (!Grounded() && airJumpCounter < maxAirJumps && jumpInput.WasPressedThisFrame())
        {
            pState.jumping = true;
            airJumpCounter++;
            rb.velocity = new Vector3(rb.velocity.x, jumpForce * playertimescale);
        }
        if (jumpInput.WasReleasedThisFrame() && rb.velocity.y > 3)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            pState.jumping = false;
        }

        //anim.SetBool("Jumping", !Grounded());
    }

    void UpdateJumpVariables()
    {
        if (Grounded())
        {
            pState.jumping = false;
            coyoteTimeCounter = coyoteTime;
            airJumpCounter = 0;
        }
        else if (coyoteTimeCounter > 0)
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (jumpInput.WasPressedThisFrame())
        {
            jumpBufferCounter = jumpBufferFrames;
        }
        else if (jumpBufferCounter > 0)
        {
            jumpBufferCounter--;
        }
    }

    private bool Walled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.01f, wallLayer);
    }

    void WallSlide()
    {
        if (Walled() && !Grounded() && ((xAxis >= 0 && pState.lookingRight) || (xAxis <= 0 && !pState.lookingRight)))
        {
            isWallSliding = true;
            rb.velocity = new Vector2(
                rb.velocity.x,
                Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue)
            );
        }
        else
        {
            isWallSliding = false;
        }
    }

    void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = !pState.lookingRight ? 1 : -1;

            CancelInvoke(nameof(StopWallJumping));
        }
        if (jumpInput.WasPressedThisFrame() && isWallSliding)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(
                wallJumpingPower.x * wallJumpingDirection,
                wallJumpingPower.y
            );
            dashed = false;
            airJumpCounter = 1;
            pState.lookingRight = !pState.lookingRight;
            float JumpBlend = Mathf.Clamp(rb.velocity.y / (2 * jumpForce), -0.5f, 0.5f);
            anim.SetFloat("Blend", (pState.lookingRight ? 6.5f : 4.5f) - JumpBlend);

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    void StopWallJumping()
    {
        isWallJumping = false;
    }

   
}
