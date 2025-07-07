//敵キャラクターの体力バーと気絶ゲージを管理するクラス

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class EnemyHpManager : MonoBehaviour
{
   [SerializeField] private Image healthImage;
   [SerializeField] private Image redHealthImage;
    [SerializeField] private Image StanImage;

    [SerializeField] private Image Timerimage;

    [SerializeField] float duration;
     [SerializeField] bool isBoss;

  
   private Enemy enemy;


  
    //初期化処理
    private void Start()
    {
        if(isBoss)
        {
        GameObject Canvas = GameObject.Find("Canvas");
        GameObject BossHPGauge = Canvas.transform.Find("BossHPGauge").gameObject;
        healthImage = BossHPGauge.transform.Find("HPGreen").gameObject.GetComponent<Image>();
        redHealthImage = BossHPGauge.transform.Find("HPRed").gameObject.GetComponent<Image>();
        StanImage = BossHPGauge.transform.Find("stun").gameObject.GetComponent<Image>();
        }
        enemy = GetComponentInParent<Enemy>();
        SetGaugeHP();
        SetGaugeStun();
        enemy.HPChangeCallBack += SetGaugeHP;
        enemy.StanChangeCallBack += SetGaugeStun;
        enemy.StanTimerCallBack += SetTimer;

    }
    
    //毎フレーム実行
   private void Update()
    {
        //GUIの抜きを一定にする処理
        if (!isBoss)
            transform.localScale = new Vector3(enemy.eState.lookingRight ? -1 : 1, 1, 1);
    } 
    
    //敵の体力に応じて体力バーを増減する処理
   public void SetGaugeHP()
    {
        healthImage.DOFillAmount(enemy.Health / enemy.maxhealth, duration).OnComplete(() =>
        {
            redHealthImage.DOFillAmount(enemy.Health / enemy.maxhealth, duration * 0.5f).SetDelay(0.5f);
        });
    }
    public void SetGaugeStun()
    {
        StanImage.DOFillAmount(enemy.Stun / 100, duration);
    }

    public void SetTimer()
    {
        Timerimage.fillAmount = 1 - enemy.sinceTimestun / enemy.stunTime;
    }
}
