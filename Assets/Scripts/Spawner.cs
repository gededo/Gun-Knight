using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    void Start()
    {
        bool b = PlayerPrefs.GetString("coins").Contains(gameObject.name);
        if (b)
        {
            gameObject.SetActive(false);
        }
    }
}
