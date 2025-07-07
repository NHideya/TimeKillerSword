
 //ゲームオブジェクトへアタッチすることでプレイヤーとして操作可能にするクラス
 
 //NOTE: PlayerControllerクラスが受け持つ処理が多いためクラスの分離が必要である


using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.VFX;

public class PlayerController : MonoBehaviour
{
    [Header("横移動用設定")]
    [SerializeField]
    private float walkspeed = 1;

    [SerializeField]
    private float durationSound;
    private float sinceSound;

    [Space(5)]
    [Header("ジャンプ設定")]
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
    [Header("設置判定設定")]
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

    [Space(5)]
    [Header("ダッシュ用設定")]
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
    [Header("攻撃設定")]
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
    [Header("チャージ攻撃設定")]
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
    [Header("体力設定")]
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
    [Header("攻撃用ゲージ（マナ）設定")]
   

    [SerializeField]
    float mana;

    [SerializeField]
    public int manaMax;


    [HideInInspector]
    public OnChangedDelegate onManaChangedCallback;
    [Space(5)]
    [Header("カメラ設定")]
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
    [Header("壁キック設定")]
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

    [Header("ジャストガード設定")]
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
    [Header("プレイヤーの摩擦の設定")]
    [SerializeField]
    private PhysicsMaterial2D airmaterial;

    [SerializeField]
    private PhysicsMaterial2D groundedmaterial;

    private BoxCollider2D bcollider;

    [Space(5)]
    [Header("サウンドエフェクト設定")]
    [SerializeField]
    AudioClip runSound;

    [SerializeField]
    AudioClip attackSound;

    [SerializeField]
    AudioClip damageSound;

    [SerializeField]
    AudioClip parrySound;

    [SerializeField]
    AudioClip grazeSound;

    [SerializeField]
    AudioClip hitSound;

    [SerializeField]
    AudioClip dashSound;

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

