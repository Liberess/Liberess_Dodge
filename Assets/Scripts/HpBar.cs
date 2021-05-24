using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public static HpBar Instance;

    public Image hpBar;

    private void Awake()
    {
        Instance = this;
    }

    public void SetHp(int _hp)
    {
        hpBar.fillAmount = (float)_hp / 10f;
    }
}