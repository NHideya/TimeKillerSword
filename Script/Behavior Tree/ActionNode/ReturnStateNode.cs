using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnStateNode : ActionNode
{
    public int _switch;


    protected override void OnStart()
    {
        // ノードが開始されたときの処理（必要に応じて）
    }

    protected override void OnStop()
    {
        // ノードが停止したときの処理（必要に応じて）
    }

    protected override State OnUpdate()
    {

        switch (_switch)
        {
            case 0:
                return context.state.EncounterPlayer ? State.Failure : State.Success;
            case 1:
                return context.state.Attackable ? State.Failure : State.Success;

        }
        return State.Failure;
    }
}
