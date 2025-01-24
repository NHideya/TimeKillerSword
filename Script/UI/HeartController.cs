using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HeartController : MonoBehaviour
{
    [SerializeField] float duration = 0.5f;
    [SerializeField] Image healthGauge;

    [SerializeField] Image redhealthGauge;
    PlayerController player;
    private GameObject[] heartContainers;
    private Image[] heartFills;

    private GameObject[] manaContainers;

    private Image[] manaFills;
    public Transform heartsParent;
    public GameObject heartContainerPrefab;

    public Transform manaParent;
    public GameObject manaContainerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerController.Instance;
        heartContainers = new GameObject[PlayerController.Instance.maxLifePoint];
        manaContainers = new GameObject[PlayerController.Instance.manaMax];
        heartFills = new Image[PlayerController.Instance.maxLifePoint];
        manaFills = new Image[PlayerController.Instance.manaMax];

        PlayerController.Instance.onLifePointChangedCallback += UpdateHeartHUD;
        PlayerController.Instance.onHealthChangedCallback += SetHPGauge;
        PlayerController.Instance.onManaChangedCallback += UpdateManaHUD;
        InstantiateHeartContainers();
        InstantiateManaContainers();
        UpdateHeartHUD();
        UpdateManaHUD();
        SetHPGauge();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetHeartContainers()
    {
        for (int i = 0; i < heartContainers.Length; i++)
        {
            if (i < PlayerController.Instance.maxLifePoint)
            {
                heartContainers[i].SetActive(true);
            }
            else
            {
                heartContainers[i].SetActive(false);
            }
        }
    }
    void SetManaContainers()
    {
        for (int i = 0; i < manaContainers.Length; i++)
        {
            if (i < PlayerController.Instance.manaMax)
            {
                manaContainers[i].SetActive(true);
            }
            else
            {
                manaContainers[i].SetActive(false);
            }
        }
    }

    void SetFilledHearts()
    {
        for (int i = 0; i < heartFills.Length; i++)
        {
            if (i < PlayerController.Instance.LifePoint)
            {
                heartFills[i].fillAmount = 1;
            }
            else
            {
                heartFills[i].fillAmount = 0;
            }
        }
    }

    void SetFilledMana()
    {
        
        for (int i = 0; i < manaFills.Length; i++)
        {
            if (i < Mathf.Floor(PlayerController.Instance.Mana))
            {
               
                manaFills[i].fillAmount = 1;
                manaFills[i].color = new Color(255,255,255,1f);
            }
            else if(i == Mathf.Floor(PlayerController.Instance.Mana))
            {
                
                manaFills[i].DOFillAmount(PlayerController.Instance.Mana - Mathf.Floor(PlayerController.Instance.Mana),0.2f);
                manaFills[i].color = new Color(255,255,255,0.5f);
            }
            else if(i > Mathf.Floor(PlayerController.Instance.Mana))
            {
                
                manaFills[i].fillAmount = 0;
            }
        }
    }

    void InstantiateHeartContainers()
    {
        for (int i = 0; i < PlayerController.Instance.maxLifePoint; i++)
        {
            GameObject temp = Instantiate(heartContainerPrefab);
            temp.transform.SetParent(heartsParent, false);
            heartContainers[i] = temp;
            heartFills[i] = temp.transform.Find("LifePoint").GetComponent<Image>();
        }
    }

    void InstantiateManaContainers()
    {
        for (int i = 0; i < PlayerController.Instance.manaMax; i++)
        {
            GameObject temp = Instantiate(manaContainerPrefab);
            temp.transform.SetParent(manaParent, false);
            manaContainers[i] = temp;
            manaFills[i] = temp.transform.Find("Mana").GetComponent<Image>();
        }
    }

    void UpdateHeartHUD()
    {
        SetHeartContainers();
        SetFilledHearts();
    }

    void UpdateManaHUD()
    {
        SetManaContainers();
        SetFilledMana();
    }

    void SetHPGauge()
    {
        healthGauge.DOFillAmount(PlayerController.Instance.Health/ PlayerController.Instance.maxHealth,duration);
        redhealthGauge.DOFillAmount(PlayerController.Instance.prevhealth / PlayerController.Instance.maxHealth,0);
       
    }
}
