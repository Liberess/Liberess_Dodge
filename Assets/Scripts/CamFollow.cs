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

    [SerializeField]
    private Vector3 bossOffset;

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

        StartCoroutine(Spawn());
    }

    private void FixedUpdate()
    {
        if(isPlayer)
        {
            if(!GameManager3.Instance.isBoss)
            {
                Vector3 newCamPos = target.transform.position + offset;
                transform.position = Vector3.Lerp(transform.position, newCamPos, smoothing * Time.deltaTime);
            }
            else
            {
                Vector3 newCamPos = target.transform.position + bossOffset;
                transform.position = Vector3.Lerp(transform.position, newCamPos, smoothing * Time.deltaTime);
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, door.transform.position, smoothing * Time.deltaTime);
        }
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(1.5f);

        isPlayer = false;
        smoothing = 0.7f;

        yield return new WaitForSeconds(6f);

        isPlayer = true;
        smoothing = 2f;

        yield return new WaitForSeconds(2f);

        smoothing = 5f;
        GameManager3.Instance.isPlay = true;
    }
}