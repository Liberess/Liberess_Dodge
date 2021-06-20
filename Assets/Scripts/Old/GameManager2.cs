using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager2 : MonoBehaviour
{
    public static GameManager2 Instance;

    public Text timeTxt;
    public Text recordTxt;
    public Text scoreTxt;

    public GameObject gameOverPanel;
    public GameObject gameStopPanel;

    public GameObject level;

    public List<GameObject> itemList = new List<GameObject>();
    public List<GameObject> spawnerList = new List<GameObject>();

    private int itemCounter;
    private int spawnCounter;

    private bool isCreate;

    public bool isPlay;
    public bool isGameOver;

    private int buildIndex;

    private int score;
    private int hour;
    private int minute;
    private int second;
    private float surviveTime;

    private void Awake() => Instance = this;

    private void Start()
    {
        buildIndex = SceneManager.GetActiveScene().buildIndex;

        if (buildIndex > 0) //Main 이상
        {
            score = 0;
            surviveTime = 0f;
            itemCounter = 0;
            spawnCounter = 0;

            isCreate = true;

            Time.timeScale = 1;

            isPlay = true;
            isGameOver = false;

            scoreTxt.text = "점수 : " + score;

            StartCoroutine(ScoreAdd());

            StartCoroutine(CreateHpItem());
            StartCoroutine(CreateBulletSpawner());
            StartCoroutine(DestroyItems());
        }
    }

    private void Update()
    {
        if (buildIndex > 0)
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

    IEnumerator CreateHpItem()
    {
        yield return new WaitForSeconds(5f);

        Vector3 randPos = new Vector3(Random.Range(-23f, 23f), 0f, Random.Range(-23f, 23f));

        if(isCreate)
        {
            if (itemCounter < 5)
            {
                GameObject hpItem = Instantiate(Resources.Load<GameObject>("HpItem"), randPos, Quaternion.identity);
                hpItem.transform.parent = level.transform;
                hpItem.transform.localPosition = randPos;
                itemList.Add(hpItem);
                itemCounter++;

                StartCoroutine(CreateHpItem());
            }
            else
            {
                yield return null;
            }
        }
        else
        {
            isCreate = true;
        }
    }

    IEnumerator DestroyItems()
    {
        yield return new WaitForSeconds(10f);

        isCreate = false;

        foreach(GameObject item in itemList)
        {
            Destroy(item);
        }

        itemList.Clear();

        foreach (GameObject bulletSpawner in spawnerList)
        {
            bulletSpawner.GetComponent<BulletSpawner>().isMove = true;
        }

        Invoke("SpawnerStop", 2f);

        StartCoroutine(DestroyItems());
    }

    private void SpawnerStop()
    {
        foreach (GameObject bulletSpawner in spawnerList)
        {
            bulletSpawner.GetComponent<BulletSpawner>().isMove = false;
        }
    }

    IEnumerator CreateBulletSpawner()
    {
        yield return new WaitForSeconds(5f);

        Vector3 randPos = new Vector3(Random.Range(-23f, 23f), 0f, Random.Range(-23f, 23f));

        if (spawnCounter < 10)
        {
            GameObject bulletSpawner = Instantiate(Resources.Load<GameObject>("BulletSpawner"), randPos, Quaternion.identity);
            //bulletSpawners[spawnCounter] = Instantiate(Resources.Load<GameObject>("BulletSpawner"), bulletSpawnerPos[spawnCounter], Quaternion.identity);
            //bulletSpawners[spawnCounter].transform.parent = level.transform;
            //bulletSpawners[spawnCounter].transform.localPosition = bulletSpawnerPos[spawnCounter];
            //level.GetComponent<Rotator>().rotationSpeed += 15f;
            spawnerList.Add(bulletSpawner);
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