using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum MonsType
{
    Slime = 1,
    TurtleShell,
    Beholder,
    Chest
}

public abstract class Monster : MonoBehaviour
{
    [SerializeField]
    protected MonsterHpBar hpBar;
    //protected HpBar hpBar;

    [SerializeField]
    protected MonsType monsType;

    protected bool isAlive;
    protected bool isAttack;

    [SerializeField]
    protected int m_health;
    public int health
    {
        get
        {
            return m_health;
        }

        set
        {
            if (health > 0)
            {
                m_health = value;
            }
        }
    }

    protected float attackRateMin = 1f;
    protected float attackRateMax = 3f;

    protected float attackRate = 3f;
    protected float attackTimer = 0f;

    protected Animator anim;
    protected Rigidbody rigid;
    protected Transform target;
    protected NavMeshAgent navAgent;

    protected void Hit()
    {
        if (isAlive)
        {
            float randAtk = Random.Range(DataManager.Instance.gameData.atk * 0.5f, DataManager.Instance.gameData.atk * 2f);
            health -= (int)randAtk;
            hpBar.currentHp = health;
            //hpBar.SetHp(health);

            if (health > 0)
            {
                anim.SetTrigger("doHit");
                GameObject blood = Instantiate(Resources.Load<GameObject>("Particles/Blood"), transform.position, Quaternion.identity);
                Destroy(blood, 1f);
            }
            else
            {
                isAlive = false;
                navAgent.isStopped = true;
                rigid.velocity = Vector3.zero;
                gameObject.layer = 10;
                anim.SetTrigger("doDie");
                Invoke("Die", 1f);
            }
        }
    }

    protected void Attack()
    {
        if (isAttack)
        {
            if (attackTimer >= attackRate)
            {
                attackTimer = 0f;

                anim.SetTrigger("doAttack");

                attackRate = Random.Range(attackRateMin, attackRateMax);
            }
        }
    }

    protected void Die()
    {
        GameManager3.Instance.monsterList.Remove(transform.parent.gameObject);
        GameManager3.Instance.monsterList.Remove(gameObject);
        GameManager3.Instance.InspectMonsList();
        transform.parent.GetComponent<MonsterParent>().IsDestroy();
    }
}