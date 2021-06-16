using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Type
{
    Player = 0,
    Enemy
}

public class HpBar : MonoBehaviour
{
    public static HpBar Instance;
    public Type type;

    public Transform target;
    public Image hpBar;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(type == Type.Enemy)
            transform.position = new Vector3(target.position.x, target.position.y + 2f, target.position.z);
    }

    public void SetHp(int _hp)
    {
        hpBar.fillAmount = (float)_hp / 10f;
    }
}