using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraFollow : MonoBehaviour
{
    public float offsetDirection;
    public float smoothTime = 0.25f;
    public float minY = -4f;
    public float maxY = 0f;
    public bool canControl = true;
    public Transform target;
    public Vector3 offset = new Vector3(0f, 4f, -10f);

    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        target = GameObject.Find("Player").transform;
    }

    private void FixedUpdate()
    {
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && canControl)
        {
            offsetDirection = Input.GetKey(KeyCode.A) ? -2.2f : 2.2f;
        }
        offset = new Vector3(offsetDirection, 4f, -10f);

        Vector3 targetposition = target.position + offset;
        targetposition.y = Mathf.Clamp(targetposition.y, minY, maxY);
        transform.position = Vector3.SmoothDamp(transform.position, targetposition, ref velocity, smoothTime);
    }
}
