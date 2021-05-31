using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    protected int damage = 3;
    protected float moveSpeed = 12f;

    protected void Start() => Destroy(gameObject, 3f);
    protected void Update() => Move();

    protected abstract void Move();
}