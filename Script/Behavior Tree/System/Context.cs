using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Context
{
    public GameObject gameObject;
    public Transform ptransform;
    public Enemy enemy;
    public PlayerController player;
    public Transform transform;
    public Animator animator;
    public Rigidbody2D physics;

    public EnemyState state;

    public Transform patroltargetA;
    public Transform patroltargetB;





    public static Context CreateFromGameObject(GameObject gameObject)
    {
        Context context = new Context();
        context.gameObject = gameObject;
        context.transform = gameObject.transform;
        context.animator = gameObject.GetComponent<Animator>();
        context.enemy = gameObject.GetComponent<Enemy>();
        context.physics = gameObject.GetComponent<Rigidbody2D>();
        context.state = gameObject.GetComponent<EnemyState>();
        

        return context;
    }


}
