using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    public static PlayerCtrl Instance;
    public HpBar hpBar;

    public GameObject gun;
    //public GameObject cam;
    //public Transform cameraArm;

    private bool isMove;
    private bool isNoDmg;
    [SerializeField]
    private bool isMiddle;

    private float moveSpeed = 8f;

    private float shotTime;
    private float shotDelayTime = 0.2f;

    Vector3 pointToLook;

    Animator anim;
    Rigidbody rigid;

    private int m_health;

    public int health
    {
        get
        {
            return m_health;
        }

        set
        {
            if (value <= 0)
            {
                Die();
            }

            m_health = value;
            //HpBar.Instance.SetHp(m_health);
            hpBar.SetHp(m_health);
        }
    }

    private void Awake() => Instance = this;

    private void Start()
    {
        isMove = true;
        isNoDmg = false;
        isMiddle = false;

        health = 10;
        m_health = 10;

        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Shot();
    }

    private void FixedUpdate()
    {
        Move();
        MousePocus();
    }

    private void Move()
    {
        if (isMove)
        {
            float inputX = Input.GetAxis("Horizontal");
            float inputZ = Input.GetAxis("Vertical");

            if(inputX == 0 && inputZ == 0)
            {
                anim.SetBool("isMove", false);
            }
            else
            {
                anim.SetBool("isMove", true);
            }

            anim.SetFloat("inputX", inputX);
            anim.SetFloat("inputZ", inputZ);

            rigid.velocity = new Vector3(inputX * moveSpeed, 0f, inputZ * moveSpeed);
        }
    }

    private void MousePocus()
    {
        if(!isMiddle)
        {
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            Plane groupPlane = new Plane(Vector3.up, Vector3.zero);

            float rayLength;

            if (groupPlane.Raycast(camRay, out rayLength))
            {
                pointToLook = camRay.GetPoint(rayLength);
                transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
            }
        }
        else
        {
            Debug.Log("미들 펄스");
        }
    }

    public void GetHeal(int _heal)
    {
        health += _heal;

        if(health > 10)
        {
            health = 10;
        }

        HpBar.Instance.SetHp(health);
    }

    public void Hit(int _damage)
    {
        if (isNoDmg == false) //무적 상태가 아니라면
        {
            health -= _damage;

            GameObject blood = Instantiate(Resources.Load<GameObject>("Particles/Blood"), transform.position, Quaternion.identity);

            Destroy(blood, 1f);

            StartCoroutine(NoDamage());
        }
    }

    private void Shot()
    {
        if(shotTime >= shotDelayTime)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                anim.SetTrigger("doShot");

                shotTime = 0;

                //GameObject bullet = Instantiate(Resources.Load<GameObject>("PlayerBullet"), gun.transform.position, Quaternion.identity);
                //bullet.transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
            }
        }
        else
        {
            shotTime += Time.deltaTime;
        }
    }

    IEnumerator NoDamage()
    {
        isNoDmg = true;

        yield return new WaitForSeconds(1f);

        isNoDmg = false;
    }

    private void Die()
    {
        isMove = false;

        if(GameManager.Instance != null)
        {
            GameManager.Instance.EndGame();
        }

        if (GameManager2.Instance != null)
        {
            GameManager2.Instance.EndGame();
        }
    }

    public void MiddleOn()
    {
        isMiddle = true;
    }

    public void MiddleOff()
    {
        isMiddle = false;
    }
}