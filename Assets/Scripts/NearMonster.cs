using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NearMonster : Monster
{
    private int damage = 1;

    private void Start()
    {
        isAlive = true;

        damage *= GameManager3.Instance.buildIndex + (int)monsType;

        attackRateMin = 1f;
        attackRateMax = 3f;

        attackRate = 3f;
        attackTimer = 0f;

        int temp = 10 * GameManager3.Instance.buildIndex * (int)monsType;

        int randHp = (int)Random.Range(temp * 0.5f, temp * 1.5f);
        m_health = randHp;

        hpBar.maxHp = health;
        hpBar.currentHp = health;

        target = FindObjectOfType<PlayerCtrl>().transform;
        navAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (DataManager.Instance.gameData.health <= 0)
            isAttack = false;

        attackTimer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if(isAlive)
        {
            float distance = Vector3.Distance(target.position, transform.position);
            transform.LookAt(target.position);

            if (distance >= 2f)
            {
                navAgent.destination = target.position;
                navAgent.isStopped = false;
                anim.SetBool("isMove", true);
            }
            else
            {
                Attack();
                isAttack = true;
                navAgent.isStopped = true;
                anim.SetBool("isMove", false);
            }
        }
    }

    private void MyAttack()
    {
        Debug.Log(damage);
        PlayerCtrl.Instance.Hit(damage);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("PlayerBullet"))
        {
            Hit();
            StartCoroutine(hpBar.Hit());
        }
    }
}