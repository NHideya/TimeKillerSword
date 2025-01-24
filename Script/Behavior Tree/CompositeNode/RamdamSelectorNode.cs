using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamdamSelectorNode : CompositeNode
{
   private int current = -1;
    protected override void OnStart()
    {
        // ランダムに次の子ノードを選択
        if (children.Count > 0)
        {
            current = Random.Range(0, children.Count);
        }
    }

    protected override void OnStop()
    {
        current = -1;
    }

    protected override State OnUpdate()
    {
        if (children.Count == 0 || current == -1)
        {
            return State.Failure;
        }

        State childState = children[current].Update();

        if (childState == State.Success || childState == State.Failure)
        {
            return childState;
        }

        return State.Running;
    }
}
