using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : CompositeNode
{
    int current;
    protected override void OnStart()
    {
        current = 0;
    }
    protected override void OnStop()
    {

    }
    protected override State OnUpdate()
    {
        var child = children[current];
        switch (child.Update())
        {
            case State.Running:
                return State.Running;
            case State.Failure:
                current++;
                break;
            case State.Success:
                current++;
                return State.Success;
        }
        return (current == children.Count) ? State.Failure : State.Running;
    }

}
