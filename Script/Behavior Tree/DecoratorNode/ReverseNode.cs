using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseNode : DecoratorNode
{
    protected override void OnStart()
    {

    }
    protected override void OnStop()
    {

    }
    protected override State OnUpdate()
    {
       switch(child.Update())
       {
            case State.Running:
                return State.Running;
            case State.Success:
                return State.Failure;
            case State.Failure:
                return State.Success;
       }
       return State.Running;
    }
}
