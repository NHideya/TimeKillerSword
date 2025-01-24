using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrolnode : ActionNode
{
   public float waitTime;

   public float patrolspeed;

    protected override void OnStart()
    {
       
    }
    protected override void OnStop()
    {
      
    }
    protected override State OnUpdate()
    {
        if (context.patroltargetA == null || context.patroltargetB == null) return State.Failure;

            
        if (Mathf.Abs(context.enemy.target.position.x - context.transform.position.x) < 1f || !context.state.canwalk)
        {
            if (context.enemy.TimeSincePatrol >= waitTime)
            {
                context.transform.localScale = new Vector3(-context.transform.localScale.x, context.transform.localScale.y, context.transform.localScale.z);
                context.enemy.TimeSincePatrol = 0;
                context.enemy.target = context.enemy.target == context.patroltargetA ? context.patroltargetB : context.patroltargetA;
                //context.state.lookingRight = context.enemy.target == context.patroltargetA;
                context.physics.velocity = new Vector2((context.enemy.target == context.patroltargetA ? patrolspeed : -patrolspeed), context.physics.velocity.y);
            }
            else
            {
                context.physics.velocity =new Vector2(0f,context.physics.velocity.y);
                context.enemy.TimeSincePatrol += Time.deltaTime;
            }
        }
        else
        {
                context.physics.velocity = new Vector2((context.enemy.target == context.patroltargetA ? patrolspeed : -patrolspeed), context.physics.velocity.y);
            }
        return State.Success;
    }
}
