using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp;

    private void Awake()
    {
        hp = 3;
    }

    private void Update()
    {
        if(hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}