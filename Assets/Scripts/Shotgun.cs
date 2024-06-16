using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Shotgun : Guns
{
    public float fovAngle = 60f;
    public float fovRange = 5f;
    public Transform shotgunTip;
    public LayerMask enemyLayer;
    public GameObject particleBullets;

    void Awake()
    {
        GunDamage = 1f;
    }

    void Update()
    {

    }

    public void Shoot(bool facingRight) 
    {
        if (facingRight) 
        {
            Instantiate(particleBullets, (shotgunTip.position + new Vector3(0.8f, 0, 0)), Quaternion.Euler(0, 90, 0));
        }
        else
        {
            Instantiate(particleBullets, (shotgunTip.position + new Vector3(-0.8f, 0, 0)), Quaternion.Euler(0, -90, 0));
        }

        float tempGunDamage = GunDamage;

        Vector2 direction = facingRight ? Vector2.right : Vector2.left;
        Collider2D[] circleRange = Physics2D.OverlapCircleAll(shotgunTip.position, fovRange, enemyLayer);
        foreach (Collider2D hit in circleRange)
        {
            Enemy enemyScript = hit.GetComponent<Enemy>();

            Vector2 directionToEnemy = (hit.transform.position - shotgunTip.position).normalized;
            float angleToEnemy = Vector2.Angle(direction, directionToEnemy);

            if (angleToEnemy < fovAngle / 2)
            {
                float distanceToEnemy = Vector3.Distance(hit.transform.position, shotgunTip.position);
                switch (distanceToEnemy)
                {
                    case < 2f:
                        GunDamage *= 10f;
                        break;
                    case < 3.5f:
                        GunDamage *= 6f;
                        break;
                    case < 4.3f:
                        GunDamage *= 4f;
                        break;
                    case <= 5.5f:
                        GunDamage *= 1f;
                        break;
                }
                enemyScript.TakeDamage(GunDamage);
                GunDamage = tempGunDamage;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector3 fovLine1 = Quaternion.AngleAxis(fovAngle / 2, Vector3.forward) * transform.right * fovRange;
        Vector3 fovLine2 = Quaternion.AngleAxis(-fovAngle / 2, Vector3.forward) * transform.right * fovRange;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(shotgunTip.position, fovLine1);
        Gizmos.DrawRay(shotgunTip.position, fovLine2);
    }

}
