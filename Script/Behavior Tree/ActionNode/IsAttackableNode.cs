using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsAttackableNode : ActionNode
{
    protected override void OnStart()
    {
        
    }
    protected override void OnStop()
    {
        
    }
    protected override State OnUpdate()
    {
        if(context.state.Attackable) return State.Success;
        else return State.Failure;
    }
}
