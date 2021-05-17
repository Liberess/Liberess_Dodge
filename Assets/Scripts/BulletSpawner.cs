using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public static BulletSpawner Instance;

    private bool isShot;

    private float spawnRateMin = 0.5f;
    private float spawnRateMax = 3f;

    private Transform target;
    private float spawnRate = 0f;
    private float spawnTimer = 0f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        isShot = true;
        spawnTimer = 0f;

        spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        target = FindObjectOfType<PlayerCtrl>().transform;
    }

    private void Update()
    {
        if(PlayerCtrl.Instance.health <= 0)
        {
            isShot = false;
        }

        if(isShot)
        {
            if (spawnTimer >= spawnRate)
            {
                spawnTimer = 0f;

                GameObject bullet = Instantiate(Resources.Load<GameObject>("Bullet"), transform.position, transform.rotation);
                bullet.GetComponent<Bullet>().isPlayer = false;
                bullet.transform.LookAt(target);

                spawnRate = Random.Range(spawnRateMin, spawnRateMax);
            }
            else
            {
                spawnTimer += Time.deltaTime;
            }
        }
    }
}