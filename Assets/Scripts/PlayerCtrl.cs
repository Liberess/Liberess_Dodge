using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    public static PlayerCtrl Instance;
    private DataManager dataMgr;
    //public HpBar hpBar;

    public GameObject gun;

    private bool isAlive;
    public bool isMove;
    public bool isShot;
    
    private bool isNoDmg;
    private bool isMiddle;

    private float shotTime;

    Vector3 pointToLook;

    Animator anim;
    Rigidbody rigid;

    private void Awake()
    {
        Instance = this;
        dataMgr = DataManager.Instance;
    }

    private void Start()
    {
        isAlive = true;
        isMove = true;
        isShot = true;
        isNoDmg = false;
        isMiddle = false;

        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Shot();
        PlayerHpBar.Instance.currentHp = DataManager.Instance.gameData.health;
    }

    private void FixedUpdate()
    {
        Move();
        MousePocus();
    }

    private void Move()
    {
        if (isMove && isAlive && GameManager3.Instance.isPlay)
        {
            float inputX = Input.GetAxis("Horizontal");
            float inputZ = Input.GetAxis("Vertical");

            if (inputX == 0 && inputZ == 0)
            {
                anim.SetBool("isMove", false);
            }
            else
            {
                anim.SetBool("isMove", true);
            }

            anim.SetFloat("inputX", inputX);
            anim.SetFloat("inputZ", inputZ);

            rigid.velocity = new Vector3(inputX * dataMgr.gameData.moveSpeed, 0f, inputZ * dataMgr.gameData.moveSpeed);
        }
    }

    private void MousePocus()
    {
        if (!isMiddle && isAlive && GameManager3.Instance.isPlay && !GameManager3.Instance.gameStopPanel.activeSelf)
        {
            float rayLength;
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane groupPlane = new Plane(Vector3.up, Vector3.zero);

            if (groupPlane.Raycast(camRay, out rayLength))
            {
                pointToLook = camRay.GetPoint(rayLength);
                transform.LookAt(pointToLook);
            }
        }
    }

    public void GetHeal(int _heal)
    {
        SoundManager.Instance.PlaySFX("Cure");

        GameObject cure = Instantiate(Resources.Load<GameObject>("Particles/Cure"), transform.position, Quaternion.identity);
        Destroy(cure, 1f);

        dataMgr.gameData.health += _heal;

        if (dataMgr.gameData.health >= dataMgr.gameData.maxHealth)
        {
            dataMgr.gameData.health = dataMgr.gameData.maxHealth;
        }

        //HpBar.Instance.SetHp(health);
        PlayerHpBar.Instance.currentHp = dataMgr.gameData.health;
    }

    public void Hit(int _damage)
    {
        if (isNoDmg == false && isAlive) //무적 상태가 아니라면
        {
            if (dataMgr.gameData.health - _damage <= 0)
                Die();
            else
                dataMgr.gameData.health -= _damage;

            GameObject blood = Instantiate(Resources.Load<GameObject>("Particles/Blood"), transform.position, Quaternion.identity);

            Destroy(blood, 1f);

            SoundManager.Instance.PlaySFX("PlayerHit");
        }
    }

    private void Shot()
    {
        if (shotTime >= dataMgr.gameData.shotDelayTime && isAlive && isShot && GameManager3.Instance.isPlay)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                shotTime = 0;

                if (dataMgr.gameData.atkCount >= 1)
                {
                    StopCoroutine(DoubleShot());
                    StartCoroutine(DoubleShot());
                }
                else
                {
                    SoundManager.Instance.PlaySFX("PlayerShot");
                    GetComponent<DemoShooting>().Shot();
                }
            }
        }
        else
        {
            shotTime += Time.deltaTime;
        }
    }

    IEnumerator DoubleShot()
    {
        int i = 0;

        while(i <= dataMgr.gameData.atkCount)
        {
            i++;
            SoundManager.Instance.PlaySFX("PlayerShot");
            GetComponent<DemoShooting>().Shot();
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator NoDamage()
    {
        isNoDmg = true;

        yield return new WaitForSeconds(0.5f);

        isNoDmg = false;
    }

    private void Die()
    {
        isAlive = false;
        isMove = false;

        dataMgr.gameData.health = 0f;

        anim.SetTrigger("doDie");
        SoundManager.Instance.PlaySFX("PlayerDie");

        GameManager3.Instance.EndGame();
    }

    #region MousePocusMiddle
    public void MiddleOn()
    {
        isMiddle = true;
    }

    public void MiddleOff()
    {
        isMiddle = false;
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            GameManager3.Instance.NextStage();
        }
    }
}