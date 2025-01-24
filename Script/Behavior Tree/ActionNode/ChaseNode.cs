using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ChaseNode : ActionNode
{
    public float chasespeed;
    protected override void OnStart()
    {
        
    }
    protected override void OnStop()
    {
       
    }
    protected override State OnUpdate()
    {
       
        if (context.state.canwalk && Mathf.Abs(context.player.transform.position.y - context.transform.position.y) < 10)
        {
            context.physics.velocity = new Vector2(context.player.transform.position.x - context.transform.position.x > 0 ? chasespeed : - chasespeed , context.physics.velocity.y);
            return State.Success;
        }
        else 
        {
            context.physics.velocity =new Vector2(0f,context.physics.velocity.y);
            context.state.EncounterPlayer = false;
            return State.Failure;
        }
    }
}
