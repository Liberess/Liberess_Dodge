using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager3 : MonoBehaviour
{
    public static GameManager3 Instance;

    [SerializeField]
    private Transform[] spawnPos;

    public Text stageTxt;
    public Text monsTxt;

    public GameObject gameOverPanel;
    public GameObject gameStopPanel;

    public List<GameObject> monsterList = new List<GameObject>();

    public bool isPlay;
    public bool isGameOver;
    public bool isBoss;

    public int buildIndex;
    private int monsCount;

    public string[] monsName = { "Monsters/SlimeMob", "Monsters/TurtleShellMob", "Monsters/BeholderMob", "Monsters/ChestMob" };

    private void Awake() => Instance = this;

    private void Start()
    {
        buildIndex = SceneManager.GetActiveScene().buildIndex;

        if (buildIndex > 0 && buildIndex < 6) //Main 초과 End 미만
        {
            Time.timeScale = 1;

            isPlay = false;
            isBoss = false;
            isGameOver = false;

            if (buildIndex < 5)
            {
                StartCoroutine(MonsterSpawn());
                StartCoroutine(CreateCureItem());
            }
            else
            {
                isBoss = true;
            }

            stageTxt.text = "스테이지 " + buildIndex;

            if(buildIndex == 1)
            {
                DataManager.Instance.ResetGame();
            }

            PlayerHpBar.Instance.SetHp();
        }

        if(buildIndex == 6) //End
        {
            Cursor.visible = true;
        }

        SoundManager.Instance.PlayBGM(SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        if (buildIndex > 0 && buildIndex < 6)
        {
            if (!isGameOver)
            {
                if (Input.GetButtonDown("Cancel"))
                {
                    if (gameStopPanel.activeSelf)
                    {
                        Cursor.visible = false;
                        gameStopPanel.SetActive(false);
                    }
                    else
                    {
                        Cursor.visible = true;
                        gameStopPanel.SetActive(true);
                    }
                }

                monsTxt.text = "남은 적 : " + (monsterList.Count / 2) + "/" + monsCount;
            }
            else
            {
                if(Input.anyKeyDown)
                {
                    Cursor.visible = true;
                    Michsky.LSS.LoadingScreenManager.Instance.LoadScene("Main");
                }
            }
        }
        else
        {
            if (Input.GetButtonDown("Cancel"))
            {
                if (gameStopPanel.activeSelf)
                {
                    Cursor.visible = false;
                    gameStopPanel.SetActive(false);
                }
                else
                {
                    Cursor.visible = true;
                    gameStopPanel.SetActive(true);
                }
            }
        }
    }

    IEnumerator MonsterSpawn()
    {
        for(int i = 0; i < spawnPos.Length; i++)
        {
            GameObject particle = Instantiate(Resources.Load<GameObject>("Particles/Spawn"), spawnPos[i].position, Quaternion.identity);
            Destroy(particle, 1.5f);

            yield return new WaitForSeconds(0.5f - (buildIndex * 0.15f));

            int rand = 0;

            switch(buildIndex)
            {
                case 1: rand = Random.Range(0, 1); break; //Slime
                case 2: rand = Random.Range(0, 2); break; //Slime ~ Turtle
                case 3: rand = Random.Range(0, 3); break; //Slime ~ Beholder
                case 4: rand = Random.Range(0, 4); break; //Slime ~ Chest
            }

            Instantiate(Resources.Load(monsName[rand]), spawnPos[i].position, Quaternion.identity);

            yield return new WaitForSeconds(0.5f);
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Monster");

        for (int i = 0; i < enemies.Length; i++)
            monsterList.Add(enemies[i]);

        monsCount = monsterList.Count / 2;
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
        SoundManager.Instance.PlaySFX("Clear");

        yield return new WaitForSeconds(2.5f);

        if(buildIndex < 5)
        {
            GameObject.Find("GameCanvas").transform.Find("Skill Group").gameObject.SetActive(true);
        }
        else
        {
            CamFollow.Instance.isPlayer = true;
            PlayerCtrl.Instance.isMove = true;
            PlayerCtrl.Instance.isShot = true;
        }
    }

    IEnumerator CreateCureItem()
    {
        yield return new WaitForSeconds(10f);

        Item item = FindObjectOfType<Item>();

        if (item == null)
        {
            int rand = Random.Range(0, 101);

            if (rand <= 30)
            {
                int randPos = Random.Range(0, spawnPos.Length);
                Instantiate(Resources.Load<GameObject>("CureItem"),
                    new Vector3(spawnPos[randPos].position.x, spawnPos[randPos].position.y + 1f), Quaternion.identity);
            }
        }

        StartCoroutine(CreateCureItem());
    }

    public void NextStage()
    {
        if(buildIndex < 5)
        {
            Michsky.LSS.LoadingScreenManager.Instance.LoadScene("Level_" + (buildIndex + 1));
        }
        else
        {
            Michsky.LSS.LoadingScreenManager.Instance.LoadScene("End");
        }
    }

    public void EndGame()
    {
        StopAllCoroutines();

        isGameOver = true;

        gameOverPanel.SetActive(true);
    }

    public void QuitGame() => Application.Quit();
}