using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public Transform target; //플레이어
    public float targetY; //카메라가 땅을 보지 않게

    public float xRotMax; //카메라 X축 회전범위
    public float rotSpeed; //카메라 회전 속도
    public float scrollSpeed; //스크롤 속도

    private float distance; //플레이어-카메라 거리
    private float minDistance; //플레이어-카메라 최소 거리
    private float maxDistance; //플레이어-카메라 최대 거리

    private float xRot;
    private float yRot;
    private Vector3 targetPos;
    private Vector3 dir;

    private void Start()
    {
        targetY = 1f;
        xRotMax = 200f; //카메라 X축 회전범위
        rotSpeed = 150f; //카메라 회전 속도
        scrollSpeed = 200f; //스크롤 속도

        distance = 5; //플레이어-카메라 거리
        minDistance = 5f; //플레이어-카메라 최소 거리
        maxDistance = 20f; //플레이어-카메라 최대 거리
    }

    private void Update()
    {
        xRot += Input.GetAxis("Mouse Y") * rotSpeed * Time.deltaTime;
        yRot += Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime;
        distance += -Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime;

        xRot = Mathf.Clamp(xRot, -xRotMax, xRotMax);
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        targetPos = target.position + Vector3.up * targetY;

        dir = Quaternion.Euler(-xRot, yRot, 0f) * Vector3.forward;
        transform.position = targetPos + dir * -distance;
    }

    private void LateUpdate()
    {
        transform.LookAt(targetPos);
    }
}