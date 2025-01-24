using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemyteritory : MonoBehaviour
{
   [SerializeField] private Enemy[] allEnemy;
    [SerializeField] GameObject gate;
    [SerializeField] GameObject BossHp;
    [SerializeField] bool isBossArea;

    private bool bossBeated;

    private void Start()
    {
        if(isBossArea)
        {
           
             GameObject Canvas = GameObject.Find("Canvas");
             BossHp = Canvas.transform.Find("BossHPGauge").gameObject;
            
        }
         if(gate != null)
            {
                gate.SetActive(false);
            }
        if(isBossArea && SaveData.Instance.bossBeated)
            {
                gameObject.SetActive(false);
            }
    }

    private void OnTriggerEnter2D(Collider2D _other)
   {
    if(_other.CompareTag("Player"))
    {
        for(int i =0;i < allEnemy.Length ; i++)
        {
            if(allEnemy[i] != null)
            {
                allEnemy[i].eState.playerinteritory = true;
                allEnemy[i].rb.isKinematic = false;
            }
            if(gate != null && allEnemy[0] != null)
            {
                gate.SetActive(true);
            }
            if(BossHp != null && allEnemy[0] != null)
            {
                BossHp.SetActive(true);
            }
            
        }
    }
   }
    private void OnTriggerExit2D(Collider2D _other)
    {
        if (_other.CompareTag("Player"))
        {
            for (int i = 0; i < allEnemy.Length; i++)
            {
                if (allEnemy[i] != null)
                {
                    allEnemy[i].eState.playerinteritory = false;
                    allEnemy[i].rb.isKinematic = true;
                    allEnemy[i].rb.velocity = Vector2.zero;
                    allEnemy[i].eState.EncounterPlayer = false;
                }
            }
             if(BossHp != null)
            {
                BossHp.SetActive(false);
            }
        }
        
    }
}
