using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckInArea : MonoBehaviour
{
    public BoxCollider2D boxCollider;
    public TextMeshPro counterText;
    public LayerMask targetLayer;
    public List<GameObject> detectedObjects;
    public bool stopCounting = false;
    public CameraTrigger cameraTrigger;
    public GameObject closedDoor;
    public GameObject openDoor;
    //public string targetTag;
    Vector2 boxPos;
    Vector2 boxSize;

    void Start()
    {
        detectedObjects = new List<GameObject>();
        boxCollider = GetComponent<BoxCollider2D>();
        boxPos = new Vector2(transform.position.x + boxCollider.offset.x, transform.position.y + boxCollider.offset.y);
        boxSize = new Vector2(boxCollider.size.x, boxCollider.size.y);
        Collider2D[] hit = Physics2D.OverlapBoxAll(boxPos, boxSize, 0, targetLayer);
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i] != null && hit[i].gameObject.tag == "Enemy")
            {
                if (!hit[i].gameObject.GetComponent<Enemy>().isDead)
                {
                    detectedObjects.Add(hit[i].gameObject);

                }
            }
        }
        counterText.text = detectedObjects.Count.ToString();
    }

    void Update()
    {
        if (!stopCounting && detectedObjects.Count > 0)
        {
            for (int i = 0; i < detectedObjects.Count; i++)
            {
                if (detectedObjects[i] != null && detectedObjects[i].gameObject.GetComponent<Enemy>().isDead)
                {
                    detectedObjects.RemoveAt(i);
                }
            }
            counterText.text = detectedObjects.Count.ToString();
        }
        else if(!stopCounting)
        {
            closedDoor.SetActive(false);
            openDoor.SetActive(true);
            StartCoroutine(cameraTrigger.changeTarget(cameraTrigger.cameraScript.target, cameraTrigger.targetTransform, cameraTrigger.holdTime));
            stopCounting = true;
        }
    }
}
