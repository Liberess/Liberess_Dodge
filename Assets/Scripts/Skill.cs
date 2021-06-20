using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SkillType
{
    Atk = 0,
    ShotSpeed,
    MoveSpeed,
    Health,
    Double
}

public class Skill : MonoBehaviour
{
    private SkillType type;

    private string[] txts = { "공격력", "공격 속도", "이동 속도", "최대 체력", "더블 공격" };

    private void Awake()
    {
        int rand = Random.Range(0, 5);

        switch(rand)
        {
            case 0: type = SkillType.Atk; break;
            case 1: type = SkillType.ShotSpeed; break;
            case 2: type = SkillType.MoveSpeed; break;
            case 3: type = SkillType.Health; break;
            case 4: type = SkillType.Double; break;
        }

        transform.Find("Text").GetComponent<Text>().text = txts[(int)type];
    }

    public void SkillChoice()
    {
        switch(type)
        {
            case SkillType.Atk: DataManager.Instance.gameData.atk += 2f; break;
            case SkillType.ShotSpeed: DataManager.Instance.gameData.shotDelayTime -= 0.5f; break;
            case SkillType.MoveSpeed: DataManager.Instance.gameData.moveSpeed += 0.5f; break;
            case SkillType.Health: DataManager.Instance.gameData.maxHealth += 100f; break;
            case SkillType.Double: DataManager.Instance.gameData.atkCount++; break;
        }

        CamFollow.Instance.isPlayer = true;
        PlayerCtrl.Instance.isMove = true;
        PlayerCtrl.Instance.isShot = true;
        PlayerCtrl.Instance.LevelUp();

        GameObject.Find("GameCanvas").transform.Find("Skill Group").gameObject.SetActive(false);
    }
}