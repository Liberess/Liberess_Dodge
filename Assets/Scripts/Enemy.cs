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

    public void Hit()
    {
        --hp;

        GameObject blood = Instantiate(Resources.Load<GameObject>("Particles/Blood"), transform.position, Quaternion.identity);

        Destroy(blood, 1f);
    }
}