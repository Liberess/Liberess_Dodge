using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    public int damage = 2;
    public float moveSpeed = 20f;

    protected void Update() => Move();

    protected abstract void Move();
}