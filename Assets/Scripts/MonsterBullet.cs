using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBullet : Bullet
{
    public bool isBoss = false;

    Rigidbody rigid;

    private void Awake() => rigid = GetComponent<Rigidbody>();

    private void Start()
    {
        damage *= GameManager3.Instance.buildIndex + 4;

        if(!isBoss)
            Destroy(gameObject, 3f);
    }

    protected override void Move()
    {
        if(!isBoss)
            rigid.velocity = transform.forward * moveSpeed;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Wall") || collision.CompareTag("Land"))
        {
            Destroy(gameObject);
        }

        if (collision.CompareTag("Player"))
        {
            PlayerCtrl player = collision.GetComponent<PlayerCtrl>();

            if (player != null)
            {
                player.Hit(damage);
                Destroy(gameObject);
            }
        }

        /* if(collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        } */
    }
}