using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum Pos
{
    PosA = 0,
    PosB,
    PosC
}

public enum Pattern
{
    Basic = 0,
    Multi,
    Circle,
    Spawn
}

public class BossMonster : Monster
{
    public static BossMonster Instance;

    public Pattern pattern;
    public Transform[] bulletPos;

    [SerializeField]
    private Transform cirPos;

    private bool isCircle;
    private bool isSpawn;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        isAlive = true;
        isSpawn = false;
        isCircle = false;

        attackRateMin = 0.5f;
        attackRateMax = 2f;

        attackTimer = 0f;

        int temp = 1500;

        int randHp = (int)Random.Range(temp * 0.5f, temp * 1.5f);
        m_health = randHp;

        hpBar.maxHp = health;
        hpBar.currentHp = health;

        attackRate = Random.Range(attackRateMin, attackRateMax);

        target = FindObjectOfType<PlayerCtrl>().transform;
        navAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();

        GameManager3.Instance.monsterList.Add(gameObject);
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

            if (distance >= 17f)
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

    public IEnumerator AttackAI()
    {
        yield return new WaitForSeconds(0.1f);

        int randAction = Random.Range(0, 4);
        pattern = (Pattern)randAction;

        switch(pattern)
        {
            case Pattern.Basic:
                anim.SetTrigger("doAttack");
                break;
            case Pattern.Multi:
                anim.SetTrigger("doMulti");
                StartCoroutine(MultiShot());
                break;
            case Pattern.Circle:
                StartCoroutine(CircleShot());
                break;
            case Pattern.Spawn:
                StartCoroutine(MonsSpawn());
                break;
        }
    }

    private void Shooting()
    {
        SoundManager.Instance.PlaySFX("ShotAttack");
        GameObject bullet = Instantiate(Resources.Load<GameObject>("BossBullet"), bulletPos[(int)Pos.PosA].position, transform.rotation);
        bullet.GetComponent<MonsterBullet>().damage = 5;
        bullet.GetComponent<MonsterBullet>().moveSpeed = 25f;
        bullet.transform.LookAt(target);
    }

    public IEnumerator MultiShot()
    {
        isAttack = false;

        for (int i = 0; i < bulletPos.Length; i++)
        {
            yield return new WaitForSeconds(0.15f);

            SoundManager.Instance.PlaySFX("ShotAttack");
            GameObject bullet = Instantiate(Resources.Load<GameObject>("BossBullet"), bulletPos[i].position, transform.rotation);
            bullet.GetComponent<MonsterBullet>().damage = 5;
            bullet.GetComponent<MonsterBullet>().moveSpeed = 30f;
            bullet.transform.LookAt(target);
        }

        isAttack = true;
    }

    public IEnumerator CircleShot()
    {
        if(!isCircle)
        {
            isCircle = true;

            float speed = 12f;
            float oneShot = 20f;

            for (int i = 0; i < oneShot; i++)
            {
                GameObject bullet = Instantiate(Resources.Load<GameObject>("BossBullet"), cirPos.position, Quaternion.identity);
                bullet.GetComponent<MonsterBullet>().isBoss = true;
                Vector3 dirVec = new Vector3(Mathf.Sin(Mathf.PI * 2 * i / oneShot), 0f, Mathf.Cos(Mathf.PI * i * 2 / oneShot));
                bullet.GetComponent<Rigidbody>().AddForce(dirVec.normalized * speed, ForceMode.Impulse);
                Vector3 rotVec = Vector3.forward * 360 * i / oneShot + Vector3.forward * 90f;
                bullet.transform.Rotate(rotVec);
            }

            float rand = Random.Range(5f, 15f);
            yield return new WaitForSeconds(rand);

            isCircle = false;
        }
    }

    public IEnumerator MonsSpawn()
    {
        if(!isSpawn)
        {
            isSpawn = true;

            int randNum = Random.Range(1, 4);
            int randPosX = Random.Range(-20, 20);
            int randPosZ = Random.Range(-30, 30);

            Vector3 randPos = new Vector3(randPosX, 0f, randPosZ);

            for (int i = 0; i < randNum; i++)
            {
                GameObject particle = Instantiate(Resources.Load<GameObject>("Particles/Spawn"), randPos, Quaternion.identity);
                Destroy(particle, 1.5f);

                yield return new WaitForSeconds(0.2f);

                int rand = Random.Range(0, 4);

                Instantiate(Resources.Load(GameManager3.Instance.monsName[rand]), randPos, Quaternion.identity);

                yield return new WaitForSeconds(0.2f);
            }

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Monster");

            for (int i = 0; i < enemies.Length; i++)
                GameManager3.Instance.monsterList.Add(enemies[i]);

            float randTime = Random.Range(8f, 20f);
            yield return new WaitForSeconds(randTime);

            isSpawn = false;
        }
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