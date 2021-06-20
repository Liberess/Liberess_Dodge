using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShotMonster : Monster
{
    public Transform bulletPos;

    private void Start()
    {
        isAlive = true;

        attackRateMin = 1f;
        attackRateMax = 3f;

        attackTimer = 0f;

        int temp = (9 * GameManager3.Instance.buildIndex * (int)monsType) / 3;

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
        if (isAlive && GameManager3.Instance.isPlay)
        {
            float distance = Vector3.Distance(target.position, transform.position);
            transform.LookAt(target.position);

            if (distance >= 6f)
            {
                navAgent.destination = target.position;
                navAgent.isStopped = false;
                anim.SetBool("isMove", true);
            }
            else
            {
                isAttack = true;
                navAgent.isStopped = true;
                anim.SetBool("isMove", false);
                Attack();
            }
        }
        else
        {
            isAttack = false;
            navAgent.isStopped = true;
        }
    }

    private void Shooting()
    {
        SoundManager.Instance.PlaySFX("ShotAttack");
        GameObject bullet = Instantiate(Resources.Load<GameObject>("EnemyBullet"), bulletPos.position, transform.rotation);
        bullet.GetComponent<MonsterBullet>().isBoss = false;
        bullet.transform.LookAt(target);
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