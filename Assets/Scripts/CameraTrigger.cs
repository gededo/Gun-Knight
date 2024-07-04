using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraTrigger : MonoBehaviour
{
    public float holdTime;
    public GameObject player;
    public GameObject mainCamera;
    public Transform targetTransform;

    bool hasTriggered = false;
    public CameraFollow cameraScript;
    PlayerController playerScript;

    void Start()
    {
        mainCamera = GameObject.Find("Main Camera");
        cameraScript = mainCamera.GetComponent<CameraFollow>();

        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && hasTriggered == false)
        {
            PlayerController playerScript = collision.gameObject.GetComponent<PlayerController>();
            if(playerScript.isDead == false)
            {
                hasTriggered = true;
                StartCoroutine(changeTarget(cameraScript.target, targetTransform, holdTime));
            }
        }
    }

    public IEnumerator changeTarget(Transform oldTarget, Transform targetTransform, float holdTime)
    {
        float oldSpeed = playerScript.speed;

        playerScript.speed = 0f;
        cameraScript.canControl = false;
        cameraScript.target = targetTransform;

        yield return new WaitForSeconds(holdTime);

        cameraScript.target = oldTarget;
        cameraScript.canControl = true;

        playerScript.speed = oldSpeed;
    }
}
