using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager3 : MonoBehaviour
{
    public static GameManager3 Instance;

    public Text stageTxt;
    public Text monsTxt;

    public GameObject gameOverPanel;
    public GameObject gameStopPanel;

    public List<GameObject> monsterList = new List<GameObject>();

    public bool isPlay;
    public bool isGameOver;

    public int buildIndex;
    private int monsCount;

    private void Awake() => Instance = this;

    private void Start()
    {
        buildIndex = SceneManager.GetActiveScene().buildIndex;

        if (buildIndex > 0) //Main 이상
        {
            Time.timeScale = 1;

            isPlay = true;
            isGameOver = false;

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Monster");

            for (int i = 0; i < enemies.Length; i++)
                monsterList.Add(enemies[i]);

            monsCount = monsterList.Count / 2;

            stageTxt.text = "스테이지 " + buildIndex;

            PlayerHpBar.Instance.SetHp();
        }
    }

    private void Update()
    {
        if (buildIndex > 0)
        {
            if (!isGameOver)
            {
                if (Input.GetButtonDown("Cancel"))
                {
                    if (gameStopPanel.activeSelf)
                    {
                        isPlay = true;
                        gameStopPanel.SetActive(false);
                    }
                    else
                    {
                        isPlay = false;
                        gameStopPanel.SetActive(true);
                    }
                }

                monsTxt.text = "남은 적 : " + (monsterList.Count / 2) + "/" + monsCount;
            }
        }
    }

    public void InspectMonsList()
    {
        if(monsterList.Count == 0)
        {
            CamFollow.Instance.isPlayer = false;
            PlayerCtrl.Instance.isMove = false;
            PlayerCtrl.Instance.isShot = false;
            StartCoroutine(OpenDoor());
        }
    }

    IEnumerator OpenDoor()
    {
        yield return new WaitForSeconds(1f);

        GameObject.Find("Door").GetComponent<Animator>().SetTrigger("doOpen");

        yield return new WaitForSeconds(2.5f);

        GameObject.Find("GameCanvas").transform.Find("Skill Group").gameObject.SetActive(true);
    }

    public void NextStage()
    {
        Michsky.LSS.LoadingScreenManager.Instance.LoadScene("Level_" + (buildIndex + 1));
    }

    public void CreateHpItem(Vector2 pos) => Instantiate(Resources.Load<GameObject>("HpItem"), pos, Quaternion.identity);

    /* private void TimeUnitChange(Text txt, string str, float time)
    {
        minute = (int)time / 60;
        hour = minute / 60;
        second = (int)time % 60;
        minute %= 60;

        if (hour > 0 && minute > 0) //시간 분 초
        {
            txt.text = str + hour + "시간 " + minute + "분 " + second + "초";
        }
        else if (hour > 0 && minute <= 0) //시간 초
        {
            txt.text = str + hour + "시간 " + second + "초";
        }
        else if (hour <= 0 && minute > 0) //분 초
        {
            txt.text = str + minute + "분 " + second + "초";
        }
        else if (hour <= 0 && minute <= 0) //초
        {
            txt.text = str + second + "초";
        }
    } */

    public void EndGame()
    {
        StopAllCoroutines();

        isGameOver = true;

        gameOverPanel.SetActive(true);
    }

    public void QuitGame() => Application.Quit();
}