    // 起動時の初期化処理
    // Awake の実行は一度のみ 
    private void Awake()
    {
        // PlayerControllerをSingletonにする
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

    // Awake()後の初期化処理
    // Startの実行は一度のみ
    void Start()
    {
        pState = GetComponent<PlayerStateList>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        bcollider = GetComponent<BoxCollider2D>();
        if (!SaveData.Instance.newGame)
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
        
    }

    // Updateは毎フレーム呼び出し
    void Update()
    {
        if (GameManager.Instance.gameisPaused)
            return;
        if (pState.cutscene || !pState.alive)
            return;
        if (pState.alive)
        {
            anim.SetBool("Grounded", Grounded());
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

        if (pState.dashing || pState.healing || pState.chargeAttacking || pState.damaged)
            return;
        if (pState.alive)
        {
            if (!isWallJumping && !Attacking && !Parring && !charging)
            {
                Move();
                Flip();
                Jump();
            }
            WallSlide();
            WallJump();

            Attack();
            Parry();
        }
        
    }

    private void FixedUpdate()
    {
        if (pState.cutscene)
            return;
        if (pState.dashing)
            return;
    }

    // キー入力を受取り、各アクションの実行可否をTrueへ更新、
    // 左ステック、WASDの入力を受け取る
    // 毎フレーム実行
    void GetInput()
    {
        // 左ステック、WASDの入力を受け取る
        // 左ステックの入力は四捨五入する
        xAxis = Mathf.Round(Axis.x);
        yAxis = Mathf.Round(Axis.y);

        //攻撃入力時、確実に攻撃できるようにするため、攻撃実行時まで入力を保持する
        if (!attack)
            attack = attackInput.WasPressedThisFrame();

        charging = attackInputLong && !pState.chargeAttacking && !attack && !pState.damaged;
        chargeAttack = attackInput.WasReleasedThisFrame();

        parry =
            guardInput.WasPressedThisFrame() && !Attacking && !charging && !pState.chargeAttacking;
    }

    // 移動キー入力時のみ実行
    //左ステック入力を２次元ベクトルとして受け取る
    public void OnMoveGet(InputAction.CallbackContext context)
    {
        Axis = context.ReadValue<Vector2>();
    }

    // 攻撃キー入力時のみ実行
    // 攻撃ボタンの長押しを検知
    public void OnAttackGet(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            attackInputLong = true;
        }
        if (context.canceled)
        {
            attackInputLong = false;
        }
    }

    // 毎フレーム実行
    // ジャンプ時プレイヤーが壁に引っかかる現象を防ぐため摩擦係数をジャンプ時0にする
    void SetMaterial()
    {
        if (Grounded())
        {
            bcollider.sharedMaterial = groundedmaterial;
        }
        else
        {
            bcollider.sharedMaterial = airmaterial;
        }
    }

    // マップ機能削除に伴い不使用
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

    // 毎フレーム実行
    // プレイヤーのグラフィックを左右反転
    void Flip()
    {
        if (xAxis < 0)
        {
            pState.lookingRight = false;
            checker.localScale = new Vector3(200, 200, 200);
        }
        else if (xAxis > 0)
        {
            pState.lookingRight = true;
            checker.localScale = new Vector3(-200, 200, 200);
        }
        anim.SetBool("LookingRight", pState.lookingRight);
    }

    // 毎フレーム実行
    private void Move()
    {
        bool isground = Grounded();
        // ジャンプアニメーションをY軸方向の速度に応じて変更
        float JumpBlend = Mathf.Clamp(rb.velocity.y / (2 * jumpForce), -0.5f, 0.5f);

        //プレイヤーのX軸方向の速度を入力に応じて計算
        rb.velocity = new Vector2(walkspeed * playertimescale * xAxis, rb.velocity.y);

        // プレイヤー接地時の足音の制御
        if (rb.velocity.x != 0 && isground)
        {
            if (sinceSound >= durationSound)
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

        // プレイヤーの接地状態、速度に応じて歩行、ジャンプ、壁張り付きアニメーションの選択を行う
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
    }

    //落下時実行
    //落下ダメージとリスポーンを行う非同期処理
    public IEnumerator FallRespawn()
    {
        yield return new WaitForSeconds(0.5f);
        transform.position = fallRespawnPoint;
        Health -= 30;
    }

    //体力を最大値に設定
    public void MaxHeal()
    {
        Health = maxHealth;
        LifePoint = maxLifePoint;
        prevhealth = maxHealth;
    }

    // 高速落下時に実行
    // 高速落下するプレイヤーをカメラが追えるよう処理

    //  TODO: プレイヤーとカメラで相互依存関係となっているためMV(R)Pパターンで要修正
    void UpdateCameraDampForPlayerFall()
    {
        //プレイヤーが高速落下中かつカメラの減衰変更前ならカメラの減衰の変更を開始
        if (
            rb.velocity.y < playerFallSpeedthreshold
            && !CameraManager.Instance.isLerpingYDamp
            && !CameraManager.Instance.hasLerpingYDamping
        )
        {
            StartCoroutine(CameraManager.Instance.LerpYDamping(true));
        }

        //落下が終了したときカメラの減衰をもとに戻す処理
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

    //毎フレーム実行
    //ダッシュ可能なときプレイヤーを無敵状態にし、ダッシュさせる
    void StartDash()
    {
        if (
            dashInput.WasPressedThisFrame()
            && canDash
            && !dashed
            && !charging
            && !pState.chargeAttacking
        )
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

    //プレイヤーを高速移動(ダッシュ)させる非同期処理

    IEnumerator Dash()
    {
        anim.SetTrigger("Dash");
        StopCoroutine(StopTakingDamage());
        if (xAxis != 0)
            Flip();
        canDash = false;
        anim.SetBool("Damaging", false);
        pState.dashing = true;
        pState.damaged = false;
        anim.SetFloat("Blend", pState.lookingRight ? 11f : 10f);
        StartCoroutine(
            SetVFX(
                dashEffect,
                effecttransform,
                0,
                new Vector3(0, 0, 0),
                new Vector3(90, 0, 0),
                new Vector3(1, 1, 1)
            )
        );
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

    // プレイヤーの全ての行動速度を上昇させる処理
    // 未実装のため不使用
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

    // 別のエリアから移動したとき実行
    // エリア間の自動移動を行う
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

    //攻撃のコンボの制御を行う列挙型
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

    // 毎フレーム実行
    // 攻撃のクールタイムの更新、コンボ状態に応じた攻撃処理の実行、コンボの管理を行う

    // TODO: 分岐が複雑化しているためStateMachineでコンボ管理をするよう修正したほうがよい
    void Attack()
    {
        anim.ResetTrigger("CancelAttack");

        timeSinceAttack = Mathf.Clamp(timeSinceAttack + Time.deltaTime, 0, 3);

        //コンボのリセット
        if (timeSinceAttack > comboResetTime || currentAttack == AttackState.ThirdAttack)
        {
            currentAttack = AttackState.None;
        }

        // チャージ攻撃が行えない状態の時のコンボリセット処理
        if (
            (
                currentAttack == AttackState.ChargeCombo1
                || currentAttack == AttackState.ChargeCombo2
                || currentAttack == AttackState.ChargeCombo3
            )
        )
        {
            if (
                (xAxis != 0 || !Grounded())
                || Mana < 1
                || (currentAttack == AttackState.ChargeCombo3 && Mana < 2)
            )
            {
                currentAttack = AttackState.None;
                anim.SetTrigger("CancelAttack");
            }
        }

        // 攻撃後のクールタイムの処理、クールタイム中は攻撃不可
        if (timeSinceAttack >= timeBetweenAttack)
        {
            Attacking = false;
            anim.SetBool("Attacking", false);
        }

        // 攻撃可能時、攻撃を実行する
        if (attack && timeSinceAttack >= timeBetweenAttack)
        {
            //保持していた攻撃入力をFalseに変更
            attack = false;
            Attacking = true;
            anim.SetBool("Attacking", true);

            // 攻撃時、横方向の移動を不可にするためプレイヤーを停止させる
            rb.velocity = new Vector2(0f, rb.velocity.y);

            //空中攻撃時、攻撃位置を一定にするためプレイヤーを浮かせる
            if (!Grounded())
                rb.velocity = new Vector2(rb.velocity.x, 15);

            //経過時間をリセットし、エフェクト、アニメーション、サウンドエフェクトを再生し
            //攻撃ヒット判定処理を呼び出す
            
            
            timeSinceAttack = 0;
            if (currentAttack == AttackState.None)
            {
                audioSource.pitch = 1f;
                audioSource.PlayOneShot(attackSound);
                currentAttack = AttackState.FirstAttack;
                anim.SetTrigger("AttackTrigger");
                anim.SetInteger("AttackType", 0);
                Hit(SideAttackTransform, SideAttackArea, damage);
                StartCoroutine(
                    SetVFX(
                        slashEffect,
                        effecttransform,
                        500,
                        new Vector3(0, 0.45f, 0),
                        new Vector3(-15, 0, 0),
                        new Vector3(-2.5f, 2, 2)
                    )
                );
            }
            else if (currentAttack == AttackState.FirstAttack)
            {
                audioSource.pitch = 0.9f;
                audioSource.PlayOneShot(attackSound);
                currentAttack = AttackState.SecondAttack;
                anim.SetTrigger("AttackTrigger");
                anim.SetInteger("AttackType", 1);
                Hit(SideAttackTransform, SideAttackArea, damage);
                StartCoroutine(
                    SetVFX(
                        slashEffect,
                        effecttransform,
                        0,
                        new Vector3(0, -0.15f, 0),
                        new Vector3(20, 0, 0),
                        new Vector3(-2.5f, 2, 2)
                    )
                );
            }
            else if (currentAttack == AttackState.SecondAttack)
            {
                audioSource.pitch = 1.1f;
                audioSource.PlayOneShot(attackSound);
                currentAttack = AttackState.ThirdAttack;
                anim.SetTrigger("AttackTrigger");
                anim.SetInteger("AttackType", 2);
                Hit(SideAttackTransform, SideAttackArea, 2 * damage);
                StartCoroutine(
                    SetVFX(
                        slashEffect,
                        effecttransform,
                        500,
                        new Vector3(0, 0.48f, 0),
                        new Vector3(-30, 0, 0),
                        new Vector3(-2.5f, 2, 2)
                    )
                );
            }

            //チャージ攻撃はゲージ(マナ)を消費する
            else if (currentAttack == AttackState.ChargeCombo1 && Mana >= 1)
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

    //毎フレーム実行
    //チャージ時間の増加、リセットを管理し。
    //チャージ攻撃可否の判定、チャージ攻撃の呼び出しを行う
    private void ChargeAttackSet()
    {
        anim.SetBool("Charging", ischarging);
        //攻撃長押しかつ接地時にチャージを行い
        if (charging && Grounded() && !Attacking)
        {
            if (chargeTime == 0)
                chargeEffect.GetComponent<VisualEffect>().SendEvent("OnPlay");
            ischarging = true;
            chargeTime = Mathf.Clamp(chargeTime + Time.deltaTime, 0, maxChargeTime);
        }
        //チャージ完了し、攻撃ボタンを離したときチャージ攻撃を呼び出す
        else if (chargeTime >= maxChargeTime && chargeAttack)
        {
            ischarging = false;
            chargeTime = 0;
            pState.chargeAttacking = true;
            chargeEffect.GetComponent<VisualEffect>().SendEvent("Stop");
            StartCoroutine(ChargeAttack());
        }
        //チャージ不完了で攻撃ボタンを離したらタイマーをリセットする
        else if (!charging)
        {
            ischarging = false;
            chargeEffect.GetComponent<VisualEffect>().SendEvent("Stop");
            chargeTime = 0;
        }
    }

    //チャージ攻撃を実行する非同期処理
    IEnumerator ChargeAttack()
    {
        audioSource.pitch = 1.2f;
        audioSource.PlayOneShot(attackSound);
        rb.velocity = Vector2.zero;
        Hit(SideAttackTransform, SideAttackArea, 1 * damage);
        anim.SetBool("Attacking", true);
        anim.SetFloat("Blend", 0);
        anim.SetTrigger("AttackTrigger");
        pState.counter = true;
        timeSinceAttack = comboResetTime - 0.5f;
        StartCoroutine(
            SetVFX(
                slashEffect,
                effecttransform,
                500,
                new Vector3(0, 0.48f, 0),
                new Vector3(30, 0, 0),
                new Vector3(-2.5f, 2, 2)
            )
        );
        yield return new WaitForSeconds(0.2f);
        pState.counter = false;
        yield return new WaitForSeconds(0.55f);
        audioSource.pitch = 0.8f;
        audioSource.PlayOneShot(attackSound);
        StartCoroutine(
            SetVFX(
                slashEffect,
                effecttransform,
                500,
                new Vector3(0, -1.25f, 0),
                new Vector3(pState.lookingRight ? 150 : 30, 90, -90),
                new Vector3(-2f, 2, 2)
            )
        );
        Hit(SideAttackTransform, SideAttackArea, 4 * damage);
        yield return new WaitForSeconds(0.03f);
        StartCoroutine(
            SetVFX(
                impactEffect,
                effecttransform,
                500,
                new Vector3(pState.lookingRight ? 4 : -4, -2.22f, 0),
                new Vector3(0, 0, 0),
                new Vector3(2, 2, 2)
            )
        );
        yield return new WaitForSeconds(0.22f);
        pState.chargeAttacking = false;
        anim.SetBool("Attacking", false);
        currentAttack = AttackState.ChargeCombo1;
    }

    IEnumerator ChargeCombo1()
    {
        timeSinceAttack = comboResetTime - 0.5f;
        pState.chargeAttacking = true;
        anim.SetTrigger("AttackTrigger");
        anim.SetBool("Attacking", true);

        yield return new WaitForSeconds(0.83f);
        audioSource.pitch = 1.3f;
        audioSource.PlayOneShot(attackSound);
        StartCoroutine(
            SetVFX(
                stabEffect,
                effecttransform,
                500,
                new Vector3(pState.lookingRight ? 1 : -1, 0, 0),
                new Vector3(0, 0, 0),
                new Vector3(-2, 2, 2)
            )
        );
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
        StartCoroutine(
            SetVFX(
                slashEffect,
                effecttransform,
                0,
                new Vector3(0, 0.48f, 0),
                new Vector3(30, 0, 0),
                new Vector3(-2.5f, 2, 2)
            )
        );
        audioSource.pitch = 1.1f;
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
        audioSource.pitch = 1.5f;
        audioSource.PlayOneShot(attackSound);
        StartCoroutine(
            SetVFX(
                slashEffect,
                effecttransform,
                500,
                new Vector3(0, 0.9f, 0),
                new Vector3(-10, 0, pState.lookingRight ? -20 : 20),
                new Vector3(-3, 2, 2)
            )
        );
        Hit(SideAttackTransform, SideAttackArea, 5 * damage);
        yield return new WaitForSeconds(0.246f);
        pState.chargeAttacking = false;
        anim.SetBool("Attacking", false);
        currentAttack = AttackState.None;
        timeSinceAttack = 0;
    }


    //攻撃の判定、判定内にいる攻撃可能なオブジェのダメージ処理を呼ぶ

    //TODO: 使役されるクラスに依存しているためinterfaceによる依存性の逆転が必要
    private void Hit(Transform _attackTransform, Vector2 _attackArea, float _damage)
    {
        //長方形の当たり判定を生成し、範囲にあるオブジェを全て取得する
        Collider2D[] objectToHit = Physics2D.OverlapBoxAll(
            _attackTransform.position,
            _attackArea,
            0,
            attackableLayer
        );
        List<Enemy> hitEnemies = new List<Enemy>();

        //当たり判定にあるオブジェクトから、Enemyクラスをリストに格納し
        //敵のダメージ処理を呼ぶ
        for (int i = 0; i < objectToHit.Length; i++)
        {
            Enemy e = objectToHit[i].GetComponent<Enemy>();
            if (e && !hitEnemies.Contains(e))
            {
                StartCoroutine(
                    SetVFX(
                        hitEffect,
                        effecttransform,
                        500,
                        new Vector3(pState.lookingRight ? 3f : -3f, 1f, 0),
                        new Vector3(0, 0, 0),
                        new Vector3(2f, 2, 2)
                    )
                );

                e.EnemyHit(_damage);
                hitEnemies.Add(e);
            }
        }
    }

    //プレイヤーのエフェクトを生成する
    IEnumerator SetVFX(
        GameObject _effect,
        Transform _position,
        int _order,
        Vector3 _offset,
        Vector3 _angle,
        Vector3 _scale
    )
    {
        _effect = Instantiate(_effect, _position);
        _effect.transform.eulerAngles = _angle;
        _effect.transform.localScale = _scale;
        _effect.transform.position += _offset;
        Renderer vfxRenderer = _effect.GetComponent<Renderer>();
        vfxRenderer.sortingOrder = _order;
        yield return null;
    }

    //毎フレーム実行
    //ジャストガードのタイムのカウントとリセットを行う
    void Parry()
    {
        //ジャストガードはクールタイム明けに使用可能、
        // ジャストガードを成功させた際クールタイム関係なく再使用可能
        if (parry && (parriedTime > parryCoolTime || parrytoken))
        {
            parrytoken = false;
            rb.velocity = new Vector2(0, rb.velocity.y);
            if (!Grounded())
                pState.recoilingY = true;
            StartCoroutine(
                SetVFX(
                    parryEffect,
                    effecttransform,
                    500,
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0),
                    new Vector3(3, 3, 3)
                )
            );
            parriedTime = 0f;
            Parring = true;
            pState.parry = true;
            anim.SetTrigger("Guard");
        }
        else if (parriedTime >= parryCoolTime)
        {
            Parring = false;
        }
        //ジャストガードはリターンの大きい完全成功(parry)とリターンの少ない妥協判定(graze)の2段階ある
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

        parriedTime = Mathf.Clamp(parriedTime + Time.deltaTime, 0f, 2f);
    }

    //難易度調整の際、GUIから操作される

    //TODO: GUIと相互依存関係となっているためMV(R)Pパターンで解消する必要がある
    public void CasualMode(bool _onOff)
    {
        isCasualMode = _onOff;
        parryGrace = _onOff ? 0.3f : 0.1f;
        grazeGrace = _onOff ? 0f : 0.1f;
    }


    //敵から攻撃された際呼び出される
    //ダメージを受けるかの可否を判定する
    //ダメージを与えるものがEnemyでない（弾丸など）場合を想定してEnemyをnull許容している

    //TODO: 使役されるクラスに依存し、攻撃対象が増えることに対応することが難しいためinterfaceによる依存性の逆転が必要
    public void TakeDamege(float _damage, bool _enemyDirection, bool _parryable, Enemy? e)
    {
        if (pState.alive)
        {
            // ジャストガード時の処理
            // プレイヤーのチャージ攻撃ゲージの増加
            // 敵の気絶値を増加

            //依存性の逆転を行うべき処理
            if (
                (pState.parry || pState.graze)
                && (_enemyDirection == !pState.lookingRight)
                && e != null
                && _parryable
            )
            {
                if (pState.parry)
                {
                    DeviceManager.Instance.StartShakeController(0.2f, 1, 0.1f);
                    Health = Mathf.Clamp(Health + 1, 0, prevhealth);
                    audioSource.pitch = 1.5f;
                    audioSource.PlayOneShot(parrySound);
                    StartCoroutine(
                        SetVFX(
                            justParryImpactEffect,
                            effecttransform,
                            500,
                            new Vector3(pState.lookingRight ? 3f : -3f, 1f, 0),
                            new Vector3(0, 0, 0),
                            new Vector3(2f, 2, 2)
                        )
                    );
                }
                else
                {
                    DeviceManager.Instance.StartShakeController(0, 0.7f, 0.1f);
                    Health = Mathf.Clamp(Health - 2, 1, prevhealth);
                    audioSource.pitch = 1f;
                    audioSource.PlayOneShot(grazeSound);
                    StartCoroutine(
                        SetVFX(
                            parryImpactEffect,
                            effecttransform,
                            500,
                            new Vector3(pState.lookingRight ? 3f : -3f, 1f, 0),
                            new Vector3(0, 0, 0),
                            new Vector3(2f, 2, 2)
                        )
                    );
                }
                //
                parrytoken = true;
                Mana += pState.parry ? 0.4f : 0.1f;

                e.ChargeStun(pState.parry);
                pState.parry = false;
                pState.graze = false;
            }
            //敵の攻撃に合わせてチャージ攻撃をしたときの処理
            //敵の気絶値が最大のとき、敵を気絶させる
            else if (pState.counter && _enemyDirection == !pState.lookingRight && e != null)
            {
                e.toStun();
            }

            //ダメージを与えたものが敵でない場合の処理
            //ジャストガード時の気絶値の変動、チャージ攻撃のカウンターの処理の削除を行っている
            else if (
                (pState.parry || pState.graze)
                && (_enemyDirection == !pState.lookingRight)
                && e == null
                && _parryable
            )
            {
                if (pState.parry)
                {
                    audioSource.pitch = 1.5f;
                    audioSource.PlayOneShot(parrySound);
                }
                else
                {
                    audioSource.pitch = 1f;
                    audioSource.PlayOneShot(grazeSound);
                }
                Mana += pState.parry ? 0.4f : 0.1f;
                parrytoken = true;
                pState.parry = false;
                pState.graze = false;
            }
            // ガード失敗時、プレイヤーが被ダメージした場合の処理
            else
            {
                DeviceManager.Instance.StartShakeController(0.5f, 0.5f, 0.3f);
                audioSource.PlayOneShot(damageSound);
                prevhealth = Health;
                Health -= isCasualMode ? maxDamage : _damage;
                StopAllCoroutines();
                canDash = true;

                // ダメージ時、非同期処理を停止させる
                StopCoroutine(ChargeAttack());
                StopCoroutine(ChargeCombo1());
                StopCoroutine(ChargeCombo2());
                StopCoroutine(ChargeCombo3());
                if (Health > 0)
                    // ダメージを受けた際、次の敵の攻撃を連続して被弾しにくいように、
                    // ヒットストップの時間を設ける
                    HitStopTime(0.5f, 1, 0.3f);

                pState.chargeAttacking = false;
                pState.counter = false;
                Attacking = false;

                // 体力0かつ残機0のとき、死亡処理の呼び出し

                if (Health <= 0 && LifePoint <= 0)
                {
                    Health = 0;
                    rb.velocity = Vector2.zero;
                    StartCoroutine(Death());
                }
                // 体力0かつ残機0のとき、残基を減らし体力を最大にする処理の呼び出し
                else if (Health <= 0 && LifePoint > 0)
                {
                    StartCoroutine(Restore());
                }
                // 体力が残ってる場合、一時的な無敵時間を発生させる
                else
                {
                    StartCoroutine(StopTakingDamage());
                }
            }
        }
    }

    //被ダメージ時に呼び出し
    //ダメージ後の無敵時間の処理と被ダメージアニメーションを再生する非同期処理
    IEnumerator StopTakingDamage()
    {
        pState.invincible = true;
        pState.damaged = true;
        StartCoroutine(
            SetVFX(bloodSpurt, effecttransform, 0, Vector3.zero, Vector3.zero, new Vector3(3, 3, 3))
        );
        anim.SetTrigger("Damage");
        anim.SetBool("Damaging", true);
        yield return new WaitForSeconds(0.1f);
        pState.invincible = false;
        yield return new WaitForSeconds(0.4f);
        pState.damaged = false;
        anim.SetBool("Damaging", false);
    }

    //残機が残っている状態で体力がなくなったとき呼び出し
    //復活アニメーションを再生し、残基を減らし、体力を最大にする非同期処理
    IEnumerator Restore()
    {
        rb.velocity = Vector2.zero;
        pState.alive = false;
        deathCutScene.SetActive(true);
        anim.SetTrigger("Death");
        anim.SetBool("isDeath", true);
        HitStopTime(0.5f, 1f, 0.5f);
        StartCoroutine(
            SetVFX(
                bloodEffectalive,
                effecttransform,
                500,
                Vector3.zero,
                Vector3.zero,
                new Vector3(3, 3, 3)
            )
        );
        yield return new WaitForSeconds(0.5f);
        anim.SetFloat("Speed", -1);
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


    //毎フレーム実行
    //ヒットストップ等で遅くなったゲーム内時間を徐々に戻す処理
    void RestoreTimeScale()
    {
        //ゲーム内時間を戻す時に実行
        //毎フレームごとの時間と指定の係数を乗算することでスムーズにゲーム内時間を戻す
        if (restoreTime)
        {
            if (Time.timeScale < 1)
            {
                Time.timeScale = Mathf.Clamp(
                    Time.timeScale + Time.unscaledDeltaTime * restoreTimeSpeed,
                    0,
                    1
                );
            }
            //ゲーム内時間が１倍になったとき一回実行
            else
            {
                Time.timeScale = 1;
                restoreTime = false;
            }
        }
    }

    //ヒットストップが発生したときのみ呼ばれる
    //ヒットストップでゲームスピードを_newTimeScale倍し、_delay秒後、_restoreSpeed秒かけてゲーム内時間を1倍に戻す
    public void HitStopTime(float _newTimeScale, float _restoreSpeed, float _delay)
    {
        restoreTimeSpeed = _restoreSpeed;
        Time.timeScale = _newTimeScale;
        if (_delay > 0)
        {
            //ヒットストップの最中呼び出されたらリセットする
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

    //プレイヤー死亡時に実行される非同期処理

    // TODO: GUIと相互依存関係となっているためMV(R)Pパターンで解消する必要がある
    IEnumerator Death()
    {
        rb.velocity = Vector2.zero;
        pState.alive = false;
        anim.SetTrigger("Death");
        anim.SetBool("isDeath", true);

        //相互依存関係の処理
        deathCutScene.SetActive(true);
        //
        StartCoroutine(
            SetVFX(
                bloodEffectdeath,
                effecttransform,
                500,
                Vector3.zero,
                Vector3.zero,
                new Vector3(3, 3, 3)
            )
        );
        HitStopTime(0.5f, 1f, 0.5f);
        //相互依存関係の処理
        TutorialManager.Instance.FadeOutTutorialAll();
        //
        yield return new WaitForSeconds(0.5f);

        //相互依存関係の処理
        StartCoroutine(UIManager.Instance.ActivateDeathScreen());
        //
        yield return new WaitForSeconds(3f);
        anim.SetBool("isDeath", false);
        //相互依存関係の処理
        deathCutScene.SetActive(false);
        //
    }

    //GUIから呼び出す
    //プレイヤーのパラメーターを初期化しリスポーンさせる

    //TODO: GUIと相互依存しているためMV(R)Pパターンで解消する必要がある
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
            //相互依存箇所
            StartCoroutine(UIManager.Instance.DeactivateDeathScreen());
            //
        }
    }

    //体力を管理するプロパティ
    
    //TODO: GUIと相互依存しているためMV(R)Pパターンで解消する必要がある
    public float Health
    {
        get { return health; }
        set
        {
            if (health != value)
            {
                health = Mathf.Clamp(value, 0, maxHealth);

                //体力の増減をGUIの表示と連動させる処理

                //相互依存箇所
                if (onHealthChangedCallback != null)
                {
                    onHealthChangedCallback.Invoke();
                }
                //
            }
        }
    }

    //残機を管理するプロパティ
    
    //TODO: GUIと相互依存しているためMV(R)Pパターンで解消する必要がある
    public int LifePoint
    {
        get { return lifePoint; }
        set
        {
            if (lifePoint != value)
            {
                lifePoint = Mathf.Clamp(value, 0, maxLifePoint);


                //残機の増減をGUIの表示と連動させる処理

                //相互依存箇所
                if (onLifePointChangedCallback != null)
                {
                    onLifePointChangedCallback.Invoke();
                }
                //
            }
        }
    }

    //攻撃ゲージを管理するプロパティ
    
    //TODO: GUIと相互依存しているためMV(R)Pパターンで解消する必要がある
    public float Mana
    {
        get { return mana; }
        set
        {
            if (mana != value)
            {
                mana = Mathf.Clamp(value, 0, manaMax);
                //相互依存箇所
                if (onManaChangedCallback != null)
                {
                    onManaChangedCallback.Invoke();
                }
                //
            }
        }
    }

    //毎フレーム実行
    //接地判定を行う
    public bool Grounded()
    {
        //接地判定用の3次元座標からレイを飛ばし、地面レイヤーにあたった際接地していると判定する
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

    //毎フレーム実行
    //ジャンプの可否の判定に基づき、プレイヤーをジャンプさせるためのY軸方向の速度の変更を行う
    void Jump()
    {
        //ジャンプ入力が有効、地面から離れてからcoyoteTime以内でジャンプ入力した場合
        //ジャンプ状態にし、プレイヤーをジャンプさせる
        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0 && !pState.jumping)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce * playertimescale);
            pState.jumping = true;
        }
        //プレイヤーが空中にいるときのみ判定、空中ジャンプ可能か判定し、可能なら追加でジャンプする
        else if (!Grounded() && airJumpCounter < maxAirJumps && jumpInput.WasPressedThisFrame())
        {
            pState.jumping = true;
            airJumpCounter++;
            rb.velocity = new Vector3(rb.velocity.x, jumpForce * playertimescale);
        }

        //ジャンプの上昇中、ジャンプボタンを離したとき上昇を停止する
        if (jumpInput.WasReleasedThisFrame() && rb.velocity.y > 3)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            pState.jumping = false;
        }

    }

    //毎フレーム実行
    //ジャンプの判定に利用するタイマーの増加をリセットを行う
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

    //毎フレーム実行
    //壁に隣接しているか判定を行う
    private bool Walled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.01f, wallLayer);
    }


    //毎フレーム実行
    //壁滑りを行っているか判定を行い、壁滑り時の剛体の速度を設定する
    void WallSlide()
    {

        //プレイヤーが壁と隣接時、壁に向かって方向キーを入力時に実行
        if (
            Walled()
            && !Grounded()
            && ((xAxis >= 0 && pState.lookingRight) || (xAxis <= 0 && !pState.lookingRight))
        )
        {
            //壁滑りを有効化し、低速で落下する
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

    //毎フレーム実行

    void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            //壁キックの向き、プレイヤーが左を向いてるときは右向き(1)に
            wallJumpingDirection = !pState.lookingRight ? 1 : -1;

            CancelInvoke(nameof(StopWallJumping));
        }
        //壁滑り中ジャンプ入力時実行
        if (jumpInput.WasPressedThisFrame() && isWallSliding)
        {
            isWallJumping = true;

            //壁キックの物理演算の処理、斜め上方向へ速度をかける
            rb.velocity = new Vector2(
                wallJumpingPower.x * wallJumpingDirection,
                wallJumpingPower.y
            );
            dashed = false;
            airJumpCounter = 1;
            pState.lookingRight = !pState.lookingRight;
            // ジャンプアニメーションをY軸方向の速度に応じて変更
            float JumpBlend = Mathf.Clamp(rb.velocity.y / (2 * jumpForce), -0.5f, 0.5f);
            anim.SetFloat("Blend", (pState.lookingRight ? 6.5f : 4.5f) - JumpBlend);


            //壁キック後wallJumpingDuration秒、壁キックの物理演算を行うため操作不能にする
            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }


    //壁キック後wallJumpingDuration秒後呼び出される
    //壁キック状態を解除し操作可能にする
    void StopWallJumping()
    {
        isWallJumping = false;
    }
}
