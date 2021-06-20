using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHpBar : MonoBehaviour
{
    public static MonsterHpBar Instance;

    public Transform target;
    public Slider hpBar;
    public Slider backHpBar;

    public float maxHp;
    public float currentHp;

    private bool hit = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (currentHp <= 0)
            Destroy(gameObject);

        transform.position = target.position;
        hpBar.value = Mathf.Lerp(hpBar.value, currentHp / maxHp, Time.deltaTime * 5f);

        if(hit)
        {
            backHpBar.value = Mathf.Lerp(backHpBar.value, hpBar.value, Time.deltaTime * 10f);

            if (hpBar.value >= backHpBar.value - 0.01f)
            {
                hit = false;
                backHpBar.value = hpBar.value;
            }
        }
    }

    public IEnumerator Hit()
    {
        yield return new WaitForSeconds(0.5f);

        hit = true;
    }
}