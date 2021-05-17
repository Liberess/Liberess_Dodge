using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float moveSpeed = 8f;

    Rigidbody rigid;

    private void Start()
    {
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

        if (collision.CompareTag("Player"))
        {
            //collision.GetComponent<PlayerCtrl>().hp -= 1;

            PlayerCtrl playerCtrl = collision.GetComponent<PlayerCtrl>();

            if(playerCtrl != null)
            {
                playerCtrl.Hit();
                //playerCtrl.Die();
                Destroy(gameObject);
            }
        }
    }
}