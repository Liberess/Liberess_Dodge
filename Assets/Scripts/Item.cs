using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    int healAmount = 20;

    private void Update()
    {
        transform.Rotate(0f, 15f * Time.deltaTime, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerCtrl.Instance.GetHeal(healAmount);
            Destroy(gameObject);
        }
    }
}