﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public static BulletSpawner Instance;
    public HpBar hpBar;

    private bool isShot;

    private int health;

    private float spawnRateMin = 0.5f;
    private float spawnRateMax = 3f;

    private Transform target;
    private float spawnRate = 0f;
    private float spawnTimer = 0f;

    private void Awake() => Instance = this;

    private void Start()
    {
        isShot = true;

        health = 10;
        spawnTimer = 0f;

        spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        target = FindObjectOfType<PlayerCtrl>().transform;
    }

    private void Update()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }

        if(PlayerCtrl.Instance.health <= 0)
        {
            isShot = false;
        }

        Shot();
    }

    private void Shot()
    {
        if (isShot)
        {
            if (spawnTimer >= spawnRate)
            {
                spawnTimer = 0f;

                GameObject bullet = Instantiate(Resources.Load<GameObject>("EnemyBullet"), transform.position, transform.rotation);
                bullet.transform.LookAt(target);

                spawnRate = Random.Range(spawnRateMin, spawnRateMax);
            }
            else
            {
                spawnTimer += Time.deltaTime;
            }
        }
    }

    public void Hit(int _damage)
    {
        health -= _damage;
        hpBar.SetHp(health);

        GameObject blood = Instantiate(Resources.Load<GameObject>("Particles/Blood"), transform.position, Quaternion.identity);

        Destroy(blood, 1f);
    }
}