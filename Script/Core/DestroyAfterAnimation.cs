using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{
    [SerializeField] float destroytime;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,destroytime);
    }

}
