using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpBar : MonoBehaviour
{
    public static PlayerHpBar Instance;

    public GameObject hpLine;
    public Slider hpBar;
    public Text hpTxt;

    [SerializeField]
    private float unitHp = 20f;
    public float maxHp;
    public float currentHp;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        hpBar.value = currentHp;
        hpTxt.text = currentHp.ToString();
    }

    public void SetHp()
    {
        hpBar.maxValue = DataManager.Instance.gameData.maxHealth;
        maxHp = DataManager.Instance.gameData.maxHealth;
        currentHp = DataManager.Instance.gameData.health;

        float scaleX = (100f / unitHp) / (maxHp / unitHp);
        hpLine.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(false);
        foreach(Transform child in hpLine.transform)
        {
            child.gameObject.transform.localScale = new Vector3(scaleX, 1f, 1f);
        }
        hpLine.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(true);
    }
}