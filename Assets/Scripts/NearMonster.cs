using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NearMonster : Monster
{
    private int damage = 3;

    private void Start()
    {
        isAlive = true;

        damage *= GameManager3.Instance.buildIndex + (int)monsType;

        attackRateMin = 1f;
        attackRateMax = 3f;

        attackTimer = 0f;

        int temp = 7 * GameManager3.Instance.buildIndex * (int)monsType;

        int randHp = (int)Random.Range(temp * 0.5f, temp * 1.2f);
        m_health = randHp;

        hpBar.maxHp = health;
        hpBar.currentHp = health;

        attackRate = Random.Range(attackRateMin, attackRateMax);

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
        if(isAlive && GameManager3.Instance.isPlay)
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
        else
        {
            isAttack = false;
            navAgent.isStopped = true;
        }
    }

    private void MyAttack()
    {
        SoundManager.Instance.PlaySFX("NearAttack");
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