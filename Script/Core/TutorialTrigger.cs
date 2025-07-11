//チュートリアルを出現させる条件を管理するクラス

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
   

   private bool onOff = false;
   private bool done;
   [SerializeField] TutorialType tutorialType;
   
   private void Start()
   {
     CheackTutorialDone(tutorialType);
     if(done)
     gameObject.SetActive(false);
   }

   //チュートリアル出現範囲内にプレイヤーが新入した時に実行
   //登録されたチュートリアルタイプをもとに表示させる処理を呼び出す
   private void OnTriggerStay2D(Collider2D _other)
   {
      if (!onOff && _other.CompareTag("Player"))
      {
         onOff = true;
         TutorialManager.Instance.FadeInTutorial(tutorialType);
      }
   }

   //チュートリアル出現範囲からプレイヤーが出た時に実行
   //セーブデータにチュートリアル出現済みの記録をつける処理を呼び出す
   //出現済みと記録されたチュートリアルは今後出現しない
     private void OnTriggerExit2D(Collider2D _other)
   {
      if (onOff && _other.CompareTag("Player"))
      {
         onOff = false;
         SaveTutorialDone(tutorialType);

         TutorialManager.Instance.FadeOutTutorial(tutorialType);
         gameObject.SetActive(false);
      }
   }

   private void SaveTutorialDone(TutorialType _tutorialType)
   {
    switch(_tutorialType)
    {
        case TutorialType.Walk :
            SaveData.Instance.walkTutorialDone = true;
            break;
         case TutorialType.Jump :
            SaveData.Instance.jumpTutorialDone = true;
            break;
        case TutorialType.WallJump :
            SaveData.Instance.wallJumpTutorialDone = true;
            break;
         case TutorialType.Attack :
            SaveData.Instance.attackTutorialDone = true;
            break;
         case TutorialType.Charge :
            SaveData.Instance.chargeAttackTutorialDone = true;
            break;
         case TutorialType.Guard :
            SaveData.Instance.guardTutorialDone = true;
            break;
         case TutorialType.Counter :
            SaveData.Instance.counterTutorialDone = true;
            break;
         case TutorialType.StrongAttack:
            SaveData.Instance.strongAttackTutorialDone = true;
            break;
    }
   }

    private void CheackTutorialDone(TutorialType _tutorialType)
   {
    switch(_tutorialType)
    {
        case TutorialType.Walk :
            done = SaveData.Instance.walkTutorialDone;
            break;
         case TutorialType.Jump :
            done =SaveData.Instance.jumpTutorialDone ;
            break;
        case TutorialType.WallJump :
            done = SaveData.Instance.wallJumpTutorialDone;
            break;
         case TutorialType.Attack :
            done = SaveData.Instance.attackTutorialDone;
            break;
         case TutorialType.Charge :
            done = SaveData.Instance.chargeAttackTutorialDone;
            break;
         case TutorialType.Guard :
            done = SaveData.Instance.guardTutorialDone;
            break;
         case TutorialType.Counter :
            done = SaveData.Instance.counterTutorialDone;
            break;
         case TutorialType.StrongAttack:
            done = SaveData.Instance.strongAttackTutorialDone;
            break;
    }
   }
}
