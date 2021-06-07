using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    private GameObject target;

    [SerializeField]
    private float smoothing = 5.0f;

    [SerializeField]
    private Vector3 offset;

    private void Start()
    {
        target = FindObjectOfType<PlayerCtrl>().gameObject;
        //offset = transform.position - target.transform.position;
    }


    private void FixedUpdate()
    {
        Vector3 newCamPos = target.transform.position + offset;

        transform.position = Vector3.Lerp(transform.position, newCamPos, smoothing * Time.deltaTime);
    }
}