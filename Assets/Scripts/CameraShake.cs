using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    [SerializeField] private float globalShakeForce = 0.001f;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void ShakeCamera(CinemachineImpulseSource impulseSource, float damage)
    {
        impulseSource.GenerateImpulseWithForce(globalShakeForce * (damage * 2));
        //print("shake strength = " + damage * 2);
    }
}
