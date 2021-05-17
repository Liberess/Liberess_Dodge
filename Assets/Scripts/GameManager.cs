using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Text timeTxt;
    public Text recordTxt;
    public Text scoreTxt;

    public GameObject gameOverPanel;
    public GameObject gameStopPanel;

    public GameObject level;
    //private GameObject[] bulletSpawners = new GameObject[4];
    //public Vector3[] bulletSpawnerPos = new Vector3[12];
    public List<GameObject> bulletSpawnerList = new List<GameObject>();
    //private Vector3[] bulletSpawnerPos = new Vector3[4];
    private int spawnCounter;

    public bool isPlay;
    public bool isGameOver;

    private int buildIndex;

    private int score;
    private int hour;
    private int minute;
    private int second;
    private float surviveTime;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        buildIndex = SceneManager.GetActiveScene().buildIndex;

        if(buildIndex > 0) //Main 이상
        {
            score = 0;
            surviveTime = 0f;
            spawnCounter = 0;

            Time.timeScale = 1;

            isPlay = true;
            isGameOver = false;

            scoreTxt.text = "점수 : " + score;

            StartCoroutine(ScoreAdd());
        }

        //BulletSpawner들의 위치 설정
        //bulletSpawnerPos[0] = new Vector3(-8f, 1f, 8f);
        //bulletSpawnerPos[1] = new Vector3(8f, 1f, 8f);
        //bulletSpawnerPos[2] = new Vector3(8f, 1f, -8f);
        //bulletSpawnerPos[3] = new Vector3(-8f, 1f, -8f);

        StartCoroutine(CreateBulletSpawner());
    }

    private void Update()
    {
        if(buildIndex > 0)
        {
            if (!isGameOver)
            {
                surviveTime += Time.deltaTime;

                TimeUnitChange(timeTxt, "생존 : ", surviveTime);

                if (Input.GetButtonDown("Cancel"))
                {
                    if (gameStopPanel.activeSelf)
                    {
                        isPlay = true;
                        Time.timeScale = 1;
                        gameStopPanel.SetActive(false);
                    }
                    else
                    {
                        isPlay = false;
                        Time.timeScale = 0;
                        gameStopPanel.SetActive(true);
                    }
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    SceneManager.LoadScene(buildIndex);
                }
            }
        }
    }

    IEnumerator CreateBulletSpawner()
    {
        yield return new WaitForSeconds(5f);

        float posX = Random.Range(-25, 25);
        float posZ = Random.Range(-25, 25);

        if(spawnCounter < 10)
        {
            GameObject bulletSpawner = Instantiate(Resources.Load<GameObject>("BulletSpawner"), new Vector3(posX, 1, posZ), Quaternion.identity);
            //bulletSpawners[spawnCounter] = Instantiate(Resources.Load<GameObject>("BulletSpawner"), bulletSpawnerPos[spawnCounter], Quaternion.identity);
            //bulletSpawners[spawnCounter].transform.parent = level.transform;
            //bulletSpawners[spawnCounter].transform.localPosition = bulletSpawnerPos[spawnCounter];
            bulletSpawnerList.Add(bulletSpawner);
            //level.GetComponent<Rotator>().rotationSpeed += 15f;
            spawnCounter++;

            StartCoroutine(CreateBulletSpawner());
        }
        else
        {
            yield return null;
        }
    }

    private void TimeUnitChange(Text txt, string str, float time)
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
    }

    IEnumerator ScoreAdd()
    {
        ++score;

        scoreTxt.text = "점수 : " + score;

        yield return new WaitForSeconds(2f);

        StartCoroutine(ScoreAdd());
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void GoMain()
    {
        SceneManager.LoadScene("Main");
    }

    public void EndGame()
    {
        StopAllCoroutines();

        isGameOver = true;

        gameOverPanel.SetActive(true);

        float bestTime = PlayerPrefs.GetFloat("BestTime");

        if (surviveTime > bestTime)
        {
            bestTime = surviveTime;
            PlayerPrefs.SetFloat("BestTime", bestTime);
        }

        TimeUnitChange(recordTxt, "최대 생존 : ", bestTime);

        //recordTxt.text = "최대 생존 : " + (int)bestTime;
    }

    public void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        float bestTime = PlayerPrefs.GetFloat("BestTime");

        if (surviveTime > bestTime)
        {
            bestTime = surviveTime;
            PlayerPrefs.SetFloat("BestTime", bestTime);
        }

        Application.Quit();
    }
}