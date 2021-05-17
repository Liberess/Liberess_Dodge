using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    public static PlayerCtrl Instance;

    public Slider hpSlider;

    private bool isMove;
    private bool isNoDmg;

    private float moveSpeed = 12f;

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
            if(value <= 0)
            {
                Die();
            }

            m_health = value;
            hpSlider.value = m_health;
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

        health = 5;
        hpSlider.maxValue = health;

        rigid = GetComponent<Rigidbody>();
        mesh = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if(isMove)
        {
            float inputX = Input.GetAxis("Horizontal");
            float inputZ = Input.GetAxis("Vertical");

            rigid.velocity = new Vector3(inputX * moveSpeed, 0f, inputZ * moveSpeed);
        }
    }

    public void Hit()
    {
        if(isNoDmg == false) //무적 상태가 아니라면
        {
            --health;

            GameObject blood = Instantiate(Resources.Load<GameObject>("Particles/Blood"), transform.position, Quaternion.identity);

            Destroy(blood, 1f);

            StartCoroutine(NoDamage());
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

    public void Die()
    {
        isMove = false;

        GameManager.Instance.EndGame();
    }
}