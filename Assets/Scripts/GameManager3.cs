using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager3 : MonoBehaviour
{
    public static GameManager3 Instance;

    public GameObject gameOverPanel;
    public GameObject gameStopPanel;

    public GameObject level;

    public List<GameObject> enemyList = new List<GameObject>();

    public bool isPlay;
    public bool isGameOver;

    private int buildIndex;

    private void Awake() => Instance = this;

    private void Start()
    {
        buildIndex = SceneManager.GetActiveScene().buildIndex;

        if (buildIndex > 0) //Main 이상
        {
            Time.timeScale = 1;

            isPlay = true;
            isGameOver = false;
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
        }
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

    public void GoMain()
    {
        SceneManager.LoadScene("Main");
    }

    public void EndGame()
    {
        StopAllCoroutines();

        isGameOver = true;

        gameOverPanel.SetActive(true);
    }

    public void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}