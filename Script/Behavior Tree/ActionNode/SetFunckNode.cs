using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFunckNode : ActionNode
{
    public string funcName;
    protected override void OnStart()
    {
       
    }
    protected override void OnStop()
    {
        
    }
    protected override State OnUpdate()
    {
        context.enemy.coroutinesignal = true;
        context.enemy.coroutinename = funcName;
        return State.Success;
    }
}
