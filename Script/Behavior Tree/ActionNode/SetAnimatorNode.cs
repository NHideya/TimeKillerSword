using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnimatorNode : ActionNode
{
    public bool animbool;
    public bool onOff;

    public bool trigger;
    public string animName;
    protected override void OnStart()
    {
       
    }
    protected override void OnStop()
    {
        
    }
    protected override State OnUpdate()
    {
      if(animbool)
      {
        context.animator.SetBool(animName,onOff);
      }
      else if(trigger)
      {
        context.animator.SetTrigger(animName);
      }
      else
      {
        return State.Failure;
      }
        return State.Success;
    }
}
