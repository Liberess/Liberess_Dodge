using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    Rigidbody rigid;

    private void Awake() => rigid = GetComponent<Rigidbody>();

    protected override void Move() => rigid.velocity = transform.forward* moveSpeed;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }

        if (collision.CompareTag("Enemy"))
        {
            BulletSpawner enemy = collision.GetComponent<BulletSpawner>();

            if (enemy != null)
            {
                enemy.Hit(damage);
                Destroy(gameObject);
            }
        }

        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}