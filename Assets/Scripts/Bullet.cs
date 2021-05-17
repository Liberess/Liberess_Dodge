using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool isPlayer;
    private float moveSpeed = 8f;

    Rigidbody rigid;

    private void Start()
    {
        isPlayer = false;
        rigid = GetComponent<Rigidbody>();

        Destroy(gameObject, 3f);
    }

    private void Update()
    {
        //transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        rigid.velocity = transform.forward * moveSpeed;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }

        if (collision.CompareTag("Player") && !isPlayer)
        {
            PlayerCtrl playerCtrl = collision.GetComponent<PlayerCtrl>();

            if(playerCtrl != null)
            {
                playerCtrl.Hit();
                Destroy(gameObject);
            }
        }

        if(collision.CompareTag("Enemy") && isPlayer)
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.Hit();
                Destroy(gameObject);
            }
        }    
    }
}