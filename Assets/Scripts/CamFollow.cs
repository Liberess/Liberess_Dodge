using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public GameObject target;
    public float smoothing = 5.0f;

    private Vector3 offset;

    private void Start() => offset = transform.position - target.transform.position;

    private void FixedUpdate()
    {
        Vector3 newCamPos = target.transform.position + offset;

        transform.position = Vector3.Lerp(transform.position, newCamPos, smoothing * Time.deltaTime);
    }
}