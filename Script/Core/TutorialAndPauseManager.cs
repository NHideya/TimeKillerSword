using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAndPauseManager : MonoBehaviour
{
    public static TutorialAndPauseManager Instance;
    [SerializeField] FadeUI counterUI;
    [SerializeField] FadeUI strongAttackUI;

    private bool done;

    public FadeUI current;
    

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    

    public void EnableTutorial(TutorialType _tutorialType)
    {
        switch(_tutorialType)
        {
            case TutorialType.Counter :
                counterUI.gameObject.SetActive(true);
                current = counterUI;
                counterUI.FadeUIIn(0.2f);
                break;
            case TutorialType.StrongAttack :
                strongAttackUI.gameObject.SetActive(true);
                current = strongAttackUI;
                strongAttackUI.FadeUIIn(0.2f);
                break;
        }
    }
    
}
