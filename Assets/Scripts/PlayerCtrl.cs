using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    public static PlayerCtrl Instance;

    public GameObject gun;
    public GameObject cam;
    public Transform cameraArm;

    private bool isMove;
    private bool isNoDmg;

    private float moveSpeed = 12f;

    private float shotTime;
    private float shotDelayTime = 1f;

    Rigidbody rigid;
    MeshRenderer mesh;

    public Material hitMaterial;
    private Material originMaterial;

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
            HpBar.Instance.SetHp(m_health);
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        isMove = true;
        isNoDmg = false;

        health = 10;
        m_health = 10;

        rigid = GetComponent<Rigidbody>();
        mesh = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        LookAround();
        Move2();
        Shot();
    }

    private void Move()
    {
        if (isMove)
        {
            float inputX = Input.GetAxis("Horizontal");
            float inputZ = Input.GetAxis("Vertical");

            //rigid.velocity = new Vector3(inputX * moveSpeed, 0f, inputZ * moveSpeed);

            //rigid.velocity = new Vector3(cam.transform.forward.x * moveSpeed, 0f, cam.transform.forward.z * moveSpeed);
            rigid.velocity = new Vector3(cam.transform.forward.x * moveSpeed, 0f, cam.transform.forward.y * moveSpeed);

            //rigid.AddForce(cam.transform.forward * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);
        }

        /* Vector3 dir = cam.transform.localRotation * Vector3.forward;
        transform.localRotation = cam.transform.localRotation;
        transform.localRotation = new Quaternion(0, transform.localRotation.y, 0, transform.localRotation.w);
        gameObject.transform.Translate(dir * 0.1f * Time.deltaTime); */
    }

    private void LookAround()
    {
        if(GameManager.Instance.isPlay)
        {
            Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            Vector3 camAngle = cameraArm.rotation.eulerAngles;

            float x = camAngle.x - mouseDelta.y;

            if (x < 180f)
            {
                x = Mathf.Clamp(x, -1f, 70f);
            }
            else
            {
                x = Mathf.Clamp(x, 335f, 361f);
            }

            cameraArm.rotation = Quaternion.Euler(camAngle.x - mouseDelta.y, camAngle.y + mouseDelta.x, camAngle.z);
        }
    }

    private void Move2()
    {
        if(GameManager.Instance.isPlay)
        {
            Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            bool isMove = moveInput.magnitude != 0;

            if (isMove)
            {
                Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
                Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
                Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

                //transform.forward = moveDir;
                //transform.forward = lookForward;
                //transform.position += moveDir * Time.deltaTime * 5f;
                rigid.velocity = new Vector3(moveDir.x * moveSpeed, 0f, moveDir.z * moveSpeed);
            }
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

    public void Hit()
    {
        if (isNoDmg == false) //무적 상태가 아니라면
        {
            //--health;
            health -= 3;

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
                shotTime = 0;

                GameObject bullet = Instantiate(Resources.Load<GameObject>("PlayerBullet"), gun.transform.position, Quaternion.identity);
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

        originMaterial = mesh.material;
        mesh.material = hitMaterial;

        yield return new WaitForSeconds(1f);

        mesh.material = originMaterial;

        isNoDmg = false;
    }

    private void Die()
    {
        isMove = false;

        GameManager.Instance.EndGame();
    }
}