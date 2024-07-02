using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCollider : MonoBehaviour
{
    public GameObject TooltipBox;
    public TooltipManager tooltipScript;

    void Start()
    {
        tooltipScript = TooltipBox.GetComponent<TooltipManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            tooltipScript.isNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            tooltipScript.isNear = false;
        }
    }
}
