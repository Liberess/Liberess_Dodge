using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 objectPos = transform.position;

        mousePos.z = objectPos.z - Camera.main.transform.position.z;

        Vector3 target = Camera.main.ScreenToWorldPoint(mousePos);

        float dx = target.x - objectPos.x;
        float dy = target.y - objectPos.y;

        float rotateDegree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, rotateDegree, 0f);
    }
}