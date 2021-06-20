using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public static CamFollow Instance;
    private GameObject target;
    private GameObject door;

    public bool isPlayer;

    [SerializeField]
    private float smoothing = 5.0f;

    [SerializeField]
    private Vector3 offset;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        isPlayer = true;
        target = FindObjectOfType<PlayerCtrl>().gameObject;
        door = GameObject.Find("DoorParent");
        //offset = transform.position - target.transform.position;
    }

    private void FixedUpdate()
    {
        if(isPlayer)
        {
            Vector3 newCamPos = target.transform.position + offset;
            transform.position = Vector3.Lerp(transform.position, newCamPos, smoothing * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, door.transform.position, smoothing * Time.deltaTime);
        }
    }
}