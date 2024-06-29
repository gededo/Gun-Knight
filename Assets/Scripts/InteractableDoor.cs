using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDoor : MonoBehaviour
{
    public bool inDoor = false;

    public Transform output;
    public Transform player;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inDoor){
            player.transform.position = output.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            inDoor = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            inDoor = false;
        }
    }

}
